using LibraryManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Logic.Interfaces
{
   public interface IUserLogic
    {
        IEnumerable<User> GetAll();
        User Get(int id);
        void Add(User user);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int userid);
        IEnumerable<Hold> GetHolds(int userid);
        IEnumerable<Checkout> GetCheckouts(int userId);
    }
}
