using Domain.Entities;
using System.Security.Claims;

namespace Application.AuthProvide
{
    public interface ITokenService
    {
        string GenerateJWT(IEnumerable<Claim>? additionalClaims = null);
        string GenerateJWTWithUser(User user, IEnumerable<Claim>? additionalClaims = null);
        string GenerateJWTWithCustomer(Customer customer, IEnumerable<Claim>? additionalClaims = null);

    }
}
