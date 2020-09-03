using AutoMapper;
using OAuth2Example.MapperProfiles;

namespace OAuth2Example.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ModelToBindModelProfile>();
                cfg.AddProfile<BindModelToModelProfile>();
            });
        }
    }
}