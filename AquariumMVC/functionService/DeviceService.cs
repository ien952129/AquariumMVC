using AquariumMVC.DTO;
using AquariumMVC.Interface;
using AquariumMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace AquariumMVC.functionService
{
    public class DeviceService:DeviceInterface
    {
        private readonly aquariumwebsiteContext db;
        private readonly ILogger<DeviceService> _logger;
        public DeviceService(aquariumwebsiteContext db, ILogger<DeviceService> logger)
        {
            this.db = db;
            _logger = logger;
        }

        public List<string> getKind()
        {
            return db.Device.Select(d => d.Kind).Distinct().ToList();
        }
        public IEnumerable<DeviceDTO> GetAllDevices() {
            var res=db.Device.Select(m=>new DeviceDTO
            {
                Name = m.Name,
                Kind = m.Kind,
                D_id=m.D_id,
                P_id=m.P_id,
                Price = m.Price,
                Type = m.Type,
                Amount = m.Amount,
                Size = m.Size,
                Memo = m.Memo,
                Img = m.Img,
            }).ToList();
            return res;
        }

        public async Task<DeviceDTO> getDevice(string fPId, int aid)
        {
            var product = await db.Device.Where(a => a.P_id == fPId && a.D_id == aid).Select(m => new DeviceDTO
            {
                Name = m.Name,
                Kind = m.Kind,
                D_id = m.D_id,
                P_id = m.P_id,
                Price = m.Price,
                Type = m.Type,
                Amount = m.Amount,
                Size = m.Size,
                Memo = m.Memo,
                Img = m.Img,
            }).SingleOrDefaultAsync();
            return product;
        }


        public alldetail DeviceDTOtoalldetail(DeviceDTO d)
        {
            var res = new alldetail()
            {
                A_id = d.D_id,
                P_id = d.P_id,
                Type = d.Type,
                Name = d.Name,
                Kind = d.Kind,
                Size = d.Size ,
                Price = d.Price,
                Amount = d.Amount,
                Memo = d.Memo,
                Img = d.Img
            };
            return res;
        }

        public OperationResult Getdevuce_sync_And_remove(string fPId, int? aid)
        {
            OperationResult res = new OperationResult();
            try
            {
                if (aid.HasValue)
                {
                    // 如果 aid 有值，获取特定的设备
                    var product = db.Device
                        .Where(a => a.P_id == fPId && a.D_id == aid.Value)
                        .SingleOrDefault();


                    db.Device.Remove(product);
                    res.Success = true;
                    res.Type=true;
                    res.Message = "刪除成功";
                    

                }
                else
                {
                    var product = db.Device.Where(a => a.P_id == fPId).ToList();
                    foreach (var i in product)
                    {
                        db.Device.Remove(i);
                    }
                    res.Success = true;
                    res.Type = false;
                    res.Message = "刪除成功";
                    

                }
                db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
                res.Type = false;
                _logger.LogError("Error occurred while removing device: {0}", ex.Message);
                return res;
            }
            
            
        }

        
    }
}
