using ArtVerse.Domain.Entities;
using System.Security.Claims;

namespace ArtVerse.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string email, string fullName, string roleName);
}
