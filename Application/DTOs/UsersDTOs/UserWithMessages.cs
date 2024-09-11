using Application.DTOs.SignalRDTOs;
using ETicaretAPI.Domain.Entities.Identity;

namespace Application.DTOs.UsersDTOs;

public class UserWithMessages
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public bool IsOnline { get; set; }
    public List<MessageDTO> Messages { get; set; }
}
