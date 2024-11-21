using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Accounts.Application.Commands.CompleteUploadPhoto;
using SachkovTech.Accounts.Application.Commands.EnrollParticipant;
using SachkovTech.Accounts.Application.Commands.Login;
using SachkovTech.Accounts.Application.Commands.Logout;
using SachkovTech.Accounts.Application.Commands.RefreshTokens;
using SachkovTech.Accounts.Application.Commands.Register;
using SachkovTech.Accounts.Application.Commands.StartUploadFile;
using SachkovTech.Accounts.Application.Queries.GetUserById;
using SachkovTech.Accounts.Application.Requests;
using SachkovTech.Accounts.Contracts.Requests;
using SachkovTech.Accounts.Infrastructure.Providers;
using SachkovTech.Core.Models;
using SachkovTech.SharedKernel;
using SachkovTech.Accounts.Presentation.Providers;
using SachkovTech.Framework;
using SachkovTech.Framework.Authorization;
using SachkovTech.Framework.Models;

namespace SachkovTech.Accounts.Presentation;

public class AccountsController : ApplicationController
{
    private readonly HttpContextProvider _httpContextProvider;

    public AccountsController(HttpContextProvider httpContextProvider)
    {
        _httpContextProvider = httpContextProvider;
    }

    [HttpPost("test")]
    [Permission(Permissions.Issues.ReadIssue)]
    public async Task<IActionResult> Test([FromServices] UserScopedData user, CancellationToken cancellationToken)
    {

        return Ok("test");
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new RegisterUserCommand(
                request.Email,
                request.UserName,
                request.Password),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new LoginCommand(request.Email, request.Password),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        var setRefreshSessionCookieRes = _httpContextProvider.SetRefreshSessionCookie(result.Value.RefreshToken);

        if (setRefreshSessionCookieRes.IsFailure)
        {
            return setRefreshSessionCookieRes.Error.ToResponse();
        }

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens([FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken)
    {
        var getRefreshSessionCookieRes = _httpContextProvider.GetRefreshSessionCookie();

        if (getRefreshSessionCookieRes.IsFailure)
        {
            return Unauthorized();
        }

        var result = await handler.Handle(
            new RefreshTokensCommand(getRefreshSessionCookieRes.Value),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        var setRefreshSessionCookieRes = _httpContextProvider.SetRefreshSessionCookie(result.Value.RefreshToken);

        if (setRefreshSessionCookieRes.IsFailure)
        {
            return setRefreshSessionCookieRes.Error.ToResponse();
        }

        return Ok(result.Value);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromServices] LogoutHandler handler,
        CancellationToken cancellationToken)
    {
        var getRefreshSessionCookieRes = _httpContextProvider.GetRefreshSessionCookie();

        if (getRefreshSessionCookieRes.IsFailure)
        {
            return Unauthorized();
        }

        var result = await handler.Handle(
            new LogoutCommand(getRefreshSessionCookieRes.Value),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        var deleteRefreshSessionCookieRes = _httpContextProvider.DeleteRefreshSessionCookie();

        if (deleteRefreshSessionCookieRes.IsFailure)
        {
            return deleteRefreshSessionCookieRes.Error.ToResponse();
        }

        return Ok();
    }

    [Permission(Permissions.Accounts.EnrollAccount)]
    [HttpPut("student-role")]
    public async Task<ActionResult> EnrollParticipant(
        [FromBody] EnrollParticipantRequest request,
        [FromServices] EnrollParticipantHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new EnrollParticipantCommand(request.Email);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(
        [FromRoute] Guid userId,
        [FromServices] GetUserByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new GetUserByIdQuery(userId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("/start-upload-photo")]
    [Permission(Permissions.Issues.UpdateIssue)]
    public async Task<IActionResult> StartUploadPhoto(
        [FromServices] StartUploadPhotoHandler handler,
        [FromServices] UserScopedData userScopedData,
        [FromBody] FileMetadataRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new StartUploadPhotoCommand(
            userScopedData.UserId,
            request.FileName,
            request.ContentType,
            request.FileSize);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("/complete-upload-photo")]
    [Permission(Permissions.Issues.UpdateIssue)]
    public async Task<IActionResult> CompleteUploadPhoto(
        [FromServices] CompleteUploadPhotoHandler handler,
        [FromServices] UserScopedData userScopedData,
        [FromBody] CompleteMultipartUploadRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CompleteUploadPhotoCommand(
            userScopedData.UserId,
            request.FileMetadata.FileName,
            request.FileMetadata.ContentType,
            request.FileMetadata.FileSize,
            request.UploadId,
            request.Parts);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            result.Error.ToResponse();

        return Ok();
    }
}