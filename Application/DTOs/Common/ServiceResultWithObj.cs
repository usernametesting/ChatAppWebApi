using Application.DTOs.Common.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Common;

public class ServiceResult<T> : IServiceResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? resultObj { get; set; }

    public HttpStatusCode StatusCode { get; set; }
}
