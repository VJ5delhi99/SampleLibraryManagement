using AutoMapper;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Model.DTOs;

namespace LibraryManagementSystem.Data.Mapping
{ 
      /// <summary>
      /// AutoMapper Mapping Profile for de/serializing entity models to DTOs
      /// </summary>
    public class EntityMappingProfile : Profile
    { 
            public EntityMappingProfile()
            {
                CreateMap<Book, BookDto>().ReverseMap();
                CreateMap<LibraryAsset, LibraryAssetDto>().ReverseMap();
                 CreateMap<Status, StatusDto>().ReverseMap();
               CreateMap<Checkout, CheckoutDto>().ReverseMap();
            CreateMap<LibraryCard, LibraryCardDto>().ReverseMap();
            CreateMap<Hold, HoldDto>().ReverseMap();
        }
         
    }
}
