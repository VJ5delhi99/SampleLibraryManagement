using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Models
{
    public class Book : LibraryAsset
    {
        [Required] [Display(Name = "ISBN #")] public string ISBN { get; set; }
        [Required] public string Author { get; set; } 
        [Required] public string Publisher { get; set; }

        public static explicit operator Book(Task<LibraryAsset> v)
        {
            throw new NotImplementedException();
        }
    }
}
