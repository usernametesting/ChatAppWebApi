using Application.DTOs.ExternalServiceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services.ExternalServices;

public interface IMailService
{
    Task SendMail(SendMailDTO _dto);
}
