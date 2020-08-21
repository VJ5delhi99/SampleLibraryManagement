using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Data.Models
{
    public  class Overdue
    {
        public int Id { get; set; }
        [Required] public string OverDueType { get; set; }
        [Required]  public int Charges { get; set; }

        [Required]        
        public LibraryAsset LibraryAsset { get; set; }

        [Required]
        public LibraryCard LibraryCard { get; set; }
    }

    public enum OverdueType
    {
        Lost,
        DelayPenalty
    }
}
