using System.Security.Claims;
using Chores.Application.Common.Authentication;

namespace Chores.Api.Common;

internal sealed class ApiAuthenticationInfo : IAuthenticationInfo
{
    #region construction

    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiAuthenticationInfo(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    private ClaimsPrincipal? User => _httpContextAccessor
        .HttpContext?
        .User;

    public string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);
}