using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueSolving.Application.Commands.SendForRevision;

public class SendForRevisionHandler : ICommandHandler<Guid, SendForRevisionCommand>
{
    private readonly IUserIssueRepository _userIssueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SendForRevisionCommand> _validator;
    private readonly ILogger<SendForRevisionHandler> _logger;

    public SendForRevisionHandler(
        IUserIssueRepository userIssueRepository,
        [FromKeyedServices(Modules.IssueSolving)] IUnitOfWork unitOfWork,
        IValidator<SendForRevisionCommand> validator,
        ILogger<SendForRevisionHandler> logger)
    {
        _userIssueRepository = userIssueRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SendForRevisionCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var userIssueResult = await _userIssueRepository
            .GetUserIssueById(UserIssueId.Create(command.UserIssueId), cancellationToken);

        if (userIssueResult.IsFailure)
        {            
            return userIssueResult.Error.ToErrorList();
        }

        var sendForRevisionResult = userIssueResult.Value.SendForRevision();

        if (sendForRevisionResult.IsFailure)
        {
            return sendForRevisionResult.Error.ToErrorList();
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "User Issue {userIssue} is sent for review",
            userIssueResult.Value.Id.Value);
        
        return userIssueResult.Value.Id.Value;
    }
}
