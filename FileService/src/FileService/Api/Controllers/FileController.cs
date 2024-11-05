using FileService.Api.Contracts;
using FileService.Application.Commands.UploadFiles;
using FileService.Application.Queries.GetLinkFiles;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    [HttpPost("{ownerTypeName}/{ownerId:guid}")]
    public async Task<IActionResult> UploadFiles(
        [FromRoute] string ownerTypeName,
        [FromRoute] Guid ownerId,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UploadFilesCommand(ownerTypeName, ownerId, files);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetLinks(
        [FromQuery] IEnumerable<Guid> fileIds,
        [FromServices] GetLinkFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetLinkFilesQuery(fileIds);

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }
}
