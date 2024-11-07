namespace TagService.Features.Queries.GetTagsByIdsList;

public record GetTagsByIdsListQuery(IEnumerable<Guid>? Ids);