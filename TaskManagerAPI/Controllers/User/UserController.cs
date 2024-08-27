using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Api.Controllers.Common;
using TaskManagerAPI.Api.Helpers;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Contracts;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Auth;

namespace TaskManagerAPI.Api.Controllers;

public class UserController : TaskManagerApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IJwtConfig _jwtConfig;

    public UserController(ISender mediator, IMapper mapper, IJwtConfig jwtConfig)
    {
        _mediator = mediator;
        _mapper = mapper;
        _jwtConfig = jwtConfig;
    }

    [HttpPost]
    [Route("create-user")]
    public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserRequest request)
    {
        CreateUserCommand command = new(request.NickName, request.Password);
        User commandResult = await _mediator.Send(command);
        CreateUserResponse response = _mapper.Map<CreateUserResponse>(commandResult);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpGet]
    [Route("get-user")]
    [Authorize]
    public async Task<ActionResult<UserResponse?>> GetUser(string id)
    {
        GetUserQuery query = new(id);
        User? queryResult = await _mediator.Send(query);
        UserResponse response = _mapper.Map<UserResponse>(queryResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpPatch]
    [Route("update-nickname")]
    [Authorize]
    public async Task<ActionResult<UserResponse?>> UpdateNickName(UpdateNickNameRequest request)
    {
        UpdateUserNickNameCommand command = new(request.Id, request.NickName);
        User? commandResult = await _mediator.Send(command);
        UserResponse response = _mapper.Map<UserResponse>(commandResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpPatch]
    [Route("update-password")]
    [Authorize]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest request)
    {
        await _mediator.Send(new UpdateUserPasswordCommand(request.Id, request.Password));
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpDelete]
    [Route("delete-user")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(string id)
    {
        DeleteUserCommand command = new(id);
        await _mediator.Send(command);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpPost]
    [Route("authenticate")]
    public async Task<ActionResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request)
    {
        AuthenticationCommand command = new(request.NickName, request.Password);
        User? commandResult = await _mediator.Send(command);
        string token = await _jwtConfig.GenerateJwtToken(commandResult!);
        AuthenticationResponse response = new(commandResult!.Id, commandResult.NickName, token);
        return StatusCode(StatusCodes.Status200OK, response);
    }
}
