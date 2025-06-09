using AutoMapper;

using Hotelium.Shared.Dependency;

namespace Hotelium.Shared;

public class AutoMapperObjectMapper(IMapper mapper) : IObjectMapper, ISingletonDependency
{
    private readonly IMapper _mapper = mapper;

    public TDestination Map<TDestination>(object source)
        => _mapper.Map<TDestination>(source);

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        => _mapper.Map(source, destination);

    public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
        => _mapper.ProjectTo<TDestination>(source);
}