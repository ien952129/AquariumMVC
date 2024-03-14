using AquariumMVC.DTO;
using AquariumMVC.Interface;
using AquariumMVC.Models;

namespace AquariumMVC.functionService
{
    public class orderdetailService : orderdetailInterface
    {
        private readonly aquariumwebsiteContext db;
        public orderdetailService(aquariumwebsiteContext db)
        {
            this.db = db;
        }

        public IEnumerable<orderdetailDTO> getOderDetail(string guid)
        {
            return (db.OrderDetail.Where(m => m.OrderGuid == guid).Select(a=>new orderdetailDTO
            {
                id=a.id,
                OrderGuid=a.OrderGuid,
                Account=a.Account,
                P_id=a.P_id,
                Name=a.Name,
                Price=a.Price,
                Qty=a.Qty,
                IsApproved=a.IsApproved,
                A_id=a.A_id
            })).ToList();
        }
    }
}
