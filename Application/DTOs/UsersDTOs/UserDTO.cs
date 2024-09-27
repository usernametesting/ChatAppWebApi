using Application.DTOs.SignalRDTOs;
using Domain.Entities.ConcretEntities;
using ETicaretAPI.Domain.Entities.Identity;

namespace Application.DTOs.UsersDTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public bool IsOnline { get; set; }
    public bool IsMutualFriendship { get; set; }
    public string? LastActivityDate { get; set; }
    public int UnreadMessageCount { get; set; }
    public string? ProfImageUrl { get; set; }
    public string? Email { get; set; }

    public List<MessageDTO>? Messages { get; set; }
    public List<Status> ? Statuses { get; set; }
}
