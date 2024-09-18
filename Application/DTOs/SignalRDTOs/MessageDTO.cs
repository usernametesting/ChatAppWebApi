using Domain.Enums.MessageEnums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.SignalRDTOs;

public class MessageDTO
{
    public string? Message { get; set; }
    public MessageType MessageType { get; set; }
    public bool IsSender { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? toUserId { get; set; }
    public MessageState State { get; set; }
    public IFormFile? File { get; set; }
}
