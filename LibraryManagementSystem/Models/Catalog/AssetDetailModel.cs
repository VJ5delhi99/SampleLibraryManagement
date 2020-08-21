using LibraryManagementSystem.Data.Models;
using System.Collections.Generic; 

namespace LibraryManagementSystem.Models.Catalog
{
    public class AssetDetailModel
    {
        public int AssetId { get; set; }
        public string Title { get; set; }
        public string AuthorOrDirector { get; set; } 
        public int Year { get; set; }
        public string Isbn { get; set; } 
        public string Status { get; set; }
        public decimal Cost { get; set; } 
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public Checkout LatestCheckout { get; set; }
        public LibraryCard CurrentAssociatedLibraryCard { get; set; }
        public IEnumerable<CheckoutHistory> CheckoutHistory { get; set; }
        public IEnumerable<AssetHoldModel> CurrentHolds { get; set; }
    }

    public class AssetHoldModel
    {
        public string UserName { get; set; }
        public string HoldPlaced { get; set; }
    }
}