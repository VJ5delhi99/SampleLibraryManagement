using LibraryManagementSystem.Data.Models;
using System.Collections.Generic;

namespace LibraryManagementSystem.Logic.Interfaces
{
    public interface IAssetType
    {
        IEnumerable<AssetType> GetAll();
        AssetType Get(int id);
        void Add(AssetType newType);
    }
}
