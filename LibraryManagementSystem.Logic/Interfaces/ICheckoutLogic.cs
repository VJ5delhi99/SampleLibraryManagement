using LibraryManagementSystem.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagementSystem.Model.DTOs;

namespace LibraryManagementSystem.Logic.Interfaces
{
  public  interface ICheckoutLogic
    {
        IEnumerable<Checkout> GetAll();
        Task<ServiceResult<CheckoutDto>> Get(int id);
        Task<ServiceResult<int>> Add(CheckoutDto newCheckoutDto);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);
        ServiceResult<bool> CheckOutItem(int assetId, int libraryCardId);
        ServiceResult<bool> CheckInItem(int assetId);
        Checkout GetLatestCheckout(int id);
        int GetNumberOfCopies(int id);
        ServiceResult<bool> IsCheckedOut(int libraryAssetId);
        string GetCurrentUser(int id);
        void PlaceHold(int assetId, int libraryCardId);

        string GetCurrentHoldUser(int id);
        string GetCurrentHoldPlaced(int id); 
        IEnumerable<Hold> GetCurrentHolds(int id);

        void MarkLost(int id);
        void MarkFound(int id);
    }
}
