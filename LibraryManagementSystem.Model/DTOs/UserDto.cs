using System;

namespace LibraryManagementSystem.Model.DTOs
{
    public class UserDto {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Telephone { get; set; }
        public string Gender { get; set; }
        public LibraryCardDto LibraryCard { get; set; } 
    }
}