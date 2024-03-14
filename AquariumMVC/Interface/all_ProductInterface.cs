using AquariumMVC.DTO;
using AquariumMVC.Models;

namespace AquariumMVC.Interface
{
    public interface all_ProductInterface
    {
        IEnumerable<string> getType();
        allProductDTO product_exist(string name);
        string addproduct_returnPID(addproduct add);
        IEnumerable<allProductDTO> GetAllProducts();
        Task<allProductDTO> getProductbyPID_Async(string pid);
        Task<string> Updateall_Product(alldetail d);
        string removeProduct(string pid);
        bool hasOtherProducts(string pid);
    }
}
