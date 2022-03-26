using Microsoft.Extensions.Configuration;
using OrmWithout.DataAccess.Abstract;
using OrmWithout.Models.Entities;
using OrmWithout.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OrmWithout.DataAccess.Concrete
{
    public class OrmWithoutDAL : IOrmWithoutDAL
    {
        private readonly IConfiguration _configuration;

        public OrmWithoutDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> AddInventorySales(InventorySales inventorySales)
        {
            try
            {
                string sqlDataSoruce = _configuration.GetConnectionString("SqlCon");
                using (var conn = new SqlConnection(sqlDataSoruce))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"INSERT INTO dbo.InventorySales(ProductId, StoreId, Date, SalesQuantity,Stock)
VALUES (@ProductId, @StoreId, @Date, @SalesQuantity,@Stock)";

                    cmd.Parameters.AddWithValue("@ProductId", inventorySales.ProductId);
                    cmd.Parameters.AddWithValue("@StoreId", inventorySales.StoreId);
                    cmd.Parameters.AddWithValue("@Date", inventorySales.Date);
                    cmd.Parameters.AddWithValue("@SalesQuantity", inventorySales.SalesQuantity);
                    cmd.Parameters.AddWithValue("@Stock", inventorySales.Stock);

                    var dataReader = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(dataReader);
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> DeleteInventorySale(int inventoryId)
        {
            try
            {
                string sqlDataSoruce = _configuration.GetConnectionString("SqlCon");
                using (var conn = new SqlConnection(sqlDataSoruce))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"DELETE FROM dbo.InventorySales WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", inventoryId);

                    var dataReader = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(dataReader);
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<BestSellerProductModel> GetBestSellerProduct()
        {
            string sqlDataSoruce = _configuration.GetConnectionString("SqlCon");
            List<BestSellerProductModel> results = new List<BestSellerProductModel>();
            using (var conn = new SqlConnection(sqlDataSoruce))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"select b.Id,b.ProductName, SUM(SalesQuantity) as Total From dbo.InventorySales a
inner join dbo.Products b on a.ProductId=b.Id
group by b.Id,a.ProductId,b.ProductName
order by b.Id asc";
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BestSellerProductModel newItem = new BestSellerProductModel();

                        newItem.Id = dr.GetInt32(0);
                        newItem.ProductName = dr.GetString(1);
                        newItem.Total = dr.GetInt32(2);

                        results.Add(newItem);
                    }
                }
                conn.Close();
            }
            var maxObject = results.OrderByDescending(item => item.Total).First();
            return maxObject;
        }

        public async Task<StoresProfitModel> GetMostProfitStore()
        {
            string sqlDataSoruce = _configuration.GetConnectionString("SqlCon");
            List<StoresProfitModel> results = new List<StoresProfitModel>();
            using (var conn = new SqlConnection(sqlDataSoruce))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"select a.Id, a.StoreName,Sum((c.SalesPrice - c.Cost) * b.SalesQuantity) as Profit from dbo.Stores a
inner join dbo.InventorySales b on a.Id=b.StoreId
inner join dbo.Products c on b.ProductId=c.Id
group by a.Id,a.StoreName
order by a.Id asc";
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        StoresProfitModel newItem = new StoresProfitModel();

                        newItem.Id = dr.GetInt32(0);
                        newItem.StoreName = dr.GetString(1);
                        newItem.Profit = dr.GetInt32(2);

                        results.Add(newItem);
                    }
                }
                conn.Close();
            }
            var maxObject = results.OrderByDescending(item => item.Profit).First();
            return maxObject;
        }

        public async Task<StoresProfitModel> GetProfitStores(string storeName)
        {

            string sqlDataSoruce = _configuration.GetConnectionString("SqlCon");
            StoresProfitModel result = new StoresProfitModel();
            using (var conn = new SqlConnection(sqlDataSoruce))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"select a.Id, a.StoreName,Sum((c.SalesPrice - c.Cost) * b.SalesQuantity) as Profit from dbo.Stores a
inner join dbo.InventorySales b on a.Id=b.StoreId
inner join dbo.Products c on b.ProductId=c.Id
group by a.Id,a.StoreName
order by a.Id asc";
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    List<StoresProfitModel> dataResult = new List<StoresProfitModel>();
                    while (dr.Read())
                    {
                        StoresProfitModel newItem = new StoresProfitModel();

                        newItem.Id = dr.GetInt32(0);
                        newItem.StoreName = dr.GetString(1);
                        newItem.Profit = dr.GetInt32(2);

                        dataResult.Add(newItem);
                    }
                    result = dataResult.FirstOrDefault(x => x.StoreName == storeName);
                }
                conn.Close();

            }
            if (result == null)
            {
                return new StoresProfitModel();
            }
            return result;
        }

        public async Task<IList<SalesHistoryModel>> GetSalesHistory()
        {
            string sqlDataSoruce = _configuration.GetConnectionString("SqlCon");
            List<SalesHistoryModel> results = new List<SalesHistoryModel>();
            using (var conn = new SqlConnection(sqlDataSoruce))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"Select a.Id, c.StoreName,b.ProductName,a.SalesQuantity From dbo.InventorySales a 
inner join dbo.Products b on a.ProductId = b.Id
inner join dbo.Stores c on a.StoreId = c.Id
group by a.Id,c.StoreName,b.ProductName,a.SalesQuantity
order by a.Id asc";
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        SalesHistoryModel newItem = new SalesHistoryModel();

                        newItem.Id = dr.GetInt32(0);
                        newItem.StoreName = dr.GetString(1);
                        newItem.ProductName = dr.GetString(2);
                        newItem.SalesQuantity = dr.GetInt32(3);

                        results.Add(newItem);
                    }
                }
                conn.Close();
            }
            return results;
        }

        public async Task<InventorySales> UpdateInventorySales(InventorySales inventorySales)
        {
            try
            {
                string sqlDataSoruce = _configuration.GetConnectionString("SqlCon");
                using (var conn = new SqlConnection(sqlDataSoruce))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"UPDATE dbo.InventorySales
SET ProductId = @ProductId, StoreId = @StoreId, Date = @Date, SalesQuantity = @SalesQuantity, Stock = @Stock
WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", inventorySales.Id);
                    cmd.Parameters.AddWithValue("@ProductId", inventorySales.ProductId);
                    cmd.Parameters.AddWithValue("@StoreId", inventorySales.StoreId);
                    cmd.Parameters.AddWithValue("@Date", inventorySales.Date);
                    cmd.Parameters.AddWithValue("@SalesQuantity", inventorySales.SalesQuantity);
                    cmd.Parameters.AddWithValue("@Stock", inventorySales.Stock);

                    var dataReader = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(dataReader);
                    conn.Close();
                }
                return inventorySales;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
