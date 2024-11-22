using AutoMapper;

namespace AuthServer.Service.GeneralMapping;

public static class ObjectMapper
{
    private static readonly Lazy<IMapper> _lazy = new Lazy<IMapper>(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DtoMapper>();
        });
        return config.CreateMapper();
    });

    public static IMapper Mapper => _lazy.Value;
}