using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Model.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace LibraryManagementSystem.Logic.Interfaces
{
    public interface IBookLogic
    {
        Task<PagedLogicResult<BookDto>> GetAll(int page, int perPage);
        Task<PagedLogicResult<BookDto>> GetByAuthor(
            string author, int page, int perPage);
        IEnumerable<Book> GetByISBN(string isbn);
        ServiceResult<BookDto> Get(int id);
        Task<ServiceResult<int>> Add(Book newBook);
    }
}
