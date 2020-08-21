using LibraryManagementSystem.Data.Models;
using System.Collections.Generic;

namespace LibraryManagementSystem.Logic.Interfaces
{
    public  interface IStatusLogic
    {
        IEnumerable<Status> GetAll();
        Status Get(int id);
        void Add(Status newStatus);
    }
}
