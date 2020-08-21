using LibraryManagementSystem.Data; 
using LibraryManagementSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Logic.Helpers;
using AutoMapper;
using System.Threading.Tasks;
using LibraryManagementSystem.Model.DTOs;
using System;
using System.Data.Entity;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Logic.Interfaces;

namespace LibraryManagementSystem.Logic
{
    public class BookLogic : BaseLibraryLogic, IBookLogic
    {
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IMapper _mapper;
        private readonly Paginator<Book> _paginator;
        public BookLogic(LibraryDbContext libraryDbContext,
            IMapper mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _paginator = new Paginator<Book>();
        }

        public async Task<ServiceResult<int>> Add(Book newBook)
        {
            _libraryDbContext.Books.Add(newBook);
            await _libraryDbContext.SaveChangesAsync();
            return new ServiceResult<int>
            {
                Data = newBook.Id
            };
        }

        /// <summary>
        /// Gets a Book Asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult<BookDto> Get(int id)
        {
            try
            {
                var book =   _libraryDbContext.Books.FirstOrDefault(b => b.Id == id);
                var bookDto = _mapper.Map<BookDto>(book);
                return new ServiceResult<BookDto>
                {
                    Data = bookDto,
                    Error = null
                };
            }
            catch (ArgumentNullException ex)
            {
                return HandleDatabaseError<BookDto>(ex);
            }
        }
        public async Task<PagedLogicResult<BookDto>> GetAll(int page, int perPage)
        {
            var books = _libraryDbContext.Books;

            try
            {
                var pageOfBooks = await _paginator
                    .BuildPageResult(books, page, perPage, b => b.Author)
                    .ToListAsync();

                var paginatedBooks = _mapper.Map<List<BookDto>>(pageOfBooks);

                var paginationResult = new PaginationResult<BookDto>
                {
                    Results = paginatedBooks,
                    PerPage = perPage,
                    PageNumber = page
                };

                return new PagedLogicResult<BookDto>
                {
                    Data = paginationResult,
                    Error = null
                };
            }
            catch (Exception ex)
            {
                return HandleDatabaseCollectionError<BookDto>(ex);
            }
        }


        public async Task<PagedLogicResult<BookDto>> GetByAuthor(
            string author, int page, int perPage)
        {
            var books = _libraryDbContext.Books;

            try
            {
                var pageOfBooks = _paginator
                    .BuildPageResult(
                        books,
                        page,
                        perPage,
                        b => b.Author.Contains(author),
                        b => b.Author);

                var paginatedBooks = _mapper
                    .Map<List<BookDto>>(await pageOfBooks.ToListAsync());

                var paginationResult = new PaginationResult<BookDto>
                {
                    Results = paginatedBooks,
                    PerPage = perPage,
                    PageNumber = page
                };

                return new PagedLogicResult<BookDto>
                {
                    Data = paginationResult,
                    Error = null
                };
            }
            catch (Exception e)
            {
                return new PagedLogicResult<BookDto>
                {
                    Data = null,
                    Error = new LogicError
                    {
                        Message = e.Message,
                        Stacktrace = e.StackTrace
                    }
                };
            }
        }

        public IEnumerable<Book> GetByISBN(string isbn)
        {
            return _libraryDbContext.Books.Where(a => a.ISBN == isbn);
        }
    }
}
