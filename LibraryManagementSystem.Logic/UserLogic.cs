using LibraryManagementSystem.Data;
using LibraryManagementSystem.Logic.Interfaces;
using LibraryManagementSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;


namespace LibraryManagementSystem.Logic
{
    public class UserLogic : IUserLogic
    {
        private readonly LibraryDbContext _libraryDbContext;
        public UserLogic(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        public void Add(User user)
        {
            _libraryDbContext.Users.Add(user);
            _libraryDbContext.SaveChanges();
        }

        public User Get(int id)
        {
            return _libraryDbContext.Users
               .Include(a => a.LibraryCard) 
               .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _libraryDbContext.Users
               .Include(a => a.LibraryCard);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int userid)
        {
              var cardId = _libraryDbContext.Users
                .Include(a => a.LibraryCard)
                .FirstOrDefault(a => a.Id == userid)?
                .LibraryCard.Id;

            return _libraryDbContext.CheckoutHistories
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.CheckedOut);
        }

        public IEnumerable<Checkout> GetCheckouts(int id)
        {
            var userCardId = Get(id).LibraryCard.Id;
            return _libraryDbContext.Checkouts
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(v => v.LibraryCard.Id == userCardId);
        }

        public IEnumerable<Hold> GetHolds(int userId)
        {
            var cardId = _libraryDbContext.Users
               .Include(a => a.LibraryCard)
               .FirstOrDefault(a => a.Id == userId)?
               .LibraryCard.Id;

            return _libraryDbContext.Holds
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.HoldPlaced);
        }
    }
}
