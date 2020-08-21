using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Models
{
    public class LibraryCard
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Overdue Fees")] public decimal Fees { get; set; }

        [Display(Name = "Card Issued Date")] public DateTime Created { get; set; }

        [Display(Name = "Books on rent")] public virtual IEnumerable<Checkout> Checkouts { get; set; }
    }
}
