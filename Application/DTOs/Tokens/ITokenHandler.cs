using Domain.Entities;
using ETicaretAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Tokens;

public interface ITokenHandler
{
    Token CreateAccessToken(AppUser appUser, int second = 3600);
    string CreateRefreshToken();

}
