using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Data.Models
{
   public class Charges
    {
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int Penalty { get; set; }
    }
}
