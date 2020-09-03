using AutoMapper;
using OAuth2Example.BindModels;
using OAuth2Example.DAL.Models;

namespace OAuth2Example.MapperProfiles
{
    public class ModelToBindModelProfile : Profile
    {
        public ModelToBindModelProfile()
        {
            SourceMemberNamingConvention = new PascalCaseNamingConvention();
            DestinationMemberNamingConvention = new LowerUnderscoreNamingConvention();

            CreateMap<User, UserBindModel>()
                .ForMember(u => u.password, opt => opt.Ignore())
                .ForMember(u => u.new_password, opt => opt.Ignore());
        }
    }
}
