using AquariumMVC.Interface;
using AquariumMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AquariumMVC.functionService
{
    public class addproductService : addProductInterface
    {
        private readonly aquariumwebsiteContext db;
        private readonly DeviceInterface df;
        public addproductService(aquariumwebsiteContext db, DeviceInterface df)
        {
            this.db = db;
            this.df = df;
        }
        public async Task<string> editDevice(string pid, int aid, alldetail d)
        {

            var product = await db.Device.Where(a => a.P_id == d.P_id && a.D_id == d.A_id).SingleOrDefaultAsync();
            if (product == null)
            {
                return "查無此產品";
            }
            product.P_id = d.P_id;
            product.D_id = d.A_id;
            product.Price = d.Price;
            product.Name = d.Name;
            product.Type = d.Type;
            product.Kind = d.Kind;
            product.Amount = d.Amount;
            product.Size = d.Size;
            product.Memo = d.Memo;
            product.Img = d.Img;
            await db.SaveChangesAsync();
            return "修改成功";
        }

        

       

        public async Task<string> editFeeds(string pid, int aid, alldetail d)
        {
            var product = await db.Feeds.Where(a => a.P_id == d.P_id && a.F_id == d.A_id).SingleOrDefaultAsync();
            if (product == null)
            {
                return "查無資料";
            }
            product.P_id = d.P_id;
            product.F_id = d.A_id;
            product.Price = d.Price;
            product.Name = d.Name;
            product.Type = d.Type;
            product.Kind = d.Kind;
            product.Amount = d.Amount;
            product.Size = d.Size;
            product.Memo = d.Memo;
            product.Img = d.Img;
            await db.SaveChangesAsync();
            return "修改成功";
        }

        

        public async Task<string> editFish(string pid, int aid, alldetail d)
        {
            var product = await db.Fish.Where(a => a.P_id == d.P_id && a.L_id == d.A_id).SingleOrDefaultAsync();
            if (product == null)
            {
                return "查無資料";
            }
            product.P_id = d.P_id;
            product.L_id = d.A_id;
            product.Price = d.Price;
            product.Name = d.Name;
            product.Type = d.Type;
            product.Kind = d.Kind;
            product.Amount = d.Amount;
            product.Size = d.Size;
            product.Memo = d.Memo;
            product.Img = d.Img;
            await db.SaveChangesAsync();
            return "修改成功";
        }
    }
}
