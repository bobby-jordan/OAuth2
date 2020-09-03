using AutoMapper;
using OAuth2Example.BindModels;
using OAuth2Example.DAL.Models;

namespace OAuth2Example.MapperProfiles
{
    public class BindModelToModelProfile : Profile
    {
        public BindModelToModelProfile()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<UserBindModel, User>();
        }
    }
}
