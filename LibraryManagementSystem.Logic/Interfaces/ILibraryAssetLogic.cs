using LibraryManagementSystem.Data.Models;
using System.Collections.Generic;
using LibraryManagementSystem.Model.DTOs;

namespace LibraryManagementSystem.Logic.Interfaces
{
    public interface ILibraryAssetLogic
    {
        IEnumerable<LibraryAsset> GetAll();
        ServiceResult<BookDto> Get(int id);
        void Add(LibraryAsset newAsset);
        string GetAuthor(int id);  
        string GetTitle(int id);
        string GetISBN(int id); 
        LibraryCard GetLibraryCardByAssetId(int id);
    }
}
