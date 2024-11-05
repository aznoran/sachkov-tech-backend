using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Infrastructure.Providers;

public class HttpContextProvider
{
    private const string REFRESH_TOKEN = "refreshToken";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Result<Guid,Error> GetRefreshSessionCookie()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return Errors.General.Failure();
        }
        
        if (!_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(REFRESH_TOKEN, out var refreshToken))
        {
            return Errors.General.NotFound(null, REFRESH_TOKEN);
        }

        return Guid.Parse(refreshToken);
    }
    
    public UnitResult<Error> SetRefreshSessionCookie(Guid refreshToken)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return Errors.General.Failure();
        }
        
        _httpContextAccessor.HttpContext.Response.Cookies.Append(REFRESH_TOKEN, refreshToken.ToString());

        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> DeleteRefreshSessionCookie()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return Errors.General.Failure();
        }

        _httpContextAccessor.HttpContext.Response.Cookies.Delete(REFRESH_TOKEN);

        return UnitResult.Success<Error>();
    }
}