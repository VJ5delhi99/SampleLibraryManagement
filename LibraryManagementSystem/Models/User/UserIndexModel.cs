using System.Collections.Generic;

namespace LibraryManagementSystem.Models.User
{
    public class UserIndexModel
    {
        public IEnumerable<UserDetailModel> Users { get; set; }
    }
}