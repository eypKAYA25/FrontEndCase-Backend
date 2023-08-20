using AutoMapper;
using Domain.Authentication.BusinessOperation;
using Domain.Company.BusinessOperation.GetAllCompaniesById;
using Domain.Company.BusinessOperation.UpdateCompanyById;
using Domain.Queries;
using Domain.User.BusinessOperation.CreateUser;
using Domain.User.BusinessOperation.UpdateUser;

namespace Domain;

public class MapperProfile : Profile
{
    public MapperProfile()
        : base("RestApiMappings")
    {
        base.CreateMap<CreateUserBusinessRequest, CreateUserQuery>()
            ;
        
        base.CreateMap<CreateUserQuery, CreateUserBusinessResponse>()
            ; 
        
        base.CreateMap<AuthenticationBusinessRequest, AuthenticationQuery>()
            ; 
        base.CreateMap<List<Domain.Entities.Company>, GetAllCompaniesByIdBusinessResponse>()
            .ForMember(dest => dest.Companies, opt => opt.MapFrom(src => src))
            ; 
        
        base.CreateMap<GetAllCompaniesByIdBusinessRequest, GetAllCompaniesByIdQuery>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.GetClaimValue(Strings.USERID)))
            ;
        
        base.CreateMap<UpdateCompanyByIdBusinessRequest, GetCompanyByIdQuery>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CompanyId))
            ; 
        
        base.CreateMap<UpdateUserBusinessRequest, GetUserByIdQuery>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GetClaimValue(Strings.USERID)))
            ;
    }
}