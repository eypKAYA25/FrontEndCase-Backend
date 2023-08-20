using AutoMapper;
using Domain;
using Domain.Authentication.BusinessOperation;
using Domain.Company.BusinessOperation.CreateCompany;
using Domain.Company.BusinessOperation.UpdateCompanyById;
using Domain.User.BusinessOperation.CreateUser;
using Domain.User.BusinessOperation.ResetPassword;
using Domain.User.BusinessOperation.UpdateUser;
using RestApi.Models.Authentication;
using RestApi.Models.CreateCompany;
using RestApi.Models.CreateUser;
using RestApi.Models.ResetPassword;
using RestApi.Models.UpdateCompany;
using RestApi.Models.UpdateUser;

namespace RestApi;

public class MapperProfile : Profile
{
    public MapperProfile()
        : base("RestApiMappings")
    {
        base.CreateMap<CreateUserRequestModel, CreateUserBusinessRequest>()
            ; 
        
        base.CreateMap<AuthenticationRequestModel, AuthenticationBusinessRequest>()
            ;  
        
        base.CreateMap<UpdateCompanyByIdRequestModel, UpdateCompanyByIdBusinessRequest>()
            ; 
        
        base.CreateMap<UpdateUserRequestModel, UpdateUserBusinessRequest>()
            ; 
        
        base.CreateMap<ResetPasswordRequestModel, ResetPasswordBusinessRequest>()
            ; 
        
        base.CreateMap<CreateCompanyRequestModel, CreateCompanyBusinessRequest>()
            // .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.GetClaimValue(Strings.USERID)))

            ;
    }
}