using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AquariumMVC.DTO
{
    public class orderdetailDTO
    {
        public int id { get; set; }

       
        public string OrderGuid { get; set; }

        
        public string Account { get; set; }

        
        public string P_id { get; set; }

        
        public string Name { get; set; }

        public int? Price { get; set; }

        public int? Qty { get; set; }

        public bool? IsApproved { get; set; }

        public int? A_id { get; set; }
    }
}
