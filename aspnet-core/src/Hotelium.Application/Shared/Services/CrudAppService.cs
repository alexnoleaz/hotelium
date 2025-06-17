using Hotelium.Shared.Entities;
using Hotelium.Shared.Repositories;
using Hotelium.Shared.Services.Dto;

namespace Hotelium.Shared.Services;

public abstract class CrudAppService<TCategoryName, TEntity, TEntityDto>
    : CrudAppService<TCategoryName, TEntity, TEntityDto, int>
    where TEntity : class, IEntity<int>
    where TEntityDto : IEntityDto<int>
{
    protected CrudAppService(IRepository<TEntity, int> repository, ILogger<TCategoryName> logger, IObjectMapper objectMapper)
        : base(repository, logger, objectMapper) { }
}

public abstract class CrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey>
    : CrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
{
    protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository, ILogger<TCategoryName> logger, IObjectMapper objectMapper)
        : base(repository, logger, objectMapper) { }
}

public abstract class CrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput>
    : CrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
{
    protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository, ILogger<TCategoryName> logger, IObjectMapper objectMapper)
        : base(repository, logger, objectMapper) { }
}

public abstract class CrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
    : CrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
    where TCreateInput : IEntityDto<TPrimaryKey>
{
    protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository, ILogger<TCategoryName> logger, IObjectMapper objectMapper)
        : base(repository, logger, objectMapper) { }
}

public abstract class CrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    : CrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
    where TUpdateInput : IEntityDto<TPrimaryKey>
{
    protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository, ILogger<TCategoryName> logger, IObjectMapper objectMapper)
        : base(repository, logger, objectMapper) { }
}

public abstract class CrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
: CrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
    where TUpdateInput : IEntityDto<TPrimaryKey>
    where TGetInput : IEntityDto<TPrimaryKey>
{
    protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository, ILogger<TCategoryName> logger, IObjectMapper objectMapper)
        : base(repository, logger, objectMapper) { }
}

public abstract class CrudAppService<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
   : CrudAppServiceBase<TCategoryName, TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>,
    ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
       where TEntity : class, IEntity<TPrimaryKey>
       where TEntityDto : IEntityDto<TPrimaryKey>
       where TUpdateInput : IEntityDto<TPrimaryKey>
       where TGetInput : IEntityDto<TPrimaryKey>
       where TDeleteInput : IEntityDto<TPrimaryKey>
{
    protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository, ILogger<TCategoryName> logger, IObjectMapper objectMapper)
        : base(repository, logger, objectMapper) { }

    public virtual TEntityDto Get(TGetInput input)
    {
        var entity = GetEntityById(input.Id);
        return MapToEntityDto(entity);
    }

    public virtual PagedResultDto<TEntityDto> GetAll(TGetAllInput input)
    {
        var query = CreateFilteredQuery(input);
        var totalCount = query.Count();

        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);

        var entities = query.ToList();

        return new PagedResultDto<TEntityDto>(
            totalCount,
            entities.Select(MapToEntityDto).ToList()
        );
    }

    public virtual TEntityDto Create(TCreateInput input)
    {
        var entity = MapToEntity(input);

        Repository.Insert(entity);
        Repository.SaveChanges();

        return MapToEntityDto(entity);
    }

    public virtual TEntityDto Update(TUpdateInput input)
    {
        var entity = GetEntityById(input.Id);
        MapToEntity(input, entity);

        Repository.Update(entity);
        Repository.SaveChanges();

        return MapToEntityDto(entity);
    }

    public virtual void Delete(TDeleteInput input)
    {
        var entity = GetEntityById(input.Id);

        Repository.Delete(entity);
        Repository.SaveChanges();
    }

    protected virtual TEntity GetEntityById(TPrimaryKey id)
        => Repository.Get(id);
}