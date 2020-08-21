using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Model.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Logic.Interfaces
{
    public interface IHoldLogic
    {
         Task<PagedLogicResult<HoldDto>> GetCurrentHolds(int id, int page, int perPage);
        Task<ServiceResult<bool>> PlaceHold(int assetId, int libraryCardId);
        Task<ServiceResult<string>> GetCurrentHoldPatron(int holdId);
         Task<ServiceResult<string>> GetCurrentHoldPlaced(int holdId);
        ServiceResult<Hold> GetEarliestHold(int libraryAssetId);
    }
}
