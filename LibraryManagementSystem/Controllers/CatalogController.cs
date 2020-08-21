 
using LibraryManagementSystem.Models.Catalog;
using LibraryManagementSystem.Models.CheckoutModels;
using System.Linq;
using System.Web.Mvc;
using LibraryManagementSystem.Logic.Interfaces;

namespace LibraryManagementSystem.Controllers
{
    public class CatalogController : Controller
    {
        // GET: Catalog
        private readonly ILibraryAssetLogic _assetsLogic;
        private readonly ICheckoutLogic _checkoutsLogic;

        public CatalogController(ILibraryAssetLogic assetsLogic, ICheckoutLogic checkoutsLogic)
        {
            _assetsLogic = assetsLogic;
            _checkoutsLogic = checkoutsLogic;
        }

        public ActionResult Index()
        {
            var assetModels = _assetsLogic.GetAll();

            var listingResult = assetModels
                .Select(a => new AssetIndexListingModel
                {
                    Id = a.Id,
                    ImageUrl = a.ImageUrl,
                    Author = _assetsLogic.GetAuthor(a.Id), 
                    CopiesAvailable = _checkoutsLogic.GetNumberOfCopies(a.Id), // Remove
                    Title = _assetsLogic.GetTitle(a.Id), 
                    NumberOfCopies = _checkoutsLogic.GetNumberOfCopies(a.Id)
                }).ToList();

            var model = new AssetIndexModel
            {
                Assets = listingResult
            };

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var asset = _assetsLogic.Get(id);

            var currentHolds = _checkoutsLogic.GetCurrentHolds(id).Select(a => new AssetHoldModel
            {
                HoldPlaced = _checkoutsLogic.GetCurrentHoldPlaced(a.Id),
                UserName = _checkoutsLogic.GetCurrentHoldUser(a.Id)
            });

            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Data.Title, 
                Year = asset.Data.Year,
                Cost = asset.Data.Cost,
                Status = asset.Data.Status.Name,
                ImageUrl = asset.Data.ImageUrl,
                AuthorOrDirector = _assetsLogic.GetAuthor(id), 
                CheckoutHistory = _checkoutsLogic.GetCheckoutHistory(id),
                CurrentAssociatedLibraryCard = _assetsLogic.GetLibraryCardByAssetId(id),
                Isbn = _assetsLogic.GetISBN(id),
                LatestCheckout = _checkoutsLogic.GetLatestCheckout(id),
                CurrentHolds = currentHolds,
                UserName = _checkoutsLogic.GetCurrentUser(id)
            };

            return View(model);
        }

        public ActionResult Checkout(int id)
        {
            var asset = _assetsLogic.Get(id);

            var model = new CheckoutModel
            {
                AssetId = id, 
                LibraryCardId = "",
                IsCheckedOut = _checkoutsLogic.IsCheckedOut(id).Data
            };
            return View(model);
        }

        public ActionResult Hold(int id)
        {
            var asset = _assetsLogic.Get(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.Data.ImageUrl,
                Title = asset.Data.Title,
                LibraryCardId = "",
                HoldCount = _checkoutsLogic.GetCurrentHolds(id).Count()
            };
            return View(model);
        }

        public ActionResult CheckIn(int id)
        {
            _checkoutsLogic.CheckInItem(id);
            return RedirectToAction("Details", new { id });
        }

        public ActionResult MarkLost(int id)
        {
            _checkoutsLogic.MarkLost(id);
            return RedirectToAction("Details", new { id });
        }

        public ActionResult MarkFound(int id)
        {
            _checkoutsLogic.MarkFound(id);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public ActionResult PlaceCheckout(int assetId, int libraryCardId)
        {
            _checkoutsLogic.CheckOutItem(assetId, libraryCardId);
            return RedirectToAction("Details", new { id = assetId });
        }

        [HttpPost]
        public ActionResult PlaceHold(int assetId, int libraryCardId)
        {
            _checkoutsLogic.PlaceHold(assetId, libraryCardId);
            return RedirectToAction("Details", new { id = assetId });
        }
    }
}