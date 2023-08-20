using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Authentication.BusinessOperation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models.Authentication;
using RestApi.Models.CreateUser;

namespace RestApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/authenticate")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AuthenticationController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("")]
        [ProducesResponseType(typeof(AuthenticationBusinessResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate(
            [Required] [FromBody] AuthenticationRequestModel requestModel
        )
        {
            AuthenticationBusinessRequest request = this._mapper.Map<AuthenticationBusinessRequest>(requestModel);
            AuthenticationBusinessResponse response = await this._mediator.Send(request);
            
            return base.Ok(response);
        }
    }
}
