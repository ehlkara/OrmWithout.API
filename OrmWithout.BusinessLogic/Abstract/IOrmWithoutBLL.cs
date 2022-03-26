using OrmWithout.Models.Entities;
using OrmWithout.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrmWithout.BusinessLogic.Abstract
{
    public interface IOrmWithoutBLL
    {
        Task<IList<SalesHistoryModel>> GetSalesHistory();
        Task<StoresProfitModel> GetProfitStores(string storeName);
        Task<StoresProfitModel> GetMostProfitStore();
        Task<BestSellerProductModel> GetBestSellerProduct();
        Task<bool> AddInventorySales(InventorySales inventorySales);
        Task<InventorySales> UpdateInventorySales(InventorySales inventorySales);
        Task<bool> DeleteInventorySale(int inventoryId);
    }
}
