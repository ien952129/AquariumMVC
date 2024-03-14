using AquariumMVC.DTO;
using AquariumMVC.Interface;
using AquariumMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AquariumMVC.functionService
{
    public class FishService: FishInterface
    {
        private readonly aquariumwebsiteContext db;
        private readonly ILogger<FishService> _logger;
        public FishService(aquariumwebsiteContext db, ILogger<FishService> logger)
        {
            this.db = db;
            _logger = logger;
        }

        public alldetail FishDTOtoalldetail(FisfDTO d)
        {
            var res = new alldetail()
            {
                A_id = d.L_id,
                P_id = d.P_id,
                Type = d.Type,
                Name = d.Name,
                Kind = d.Kind,
                Size = d.Size,
                Price = d.Price,
                Amount = d.Amount,
                Memo = d.Memo,
                Img = d.Img
            };
            return res;
        }

        public async Task<FisfDTO> getFish(string fPId, int aid)
        {
            var product = await db.Fish.Where(a => a.P_id == fPId && a.L_id == aid).Select(m => new FisfDTO
            {
                Name = m.Name,
                Kind = m.Kind,
                L_id = m.L_id,
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

        public OperationResult Getfish_sync_And_remove(string fPId, int? aid)
        {
            OperationResult res = new OperationResult();
            try
            {
                if (aid.HasValue)
                {
                    // 如果 aid 有值，获取特定的设备
                    var product = db.Fish
                        .Where(a => a.P_id == fPId && a.L_id == aid.Value)
                        .SingleOrDefault();


                    db.Fish.Remove(product);
                    res.Success = true;
                    res.Type = true;
                    res.Message = "刪除成功";


                }
                else
                {
                    var product = db.Fish.Where(a => a.P_id == fPId).ToList();
                    foreach (var i in product)
                    {
                        db.Fish.Remove(i);
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

        public List<string> getKind()
        {
            return db.Fish.Select(d => d.Kind).Distinct().ToList();
        }
    }
}
