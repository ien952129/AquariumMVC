using AquariumMVC.DTO;

namespace AquariumMVC.Interface
{
    public interface orderdetailInterface
    {
        IEnumerable<orderdetailDTO> getOderDetail(string guid);
    }
}
