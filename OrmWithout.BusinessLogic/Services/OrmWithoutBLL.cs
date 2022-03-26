using OrmWithout.BusinessLogic.Abstract;
using OrmWithout.DataAccess.Abstract;
using OrmWithout.Models.Entities;
using OrmWithout.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrmWithout.BusinessLogic.Services
{
    public class OrmWithoutBLL : IOrmWithoutBLL
    {
        private readonly IOrmWithoutDAL _orm;

        public OrmWithoutBLL(IOrmWithoutDAL orm)
        {
            _orm = orm;
        }

        public async Task<bool> AddInventorySales(InventorySales inventorySales)
        {
            return await _orm.AddInventorySales(inventorySales);
        }

        public async Task<bool> DeleteInventorySale(int inventoryId)
        {
            return await _orm.DeleteInventorySale(inventoryId);
        }

        public async Task<BestSellerProductModel> GetBestSellerProduct()
        {
            return await _orm.GetBestSellerProduct();
        }

        public async Task<StoresProfitModel> GetMostProfitStore()
        {
            return await _orm.GetMostProfitStore();
        }

        public async Task<StoresProfitModel> GetProfitStores(string storeName)
        {
            return await _orm.GetProfitStores(storeName);
        }

        public async Task<IList<SalesHistoryModel>> GetSalesHistory()
        {
            return await _orm.GetSalesHistory();
        }

        public async Task<InventorySales> UpdateInventorySales(InventorySales inventorySales)
        {
            return await _orm.UpdateInventorySales(inventorySales);
        }
    }
}
