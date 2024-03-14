using AquariumMVC.DTO;
using AquariumMVC.Models;

namespace AquariumMVC.Interface
{
    public interface FishInterface
    {
        List<string> getKind();
        Task<FisfDTO> getFish(string fPId, int aid);
        alldetail FishDTOtoalldetail(FisfDTO d);
        OperationResult Getfish_sync_And_remove(string fPId, int? aid);
    }
}
