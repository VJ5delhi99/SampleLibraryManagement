using LibraryManagementSystem.Data;
using LibraryManagementSystem.Logic.Interfaces;
using LibraryManagementSystem.Data.Models;  
using System.Collections.Generic;
using System.Linq; 
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Model.DTOs;

namespace LibraryManagementSystem.Logic
{
    public class LibraryAssetLogic : ILibraryAssetLogic
    {
        private readonly IMapper _mapper;
        private readonly LibraryDbContext _libraryDbContext;
        public LibraryAssetLogic(LibraryDbContext libraryDbContext, IMapper mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
        }
        public void Add(LibraryAsset newAsset)
        {
            _libraryDbContext.LibraryAssets.Add(newAsset);
            _libraryDbContext.SaveChanges();
        }

        private Task<LibraryAsset> GetLibraryAssetsAsync(int id)
        {
            return _libraryDbContext.LibraryAssets
             .Include(a => a.Status)
             .Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public  ServiceResult<BookDto> Get(int id)
        {
            var asset = GetLibraryAssetsAsync(id); 

            var assetDto = _mapper.Map<BookDto>(asset.Result);
            return new ServiceResult<BookDto>
            {
                Data = assetDto,
                Error = null
            }; 
        }

        public IEnumerable<LibraryAsset> GetAll()
        {

            return _libraryDbContext.LibraryAssets
                .Include(a => a.Status) ;
        } 

       
        public LibraryCard GetLibraryCardByAssetId(int id)
        {
            return _libraryDbContext.Checkouts.Where(a => a.LibraryAsset.Id==id).Select(a=>a.LibraryCard).FirstOrDefault(); 
        }

        public  string  GetISBN(int id)
        {
            var book =   Get(id);
            return book?.Data?.ISBN;
        }

        public string GetTitle(int id)
        {
            return _libraryDbContext.LibraryAssets.FirstOrDefault(a => a.Id == id).Title;
        }
         
        public  string  GetAuthor(int id)
        {
            var book =   Get(id);
            return book.Data?.Author;
        }
         
    }
}
