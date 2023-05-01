﻿using Identity.Application.Models.Requests;

namespace Identity.Application.Services;

public interface IJwtTokenHandlerService
{
    Task<AuthenticationResponseModel?> AuthenticateAccount(AuthenticationRequestModel authRequest);
    
    Task<AuthJwtResponseModel> RegisterAccount(RegisterRequestModel registerRequest);
}