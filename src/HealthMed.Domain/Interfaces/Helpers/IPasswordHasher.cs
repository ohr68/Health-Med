﻿namespace HealthMed.Domain.Interfaces.Helpers;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string password);
}