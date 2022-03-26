using Microsoft.AspNetCore.Mvc;
using OrmWithout.BusinessLogic.Abstract;
using OrmWithout.Models.Entities;
using OrmWithout.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrmWithout.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrmWihoutsController : ControllerBase
    {
        private readonly IOrmWithoutBLL _orm;

        public OrmWihoutsController(IOrmWithoutBLL orm)
        {
            _orm = orm;
        }

        [HttpGet("GetSalesHistory")]
        public async Task<IList<SalesHistoryModel>> GetSalesHistory()
        {
            return await _orm.GetSalesHistory();
        }

        [HttpPost("GetStoresProfit")]
        public async Task<StoresProfitModel> GetProfitStores(string storeName)
        {
            return await _orm.GetProfitStores(storeName);
        }

        [HttpGet("GetMostProfitStore")]
        public async Task<StoresProfitModel> GetMostProfitStore()
        {
            return await _orm.GetMostProfitStore();
        }

        [HttpGet("GetBestSellerProduct")]
        public async Task<BestSellerProductModel> GetBestSellerProduct()
        {
            return await _orm.GetBestSellerProduct();
        }

        [HttpPost("AddInventorySales")]
        public async Task<bool> AddInventorySales([FromBody] InventorySales inventorySales)
        {
            return await _orm.AddInventorySales(inventorySales);
        }

        [HttpPut("UpdateInventorySales")]
        public async Task<InventorySales> UpdateInventorySales([FromBody] InventorySales inventorySales)
        {
            return await _orm.UpdateInventorySales(inventorySales);
        }

        [HttpDelete("DeleteInventorySale")]
        public async Task<bool> DeleteInventorySale(int inventoryId)
        {
            return await _orm.DeleteInventorySale(inventoryId);
        }
    }
}
