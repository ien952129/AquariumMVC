using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AquariumMVC.DTO
{
    public class memberDTO
    {
        public string Account { get; set; }

        
        public string Password { get; set; }

        
        public string Name { get; set; }

        
        public string Email { get; set; }

       
        public string Address { get; set; }

        public bool IsAdmin { get; set; }
    }
}
