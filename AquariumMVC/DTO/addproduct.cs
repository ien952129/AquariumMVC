using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AquariumMVC.DTO
{
    public class addproduct
    {

        [StringLength(10)]
        [Unicode(false)]
        public string Type { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(30)]
        public string Kind { get; set; }

        [StringLength(10)]
        public string Size { get; set; }

        public int? Price { get; set; }

        public int? Amount { get; set; }

        public string Memo { get; set; }

        [StringLength(30)]
        public string Img { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
