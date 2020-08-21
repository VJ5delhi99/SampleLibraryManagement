using LibraryManagementSystem.Data.Models;
using System.Collections.Generic;

namespace LibraryManagementSystem.Logic.Interfaces
{
    public interface ILibraryCardLogic
    {
        IEnumerable<LibraryCard> GetAll();
        LibraryCard Get(int id);
        void Add(LibraryCard newCard);
    }
}
