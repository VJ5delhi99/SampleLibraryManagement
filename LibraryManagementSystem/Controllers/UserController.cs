using System.Linq;
using System.Web.Mvc;
using LibraryManagementSystem.Logic.Interfaces;
using LibraryManagementSystem.Models.User;

namespace LibraryManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserLogic _userLogic;

        public UserController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        public ActionResult Index()
        {
            var allUsers = _userLogic.GetAll();

            var userDetailModels = allUsers
                .Select(p => new UserDetailModel
                {
                    Id = p.Id,
                    LastName = p.LastName ?? "No First Name Provided",
                    FirstName = p.FirstName ?? "No Last Name Provided",
                    LibraryCardId = p.LibraryCard?.Id,
                    OverdueFees = p.LibraryCard?.Fees
                }).ToList();

            var model = new UserIndexModel
            {
                Users = userDetailModels
            };

            return View(model);
        }

        public  ActionResult Detail(int id)
        {
            var patron = _userLogic.Get(id);

            var model = new UserDetailModel
            {
                Id = patron.Id,
                LastName = patron.LastName ?? "No Last Name Provided",
                FirstName = patron.FirstName ?? "No First Name Provided",
                Address = patron.Address ?? "No Address Provided", 
                MemberSince = patron.LibraryCard?.Created,
                OverdueFees = patron.LibraryCard?.Fees,
                LibraryCardId = patron.LibraryCard?.Id,
                Telephone = string.IsNullOrEmpty(patron.Telephone) ? "No Telephone Number Provided" : patron.Telephone,
                AssetsCheckedOut = _userLogic.GetCheckouts(id).ToList(),
                CheckoutHistory = _userLogic.GetCheckoutHistory(id),
                Holds = _userLogic.GetHolds(id)
            };

            return View(model);
        }
    }
}