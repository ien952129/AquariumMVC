using AquariumMVC.DTO;
using AquariumMVC.Models;
namespace AquariumMVC.Interface
{
    public interface addProductInterface
    {
        Task<string> editDevice(string pid, int aid ,alldetail d);
        Task<string> editFish(string pid, int aid, alldetail d);
        Task<string> editFeeds(string pid, int aid, alldetail d);
    }
}
