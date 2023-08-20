using AutoMapper;
using Domain.User.BusinessOperation.CreateUser;
using Domain.User.BusinessOperation.ForgotPassword;
using Domain.User.BusinessOperation.ResetPassword;
using Domain.User.BusinessOperation.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models.CreateUser;
using RestApi.Models.ResetPassword;
using RestApi.Models.UpdateUser;

namespace RestApi.Controllers
{
    
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UserController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }
       
        [HttpPost]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel requestModel, CancellationToken cancellationToken)
        {
            CreateUserBusinessRequest request = new (this.User);
            this._mapper.Map(requestModel, request);
            await this._mediator.Send(request, cancellationToken);
            return base.Ok(StatusCodes.Status201Created);
        }

        [HttpPatch]
        [Authorize]
        [Route("update/user")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBuyerLimit([FromBody] UpdateUserRequestModel requestModel,
            CancellationToken cancellationToken)
        {
            UpdateUserBusinessRequest request = new UpdateUserBusinessRequest(this.User);
                
            this._mapper.Map(requestModel, request);
            await this._mediator.Send(request, cancellationToken);
            return base.StatusCode(StatusCodes.Status201Created);
        }
        
        [HttpGet]
        [Authorize]
        [Route("resetPasswordToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetResetPasswordToken(CancellationToken cancellationToken)
        {
            GetResetPasswordTokenBusinessRequest request = new GetResetPasswordTokenBusinessRequest(this.User);
            string token = await this._mediator.Send(request, cancellationToken);
            return base.Ok(token);
        }
        
        [HttpPatch]
        [Authorize]
        [Route("resetPassword")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel requestModel,
            CancellationToken cancellationToken)
        {
            ResetPasswordBusinessRequest request = new ResetPasswordBusinessRequest(this.User);
                
            this._mapper.Map(requestModel, request);
            await this._mediator.Send(request, cancellationToken);
            return base.StatusCode(StatusCodes.Status201Created);
        }
    }
}
