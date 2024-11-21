using CSharpFunctionalExtensions;
using SachkovTech.Issues.Application.Features.Issue.Queries.GetIssueById;
using SachkovTech.Issues.Application.Features.Modules.Queries.GetIssueByPosition;
using SachkovTech.Issues.Contracts;
using SachkovTech.Issues.Contracts.Responses;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Presentation;

public class IssuesContract : IIssuesContract
{
    private readonly GetIssueByIdHandler _getIssueByIdHandler;
    private readonly GetIssueByPositionHandler _getIssueByPositionHandler;

    public IssuesContract(
        GetIssueByIdHandler getIssueByIdHandler,
        GetIssueByPositionHandler getIssueByPositionHandler)
    {
        _getIssueByIdHandler = getIssueByIdHandler;
        _getIssueByPositionHandler = getIssueByPositionHandler;
    }
    
    public async Task<Result<IssueResponse, ErrorList>> GetIssueById(
        Guid issueId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetIssueByIdQuery(issueId);

        return await _getIssueByIdHandler.Handle(query, cancellationToken);
    }
    
    public async Task<Result<Guid, ErrorList>> GetIssueByPosition(
        ModuleId moduleId,
        Position position,
        CancellationToken cancellationToken = default)
    {
        var query = new GetIssueByPositionQuery(moduleId, position);

        return await _getIssueByPositionHandler.Handle(query, cancellationToken);
    }
}