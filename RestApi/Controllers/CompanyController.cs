using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Company.BusinessOperation.CreateCompany;
using Domain.Company.BusinessOperation.GetAllCompaniesById;
using Domain.Company.BusinessOperation.UpdateCompanyById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models.CreateCompany;
using RestApi.Models.CreateUser;
using RestApi.Models.UpdateCompany;

namespace RestApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/company")]
    public class CompanyController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CompanyController(IMapper mapper, IMediator mediator)
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
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequestModel requestModel, CancellationToken cancellationToken)
        {
            CreateCompanyBusinessRequest request = new (this.User);
            this._mapper.Map(requestModel, request);
            await this._mediator.Send(request, cancellationToken);
            return base.Ok(StatusCodes.Status201Created);
        }
        
        [HttpGet]
        [Route("companies")]
        [Authorize]
        [ProducesResponseType(typeof(GetAllCompaniesByIdBusinessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCompany(CancellationToken cancellationToken)
        {
            GetAllCompaniesByIdBusinessRequest request = new(base.User);
            GetAllCompaniesByIdBusinessResponse response = await this._mediator.Send(request, cancellationToken);
            return base.Ok(response);
        }
        [HttpPatch]
        [Authorize]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCompanyById([FromRoute(Name = "id")] Guid id,[FromBody] UpdateCompanyByIdRequestModel requestModel,
            CancellationToken cancellationToken)
        {
            UpdateCompanyByIdBusinessRequest request =  new UpdateCompanyByIdBusinessRequest(this.User)
            {
                CompanyId = id,
            };

            this._mapper.Map(requestModel, request);
            await this._mediator.Send(request, cancellationToken);
            return base.StatusCode(StatusCodes.Status201Created);
        }
    }
}
