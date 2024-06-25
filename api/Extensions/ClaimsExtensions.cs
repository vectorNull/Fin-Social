using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace api.Extensions
{
    public static class ClaimsExtensions
    {
        public static string? GetUsername(this ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var usernameClaimTypes = new[]
            {
                ClaimTypes.Name,
                ClaimTypes.GivenName,
            };

            foreach (var claimType in usernameClaimTypes)
            {
                var claim = user.Claims.SingleOrDefault(x => x.Type.Equals(claimType));
                if (claim != null)
                {
                    return claim.Value;
                }
            }

            return null;
        }
    }
}
