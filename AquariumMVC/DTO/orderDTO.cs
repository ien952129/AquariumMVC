using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AquariumMVC.DTO
{
    public class orderDTO
    {
        public int O_id { get; set; }

        
        public string OrderGuid { get; set; }

        
        public string Account { get; set; }

        
        public string Receiver { get; set; }

        
        public string ReceiverTel { get; set; }

        
        public string Address { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }

        public int? total_price { get; set; }
    }
}
