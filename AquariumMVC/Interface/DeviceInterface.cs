using AquariumMVC.DTO;
using AquariumMVC.Models;

namespace AquariumMVC.Interface
{
    public interface DeviceInterface
    {
        List<string> getKind();
        IEnumerable<DeviceDTO> GetAllDevices();
        Task<DeviceDTO> getDevice(string fPId, int aid);
        alldetail DeviceDTOtoalldetail(DeviceDTO d);
        OperationResult Getdevuce_sync_And_remove(string fPId, int? aid);


    }
}
