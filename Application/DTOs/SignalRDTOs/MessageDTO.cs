using Domain.Enums.MessageEnums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.DTOs.SignalRDTOs;

public class MessageDTO
{

    public string? Message { get; set; }

    public MessageType MessageType { get; set; }

    public bool IsSender { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ToUserId { get; set; }

    public MessageState State { get; set; }
}
