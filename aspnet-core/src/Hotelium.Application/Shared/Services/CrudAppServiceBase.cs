using Hotelium.Shared.Linq;
using Hotelium.Shared.Entities;
using Hotelium.Shared.Services.Dto;
using Hotelium.Shared.Repositories;

namespace Hotelium.Shared.Services;

public abstract class CrudAppServiceBase<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    : ApplicationService<TCategoryName>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
    where TUpdateInput : IEntityDto<TPrimaryKey>
{
    protected readonly IRepository<TEntity, TPrimaryKey> Repository;

    protected CrudAppServiceBase(
        IRepository<TEntity, TPrimaryKey> repository,
        ILogger<TCategoryName> logger,
        IObjectMapper objectMapper
    )
        : base(logger, objectMapper) => Repository = repository;

    protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetAllInput input)
    {
        var sortInput = input as ISortedResultRequest;
        if (sortInput is not null)
            if (!string.IsNullOrWhiteSpace(sortInput.Sorting))
                return query.OrderBy(sortInput.Sorting);

        if (input is ILimitedResultRequest)
            return query.OrderByDescending(e => e.Id);

        return query;
    }

    protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetAllInput input)
    {
        var pagedInput = input as IPagedResultRequest;
        if (pagedInput is not null)
            return query.PageBy(pagedInput);

        var limitedInput = input as ILimitedResultRequest;
        if (limitedInput is not null)
            return query.Take(limitedInput.MaxResultCount);

        return query;
    }

    protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetAllInput input)
        => Repository.GetAll();

    protected virtual TEntityDto MapToEntityDto(TEntity entity)
        => ObjectMapper.Map<TEntityDto>(entity);

    protected virtual TEntity MapToEntity(TCreateInput createInput)
        => ObjectMapper.Map<TEntity>(createInput);

    protected virtual void MapToEntity(TUpdateInput updateInput, TEntity entity)
        => ObjectMapper.Map(updateInput, entity);
}