using AquariumMVC.DTO;
using AquariumMVC.Models;

namespace AquariumMVC.Interface
{
    public interface FeedsInterface
    {
        List<string> getKind();
        Task<FeedsDTO> getFeeds(string fPId, int aid);
        alldetail FeedsDTOtoalldetail(FeedsDTO d);
        OperationResult Getfeeds_sync_And_remove(string fPId, int? aid);
    }
}
