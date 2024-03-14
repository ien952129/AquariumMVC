using AquariumMVC.DTO;
using AquariumMVC.Interface;
using AquariumMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AquariumMVC.functionService
{
    public class memberService : memberInterface
    {
        private readonly aquariumwebsiteContext db;
        public memberService(aquariumwebsiteContext db)
        {
            this.db = db;
        }
        public Memberdata DTOtoData(memberDTO m)
        {
            Memberdata a= new Memberdata() {
                Account = m.Account,
                Password = m.Password,
                Name = m.Name,
                Email = m.Email,
                Address = m.Address,
                IsAdmin = m.IsAdmin
            };
            return a;
        }
        public memberDTO DatatoDTO(Memberdata m)
        {
            memberDTO a = new memberDTO()
            {
                Account = m.Account,
                Password = m.Password,
                Name = m.Name,
                Email = m.Email,
                Address = m.Address,
                IsAdmin = m.IsAdmin
            };
            return a;
        }

        public string addmember(memberDTO DTO)
        {
            try
            {
                Memberdata d = DTOtoData(DTO);
                db.Memberdata.Add(d);
                db.SaveChanges();
                return "新增成功";
            }catch (Exception e)
            {
                return "新增失敗"+e.Message;
            }
            
        }

        public IEnumerable<memberDTO> GetMembers()
        {
            return db.Memberdata.Select(m => new memberDTO
            {
                Account=m.Account,
                Password=m.Password,
                Name=m.Name,Email=m.Email,
                Address=m.Address,
                IsAdmin=m.IsAdmin
            }).ToList();
        }

        public string delete_Member(string id)
        {
            try
            {
                var remove = db.Memberdata.Where(a => a.Account == id).SingleOrDefault();
                db.Memberdata.Remove(remove);
                db.SaveChanges();
                return "刪除成功";
            }catch(Exception e) { return "刪除失敗" + e.Message; }

        }
        public async Task<memberDTO> GetMemberByAsync(string id)
        {
            try
            {
                var a = await db.Memberdata.Where(a => a.Account == id).SingleOrDefaultAsync();
                var tran = DatatoDTO(a);
                return tran;
            }
            catch
            {
                return null;
            }
        }
        public async Task<string> UpdateMemberAsync(string id, memberDTO m)
        {
            var memberToUpdate = await db.Memberdata.FindAsync(id);
            if (memberToUpdate == null)
            {
                return "member not found";
            }
            memberToUpdate.Account=m.Account;
            memberToUpdate.Name=m.Name;
            memberToUpdate.Email=m.Email;
            memberToUpdate.Address=m.Address;
            memberToUpdate.Password=m.Password;
            memberToUpdate.IsAdmin=m.IsAdmin;

            

            try
            {
                db.Memberdata.Update(memberToUpdate);
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
