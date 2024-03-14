using AquariumMVC.DTO;

namespace AquariumMVC.Interface
{
    public interface memberInterface
    {
        IEnumerable<memberDTO> GetMembers();
        string addmember(memberDTO DTO);
        string delete_Member(string id);
        Task<memberDTO> GetMemberByAsync(string id);
        Task<string> UpdateMemberAsync(string id, memberDTO m);
    }
}