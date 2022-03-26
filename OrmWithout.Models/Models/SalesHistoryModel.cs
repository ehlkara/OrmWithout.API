using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrmWithout.Models.Models
{
    public class SalesHistoryModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string StoreName { get; set; }
        public int SalesQuantity { get; set; }
    }
}
