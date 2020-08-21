﻿namespace LibraryManagementSystem.Models.Catalog
{
    public class AssetIndexListingModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; } 
        public int NumberOfCopies { get; set; }
        public int CopiesAvailable { get; set; }
    }
}