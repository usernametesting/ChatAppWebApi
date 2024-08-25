using Application.DTOs.Common;
using Application.DTOs.Common.Bases;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatApis.Helpers;

public class HttpResult : ControllerBase
{
    public async Task<IActionResult> Result(IServiceResult result)
    {
        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                return Ok(result);
            case HttpStatusCode.Created:
                return Created(string.Empty, result); 
            case HttpStatusCode.Accepted:
                return Accepted(result);
            case HttpStatusCode.NoContent:
                return NoContent();
            case HttpStatusCode.BadRequest:
                return BadRequest(result);
            case HttpStatusCode.Unauthorized:
                return Unauthorized(result);
            case HttpStatusCode.Forbidden:
                return Forbid();
            case HttpStatusCode.NotFound:
                return NotFound(result);
            case HttpStatusCode.Conflict:
                return Conflict(result);
            case HttpStatusCode.UnprocessableEntity:
                return UnprocessableEntity(result);
            case HttpStatusCode.InternalServerError:
                return StatusCode(500, result);
            default:
                return StatusCode((int)result.StatusCode, result); 
        }
    }

   


}
