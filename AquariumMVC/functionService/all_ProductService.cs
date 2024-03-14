using AquariumMVC.DTO;
using AquariumMVC.Interface;
using AquariumMVC.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using AquariumMVC.tools;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AquariumMVC.functionService
{
    public class all_ProductService : all_ProductInterface
    {
        private readonly aquariumwebsiteContext db;
        public all_ProductService(aquariumwebsiteContext db)
        {
            this.db = db;
        }

        public IEnumerable<string> getType()
        {
            return db.all_Product.Select(p => p.type).Distinct().ToList();
        }
        public allProductDTO product_exist(string name)
        {
            var res= db.all_Product.Where(a => a.Name == name).Select(m=>new allProductDTO
            {
                P_id=m.P_id,
                Name=m.Name,
                Img=m.Img,
                kind=m.kind,
                type=m.type
            }).FirstOrDefault();
            if (res != null)
            {
                return res;
            }
            else
            {
                return null;
            }
        }
        public string addproduct_returnPID(addproduct add)
        {
            all_Product a = new all_Product();
            a.P_id = layoutTools.produce_Pid(add.Type);
            a.Name = add.Name;
            a.type = add.Type;
            a.kind = add.Kind;
            a.Img = add.Img;
            db.all_Product.Add(a);
            db.SaveChanges();
            return a.P_id;
        }

        public IEnumerable<allProductDTO> GetAllProducts()
        {
            return db.all_Product.Select(m=>new allProductDTO
            {
                P_id = m.P_id,
                Name = m.Name,
                Img = m.Img,
                kind = m.kind,
                type = m.type
            }).ToList();
        }
        public async Task<allProductDTO> getProductbyPID_Async(string pid)
        {
            var all_list = await db.all_Product.Where(a => a.P_id == pid).Select(m=>new allProductDTO
            {
                P_id = m.P_id,
                Name = m.Name,
                Img = m.Img,
                kind = m.kind,
                type = m.type
            }).SingleOrDefaultAsync();
            return all_list;
        }

        public async Task<string> Updateall_Product(alldetail d)
        {
            var all_list = await db.all_Product.Where(a => a.P_id == d.P_id).Select(m => new allProductDTO
            {
                P_id = m.P_id,
                Name = m.Name,
                Img = m.Img,
                kind = m.kind,
                type = m.type
            }).SingleOrDefaultAsync();
            if (all_list != null)
            {
                all_list.type = d.Type;
                all_list.Img = d.Img;
                all_list.Name = d.Name;
                all_list.kind = d.Kind;
                all_list.P_id = d.P_id;
                db.SaveChanges();
                return "修改成功";
            }
            else
            {
                return "目錄修改失敗";
            }
        }
        public string removeProduct(string pid)
        {
            try
            {
                var allcontent = db.all_Product.Where(a => a.P_id == pid).SingleOrDefault();
                db.all_Product.Remove(allcontent);
                db.SaveChanges();
                return "目錄刪除成功";
            }catch (Exception ex)
            {
                return "刪除失敗" + ex.Message;
            }
        }
        public bool hasOtherProducts(string pid)
        {
            bool hasOtherProducts = db.Device.Any(d => d.P_id == pid) ||
                                        db.Fish.Any(f => f.P_id == pid) ||
                                        db.Feeds.Any(f => f.P_id == pid);
            return hasOtherProducts;
        }
    }
    
}
