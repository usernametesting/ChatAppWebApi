﻿using Application.Abstractions.Services.Authentications;
using Application.DTOs;
using Application.DTOs.AuthDTOs;
using Application.DTOs.Common;
using Application.DTOs.Common.Bases;
using Application.DTOs.Tokens;
using ETicaretAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services;

public interface IAuthService : IExternalAuthService, IInternalAuthService
{
    Task<IServiceResult> LoginAsync(LoginUserDTO model);
    Task<IServiceResult> RefrehTokenAsync(RefreshTokenDTO refreshToken);
    Task<ServiceResult> RegisterAsync(CreateUserDTO model);

    Task<ServiceResult> ConfirmEmailAsync(int id,string token);
    Task<ServiceResult> Logut();

}
