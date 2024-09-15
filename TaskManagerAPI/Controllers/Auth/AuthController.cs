using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Api.Controllers.Common;
using TaskManagerAPI.Api.Models;
using TaskManagerAPI.Contracts;
using MediatR;
using MapsterMapper;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Contracts.HTTP;

namespace TaskManagerAPI.Api.Controllers;

public class AuthController : TaskManagerApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Signs In an user.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /login
    ///     {
    ///        "nickname": "john_doe",
    ///        "password": "MyPassword@1234"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns user information and the session JWT token</response>
    /// <response code="404">If user not found</response>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        LoginQuery query = new(request.NickName, request.Password);
        LoginResult? queryResult = await _mediator.Send(query);
        return StatusCode(StatusCodes.Status200OK, _mapper.Map<LoginResponse>(queryResult));
    }
}
