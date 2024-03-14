using AquariumMVC.DTO;
using AquariumMVC.Interface;
using AquariumMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AquariumMVC.functionService
{
    public class FeedsService:FeedsInterface
    {
        private readonly aquariumwebsiteContext db;
        private readonly ILogger<FeedsService> _logger;
        public FeedsService(aquariumwebsiteContext db,ILogger<FeedsService> _logger)
        {
            this.db = db;
            this._logger = _logger;
        }

        public alldetail FeedsDTOtoalldetail(FeedsDTO d)
        {
            var res = new alldetail()
            {
                A_id = d.F_id,
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

        public async Task<FeedsDTO> getFeeds(string fPId, int aid)
        {
            var product = await db.Feeds.Where(a => a.P_id == fPId && a.F_id == aid).Select(m => new FeedsDTO
            {
                Name = m.Name,
                Kind = m.Kind,
                F_id = m.F_id,
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

        public OperationResult Getfeeds_sync_And_remove(string fPId, int? aid)
        {
            OperationResult res = new OperationResult();
            try
            {
                if (aid.HasValue)
                {
                    // 如果 aid 有值，获取特定的设备
                    var product = db.Feeds
                        .Where(a => a.P_id == fPId && a.F_id == aid.Value)
                        .SingleOrDefault();


                    db.Feeds.Remove(product);
                    res.Success = true;
                    res.Type = true;
                    res.Message = "刪除成功";


                }
                else
                {
                    var product = db.Feeds.Where(a => a.P_id == fPId).ToList();
                    foreach (var i in product)
                    {
                        db.Feeds.Remove(i);
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
            return db.Feeds.Select(d => d.Kind).Distinct().ToList();
        }
    }
}
