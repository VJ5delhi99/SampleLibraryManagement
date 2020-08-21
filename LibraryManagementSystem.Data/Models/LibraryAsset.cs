﻿using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Data.Models
{
    public abstract class LibraryAsset
    {
        public int Id { get; set; }

        [Required] public string Title { get; set; }
        [Required] public int Year { get; set; } // storing only year i.e. 2020
        [Required] public Status Status { get; set; }

        [Required]
        [Display(Name = "Cost of Replacement")]
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; } 
    }
}
