using AquariumMVC.DTO;
using AquariumMVC.Interface;
using AquariumMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace AquariumMVC.functionService
{
    public class orderService : orderInterface
    {
        private readonly aquariumwebsiteContext db;
        public orderService(aquariumwebsiteContext db)
        {
            this.db = db;
        }

        public IEnumerable<orderDTO> getALLOder()
        {
            return db.Order.Select(a => new orderDTO
            {
                O_id = a.O_id,
                OrderGuid = a.OrderGuid,
                Account = a.Account,
                Receiver = a.Receiver,
                ReceiverTel = a.ReceiverTel,
                Address = a.Address,
                Date = a.Date,
                total_price = a.total_price,
            }).ToList();
        }

        public int getOrderPrice(string guid)
        {
            return db.Order.Where(m => m.OrderGuid == guid).Select(m => m.total_price).FirstOrDefault() ?? 0;
        }
        
        public string removeOrder(int orderId)
        {
            string msg = "";
            try
            {
                var orderToRemove = db.Order.SingleOrDefault(o => o.O_id == orderId);
                if (orderToRemove != null)
                {
                    db.Order.Remove(orderToRemove);
                    db.SaveChanges();
                    msg = "移除成功";
                }
                else
                {
                    msg = "未找到指定的訂單";
                }
            }
            catch (Exception ex)
            {
                msg = "移除失敗: " + ex.Message;
            }

            return msg;
        }
        public async Task<orderDTO> GetOrderByAsync(int id)
        {
            try
            {
                var a = await db.Order.Where(a => a.O_id == id).SingleOrDefaultAsync();
                var tran = new orderDTO
                {
                    O_id = a.O_id,
                    OrderGuid = a.OrderGuid,
                    Account = a.Account,
                    Receiver = a.Receiver,
                    ReceiverTel = a.ReceiverTel,
                    Address = a.Address,
                    Date = a.Date,
                    total_price = a.total_price,
                };
                return tran;
            }
            catch
            {
                return null;
            }
        }
        public async Task<string> UpdateOrderAsync(int id, orderDTO orderData)
        {
            var orderToUpdate = await db.Order.FindAsync(id);
            if (orderToUpdate == null)
            {
                return "Order not found";
            }

            orderToUpdate.OrderGuid = orderData.OrderGuid;
            orderToUpdate.Account = orderData.Account;
            orderToUpdate.Receiver = orderData.Receiver;
            orderToUpdate.ReceiverTel = orderData.ReceiverTel;
            orderToUpdate.Address = orderData.Address;
            orderToUpdate.Date = orderData.Date;
            orderToUpdate.total_price = orderData.total_price;

            try
            {
                db.Update(orderToUpdate);
                await db.SaveChangesAsync();
                return "Update successful";
            }
            catch (DbUpdateConcurrencyException)
            {
                return "Update failed";
            }
        }

    }
}
