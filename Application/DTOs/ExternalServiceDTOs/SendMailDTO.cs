using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ExternalServiceDTOs;

public class SendMailDTO
{
    public string Email { get; set; }
    public string CallBackUrl { get; set; }
}
