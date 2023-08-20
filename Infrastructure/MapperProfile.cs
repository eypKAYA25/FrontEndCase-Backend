using AutoMapper;
using Domain.Company.BusinessOperation.CreateCompany;
using Domain.Entities;
using Domain.Queries;

namespace Infrastructure;

public class MapperProfile : Profile
{
    public MapperProfile()
        : base("InfrastructureMappings")
    {
        base.CreateMap<AspUser, Infrastructure.Entities.Identity.AspUser>().ReverseMap()
            ;
        
        base.CreateMap<CreateUserQuery, Domain.Entities.User>()
            ; 
        
        base.CreateMap<CreateCompanyBusinessRequest, CreateCompanyQuery>()
            ; 
        
        base.CreateMap<Infrastructure.Entities.User, Domain.Entities.User>().ReverseMap()
            ; 
        
        base.CreateMap<Infrastructure.Entities.Company, Domain.Entities.Company>().ReverseMap()
            ;
    }
}