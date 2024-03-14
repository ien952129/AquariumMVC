using AquariumMVC.DTO;

namespace AquariumMVC.Interface
{
    public interface orderInterface
    {
        IEnumerable<orderDTO> getALLOder();
        int getOrderPrice(string guid);
        string removeOrder(int orderId);
        Task<orderDTO> GetOrderByAsync(int id);
        Task<string> UpdateOrderAsync(int id, orderDTO orderData);
    }
}
