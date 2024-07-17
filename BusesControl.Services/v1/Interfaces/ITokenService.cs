﻿using BusesControl.Entities.Models;

namespace BusesControl.Services.v1.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAcess(UserModel user);
}
