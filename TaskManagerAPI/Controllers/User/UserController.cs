using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Api.Controllers.Common;
using TaskManagerAPI.Api.Models;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Contracts;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions.BaseExceptions;
using TaskManagerAPI.Infrastructure.Auth;

namespace TaskManagerAPI.Api.Controllers;

public class UserController : TaskManagerApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public UserController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates an user.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /create-user
    ///     {
    ///        "nickName": "john_doe",
    ///        "password": "Pass@1234",
    ///        "role": "Common"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">If user creation succeeds</response>
    /// <response code="404">If user not found</response>
    /// <response code="409">If user with provided nickname already exists</response>
    [HttpPost]
    [Route("create-user")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserRequest request)
    {
        CreateUserCommand command = new(request.NickName, request.Password, request.Role);
        User commandResult = await _mediator.Send(command);
        CreateUserResponse response = _mapper.Map<CreateUserResponse>(commandResult);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Returns an user based on it's id.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">If user returns</response>
    /// <response code="404">If user not found</response>
    [HttpGet]
    [Route("get-user")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse?>> GetUser(string id)
    {
        GetUserQuery query = new(id);
        User? queryResult = await _mediator.Send(query);
        UserResponse response = _mapper.Map<UserResponse>(queryResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    /// <summary>
    /// Updates an user nickname.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /update-nickname
    ///     {
    ///        "id": "e6301c32-76fd-4cd4-a8df-1c5378fdf160",
    ///        "nickname": "jane_doe"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">If nickname update is successful</response>
    /// <response code="404">If user not found</response>
    [HttpPatch]
    [Route("update-nickname")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse?>> UpdateNickName(UpdateNickNameRequest request)
    {
        UpdateUserNickNameCommand command = new(request.Id, request.NickName);
        User? commandResult = await _mediator.Send(command);
        UserResponse response = _mapper.Map<UserResponse>(commandResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    /// <summary>
    /// Updates an user password.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /update-password
    ///     {
    ///        "id": "e6301c32-76fd-4cd4-a8df-1c5378fdf160",
    ///        "password": "MyPassword@1234"
    ///     }
    ///
    /// </remarks>
    /// <response code="204">If password update is successful</response>
    /// <response code="404">If user not found</response>
    [HttpPatch]
    [Route("update-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest request)
    {
        await _mediator.Send(new UpdateUserPasswordCommand(request.Id, request.Password));
        return StatusCode(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// Deletes an user.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204">If user deletion succeed</response>
    /// <response code="404">If user not found</response>
    [HttpDelete]
    [Route("delete-user")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(string id)
    {
        DeleteUserCommand command = new(id);
        await _mediator.Send(command);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}
