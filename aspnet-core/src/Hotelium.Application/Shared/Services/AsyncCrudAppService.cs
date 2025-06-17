using Hotelium.Shared.Entities;
using Hotelium.Shared.Linq;
using Hotelium.Shared.Repositories;
using Hotelium.Shared.Services.Dto;

namespace Hotelium.Shared.Services;

public abstract class AsyncCrudAppService<TCategoryName, TEntity, TEntityDto>
    : AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, int>
    where TEntity : class, IEntity<int>
    where TEntityDto : IEntityDto<int>
{
    protected AsyncCrudAppService(
        IAsyncQueryableExecuter asyncQueryableExecuter,
        IRepository<TEntity, int> repository,
        ILogger<TCategoryName> logger,
        IObjectMapper objectMapper
    )
        : base(asyncQueryableExecuter, repository, logger, objectMapper) { }
}

public abstract class AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey>
    : AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
{
    protected AsyncCrudAppService(
        IAsyncQueryableExecuter asyncQueryableExecuter,
        IRepository<TEntity, TPrimaryKey> repository,
        ILogger<TCategoryName> logger,
        IObjectMapper objectMapper
    )
        : base(asyncQueryableExecuter, repository, logger, objectMapper) { }
}

public abstract class AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput>
    : AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
{
    protected AsyncCrudAppService(
        IAsyncQueryableExecuter asyncQueryableExecuter,
        IRepository<TEntity, TPrimaryKey> repository,
        ILogger<TCategoryName> logger,
        IObjectMapper objectMapper
    )
        : base(asyncQueryableExecuter, repository, logger, objectMapper) { }
}

public abstract class AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
    : AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
    where TGetAllInput : IPagedAndSortedResultRequest
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
   where TCreateInput : IEntityDto<TPrimaryKey>
{
    protected AsyncCrudAppService(
        IAsyncQueryableExecuter asyncQueryableExecuter,
        IRepository<TEntity, TPrimaryKey> repository,
        ILogger<TCategoryName> logger,
        IObjectMapper objectMapper
    )
        : base(asyncQueryableExecuter, repository, logger, objectMapper) { }
}

public abstract class AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    : AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
    where TUpdateInput : IEntityDto<TPrimaryKey>
{
    protected AsyncCrudAppService(
        IAsyncQueryableExecuter asyncQueryableExecuter,
        IRepository<TEntity, TPrimaryKey> repository,
        ILogger<TCategoryName> logger,
        IObjectMapper objectMapper
    )
        : base(asyncQueryableExecuter, repository, logger, objectMapper) { }
}

public abstract class AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
: AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
    where TUpdateInput : IEntityDto<TPrimaryKey>
    where TGetInput : IEntityDto<TPrimaryKey>
{
    protected AsyncCrudAppService(
        IAsyncQueryableExecuter asyncQueryableExecuter,
        IRepository<TEntity, TPrimaryKey> repository,
        ILogger<TCategoryName> logger,
        IObjectMapper objectMapper
    )
        : base(asyncQueryableExecuter, repository, logger, objectMapper) { }
}

public abstract class AsyncCrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
   : CrudAppServiceBase<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>,
    IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
       where TEntity : class, IEntity<TPrimaryKey>
       where TEntityDto : IEntityDto<TPrimaryKey>
       where TUpdateInput : IEntityDto<TPrimaryKey>
       where TGetInput : IEntityDto<TPrimaryKey>
       where TDeleteInput : IEntityDto<TPrimaryKey>
{

    protected readonly IAsyncQueryableExecuter AsyncQueryableExecuter;

    protected AsyncCrudAppService(
        IAsyncQueryableExecuter asyncQueryableExecuter,
        IRepository<TEntity, TPrimaryKey> repository,
        ILogger<TCategoryName> logger,
        IObjectMapper objectMapper
    )
        : base(repository, logger, objectMapper) => AsyncQueryableExecuter = asyncQueryableExecuter;

    public virtual async Task<TEntityDto> GetAsync(TGetInput input)
    {
        var entity = await GetEntityByIdAsync(input.Id);
        return MapToEntityDto(entity);
    }

    public virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
    {
        var query = CreateFilteredQuery(input);
        var totalCount = await AsyncQueryableExecuter.CountAsync(query);

        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);

        var entities = await AsyncQueryableExecuter.ToListAsync(query);

        return new PagedResultDto<TEntityDto>(
            totalCount,
            entities.Select(MapToEntityDto).ToList()
        );
    }

    public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
    {
        var entity = MapToEntity(input);

        await Repository.InsertAsync(entity);
        await Repository.SaveChangesAsync();

        return MapToEntityDto(entity);
    }

    public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
    {
        var entity = await GetEntityByIdAsync(input.Id);
        MapToEntity(input, entity);

        await Repository.UpdateAsync(entity);
        await Repository.SaveChangesAsync();

        return MapToEntityDto(entity);
    }

    public virtual async Task DeleteAsync(TDeleteInput input)
    {
        var entity = await GetEntityByIdAsync(input.Id);

        await Repository.DeleteAsync(entity);
        await Repository.SaveChangesAsync();
    }

    protected virtual Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
        => Repository.GetAsync(id);
}