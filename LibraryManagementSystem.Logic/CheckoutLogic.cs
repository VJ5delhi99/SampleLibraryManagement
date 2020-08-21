using LibraryManagementSystem.Data; 
using LibraryManagementSystem.Data.Models;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using LibraryManagementSystem.Logic.Interfaces;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity.Infrastructure;
using LibraryManagementSystem.Logic.Helpers;
using AutoMapper;
using LibraryManagementSystem.Model.DTOs;

namespace LibraryManagementSystem.Logic
{
    public class CheckoutLogic : ICheckoutLogic
    {
        private readonly LibraryDbContext _libraryDbContext; 
        private readonly IMapper _mapper;
        private readonly Paginator<Hold> _holdsPaginator;
        private readonly Paginator<Checkout> _checkoutPaginator;
        private readonly Paginator<CheckoutHistory> _checkoutHistoryPaginator;
        private readonly IHoldLogic _holdLogic;

        public CheckoutLogic(
            LibraryDbContext context,
            IHoldLogic holdLogic,
            IMapper mapper)
        {
            _libraryDbContext = context;
            _holdLogic = holdLogic;
            _mapper = mapper;
            _holdsPaginator = new Paginator<Hold>();
            _checkoutPaginator = new Paginator<Checkout>();
            _checkoutHistoryPaginator = new Paginator<CheckoutHistory>();
        }
       
         /// <summary>
        /// Add a checkout given a Checkout DTO representing a new instance
        /// </summary>
        /// <param name="newCheckoutDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> Add(CheckoutDto newCheckoutDto) {
            var checkoutEntity = _mapper.Map<Checkout>(newCheckoutDto);
            try {
                  _libraryDbContext.Checkouts.Add(checkoutEntity);
                await _libraryDbContext.SaveChangesAsync();
                return new ServiceResult<int> {
                    Data = checkoutEntity.Id,
                    Error = null
                };
            } catch (Exception ex) when (
                ex is DbUpdateException 
                || ex is DBConcurrencyException) {
                
                return new ServiceResult<int> {
                    Data = 0,
                    Error = new LogicError {
                        Message = ex.Message,
                        Stacktrace = ex.StackTrace
                    }
                };
            }
        }

        /// <summary>
        /// Checks in the given Library Asset ID
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public  ServiceResult<bool> CheckInItem(int assetId)
        {
            try
            {
                var now = DateTime.UtcNow;

                var libraryAsset =  _libraryDbContext.LibraryAssets
                    .FirstOrDefault(a => a.Id == assetId);

                _libraryDbContext.LibraryAssets.AddOrUpdate(libraryAsset);

                // remove any existing checkouts on the item
                var checkout =  _libraryDbContext.Checkouts
                    .FirstOrDefault(a => a.LibraryAsset.Id == assetId);

                if (checkout != null)
                {
                    _libraryDbContext.Checkouts.Remove(checkout);
                }

                // close any existing checkout history
                var history = _libraryDbContext.CheckoutHistories
                    .Include(h => h.LibraryAsset)
                    .Include(h => h.LibraryCard)
                    .FirstOrDefaultAsync(h =>
                        h.LibraryAsset.Id == assetId
                        && h.CheckedIn == null).Result;

                if (history != null)
                {
                    history.CheckedIn = now;
                    _libraryDbContext.CheckoutHistories.AddOrUpdate(history);


                    //TO do 15 needs to be picked from DB or Config file
                    if (DateTime.Now.Subtract(history.CheckedOut).TotalDays > 15)
                    {
                        var charges = _libraryDbContext.Charges.FirstOrDefault(c => c.Type == OverdueType.DelayPenalty.ToString());
                        if (charges != null)
                        {
                            history.LibraryCard.Fees = history.LibraryCard.Fees + charges.Penalty;
                            Overdue overdue = new Overdue
                            {
                                Charges = charges.Penalty,
                                LibraryCard = history.LibraryCard,
                                LibraryAsset = history.LibraryAsset,
                                OverDueType= OverdueType.DelayPenalty.ToString()

                            };
                            _libraryDbContext.Overdues.Add(overdue);
                            _libraryDbContext.LibraryCards.AddOrUpdate(history.LibraryCard);

                        }
                    }
                }


                // if there are current holds, check out the item to the earliest
                // TODO
                var wasCheckedOutToNewHold =  CheckoutToEarliestHold(assetId);

                if (wasCheckedOutToNewHold)
                {
                    return new ServiceResult<bool>
                    {
                        Data = true,
                        Error = null
                    };
                }

                // otherwise, set item status to available
                // TODO magic string
                libraryAsset.Status =  _libraryDbContext.Statuses
                    .FirstOrDefault(a => a.Name == "Available");

                _libraryDbContext.SaveChanges();

                return new ServiceResult<bool>
                {
                    Data = true,
                    Error = null
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<bool>
                {
                    Data = true,
                    Error = null
                };
            }
            
        }
         

        public void PlaceHold(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;

            var asset = _libraryDbContext.LibraryAssets
                .Include(x=>x.Status)
                .First(a => a.Id == assetId);

            var card = _libraryDbContext.LibraryCards
                .First(a => a.Id == libraryCardId);

            _libraryDbContext.LibraryAssets.AddOrUpdate(asset);

            if (asset.Status.Name == "Available")
                asset.Status = _libraryDbContext.Statuses.FirstOrDefault(a => a.Name == "On Hold");

            var hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = card
            };

            _libraryDbContext.Holds.Add(hold);
            _libraryDbContext.SaveChanges();
        }

        /// <summary>
        /// Checks the provided Library Asset out to the provided Library Card
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="libraryCardId"></param>
        /// <returns></returns>
        public ServiceResult<bool> CheckOutItem(int assetId, int libraryCardId)
        {
            try
            {
                var now = DateTime.UtcNow;

                var isAlreadyCheckedOut = IsCheckedOut(assetId);

                if (isAlreadyCheckedOut.Data)
                {
                    return new ServiceResult<bool>
                    {
                        Data = false,
                        // TODO
                        Error = null
                    };
                }

                var libraryAsset = _libraryDbContext.LibraryAssets
                    .Include(a => a.Status)
                    .FirstOrDefault(a => a.Id == assetId);

                _libraryDbContext.LibraryAssets.AddOrUpdate(libraryAsset);

                // TODO
                libraryAsset.Status =  _libraryDbContext.Statuses
                    .FirstOrDefault(a => a.Name == "Checked Out");

                var libraryCard =  _libraryDbContext.LibraryCards
                    .FirstOrDefault(a => a.Id == libraryCardId);

                var checkout = new Checkout
                {
                    LibraryAsset = libraryAsset,
                    LibraryCard = libraryCard,
                    Since = now,
                    Until = GetDefaultDateDue(now)
                };

                _libraryDbContext.Checkouts.Add(checkout);

                var checkoutHistory = new CheckoutHistory
                {
                    CheckedOut = now,
                    LibraryAsset = libraryAsset,
                    LibraryCard = libraryCard
                };

                _libraryDbContext.CheckoutHistories.Add(checkoutHistory);
                 _libraryDbContext.SaveChanges();

                return new ServiceResult<bool>
                {
                    Data = true,
                    Error = null
                };
            }catch(Exception ex)
            {
                return new ServiceResult<bool>
                {
                    Data = true,
                    Error = null
                };
            }
        }

        /// <summary>
        /// Gets default date an asset is due
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        /// TODO Magic Number
        private static DateTime GetDefaultDateDue(DateTime now) => now.AddDays(15);
        /// <summary>
        /// Get the Checkout corresponding to the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<CheckoutDto>> Get(int id)
        {
            var checkout = await _libraryDbContext.Checkouts
                .FirstAsync(p => p.Id == id);

            var checkoutDto = _mapper.Map<CheckoutDto>(checkout);

            return new ServiceResult<CheckoutDto>
            {
                Data = checkoutDto,
                Error = null
            };
        }
        public IEnumerable<Checkout> GetAll()
        {
            return _libraryDbContext.Checkouts;
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {

            return _libraryDbContext.CheckoutHistories
                .Include(a => a.LibraryAsset)
                .Include(c => c.LibraryCard)
                .Where(a => a.LibraryAsset.Id == id);
        }

        public string GetCurrentHoldUser(int id)
        {

            var hold = _libraryDbContext.Holds
                .Include(a=>a.LibraryAsset)
                .Include(c=>c.LibraryCard)
                .Where(v => v.Id == id);

            var cardId = hold
                .Include(h=>h.LibraryCard)
                .Select(a => a.LibraryCard.Id)
                .FirstOrDefault();

            var user = _libraryDbContext.Users 
                .Include(c => c.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return user?.FirstName + " " + user?.LastName;
        }

        public string GetCurrentHoldPlaced(int id)
        {

            var hold = _libraryDbContext.Holds
                .Include(a => a.LibraryAsset)
                .Include(c => c.LibraryCard)
                .Where(v => v.Id == id);

            return hold.Select(a => a.HoldPlaced)
                .FirstOrDefault().ToString(CultureInfo.InvariantCulture);
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {

            return _libraryDbContext.Holds
                .Include(a=>a.LibraryAsset)
                .Where(a => a.LibraryAsset.Id == id);
        }

        public string GetCurrentUser(int id)
        {

            var checkout = _libraryDbContext.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(c => c.LibraryCard)
                .FirstOrDefault(a => a.LibraryAsset.Id == id);

            if (checkout == null) return "Not checked out.";

            var cardId = checkout.LibraryCard.Id;

            var user = _libraryDbContext.Users
                .Include(c=>c.LibraryCard)
                .FirstOrDefault(c => c.LibraryCard.Id == cardId);

            return user?.FirstName + " " + user?.LastName;
        }

        public Checkout GetLatestCheckout(int id)
        {
            return _libraryDbContext.Checkouts
              .Where(c => c.LibraryAsset.Id == id)
              .OrderByDescending(c => c.Since)
              .FirstOrDefault();
        }

        public Task<bool> IsCheckOutAsync(int libraryAssetId)
        {
           return _libraryDbContext.Checkouts
               .AnyAsync(a => a.LibraryAsset.Id == libraryAssetId);
        }
        public int GetNumberOfCopies(int id)
        {
            return _libraryDbContext.LibraryAssets
               .First(a => a.Id == id)
               .NumberOfCopies;
        }

        /// <summary>
        /// Returns true if a given Library Asset ID is checked out
        /// </summary>
        /// <param name="libraryAssetId"></param>
        /// <returns></returns>
        public ServiceResult<bool> IsCheckedOut(int libraryAssetId)
        {
            var isCheckedOut = IsCheckOutAsync(libraryAssetId);

            return new ServiceResult<bool>
            {
                Data = isCheckedOut.Result,
                Error = null
            };
        }

        public void MarkFound(int id)
        {
            var item = _libraryDbContext.LibraryAssets
    .First(a => a.Id == id);

            _libraryDbContext.LibraryAssets.AddOrUpdate(item);
            item.Status = _libraryDbContext.Statuses.FirstOrDefault(a => a.Name == "Available");
            var now = DateTime.Now;

            // remove any existing checkouts on the item
            var checkout = _libraryDbContext.Checkouts
                .FirstOrDefault(a => a.LibraryAsset.Id == id);
            if (checkout != null) _libraryDbContext.Checkouts.Remove(checkout);

            // close any existing checkout history
            var history = _libraryDbContext.CheckoutHistories
                .FirstOrDefault(h =>
                    h.LibraryAsset.Id == id
                    && h.CheckedIn == null);
            if (history != null)
            {
                _libraryDbContext.CheckoutHistories.AddOrUpdate(history);
                history.CheckedIn = now;
            }

            _libraryDbContext.SaveChanges();
        }

        public void MarkLost(int id)
        {
            var item = _libraryDbContext.LibraryAssets
                .First(a => a.Id == id);

            _libraryDbContext.LibraryAssets.AddOrUpdate(item);
            // TODO to add charges if it has been lost by the user
            item.Status = _libraryDbContext.Statuses.FirstOrDefault(a => a.Name == "Lost");

            _libraryDbContext.SaveChanges();
        }

        /// <summary>
        /// Checks the given Library Asset ID out to the next Hold
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        private  bool CheckoutToEarliestHold(int assetId)
        {

            var earliestHold =  _holdLogic.GetEarliestHold(assetId);

            if (earliestHold?.Data == null)
            {
                return false;
            }

            var card = earliestHold.Data.LibraryCard;

            _libraryDbContext.Holds.Remove(earliestHold.Data);
            _libraryDbContext.SaveChanges();

            // TODO
            var checkOutResult =  CheckOutItem(assetId, card.Id);

            return checkOutResult.Data;
        }



        private DateTime GetDefaultCheckoutTime(DateTime now)
        {
            return now.AddDays(15);
        }
    }
}
