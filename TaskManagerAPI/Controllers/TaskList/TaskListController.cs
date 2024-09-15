using MediatR;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Api.Controllers.Common;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using TaskManagerAPI.Api.Models;

namespace TaskManagerAPI.Api.Controllers;

[Authorize]
public class TaskListController : TaskManagerApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public TaskListController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a task list.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /create-tasklist
    ///     {
    ///       "name": "My Tasks",
    ///       "userId": "e6301c32-76fd-4cd4-a8df-1c5378fdf160"
    ///     }
    /// </remarks>
    /// <response code="201">If task list creation succeeds</response>
    /// <response code="404">If user with given id does not exists</response>
    [HttpPost]
    [Route("create-tasklist")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskListResponse>> CreateTaskList(CreateTaskListRequest request)
    {
        CreateTaskListCommand command = _mapper.Map<CreateTaskListCommand>(request);
        TaskList commandResult = await _mediator.Send(command);
        TaskListResponse response = _mapper.Map<TaskListResponse>(commandResult);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Returns a task list based on it's id.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">If a task list returns.</response>
    /// <response code="404">If task list not found.</response>
    [HttpGet]
    [Route("get-tasklist")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskListResponse?>> GetTaskList(string id)
    {
        TaskList? commandResult = await _mediator.Send(new GetTaskListQuery(id));
        TaskListResponse response = _mapper.Map<TaskListResponse>(commandResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    /// <summary>
    /// Returns all task lists.
    /// </summary>
    /// <response code="200">If all task lists returns.</response>
    [HttpGet]
    [Route("get-all-tasklists")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskListResponse>?>> GetAllTaskLists()
    {
        IEnumerable<TaskList>? commandResult = await _mediator.Send(new GetAllTaskListsQuery());
        IEnumerable<TaskListResponse> response = _mapper.Map<IEnumerable<TaskListResponse>>(commandResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    /// <summary>
    /// Updates a task list name.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /update-tasklist
    ///     {
    ///       "name": "My Tasks",
    ///       "userId": "e6301c32-76fd-4cd4-a8df-1c5378fdf160"
    ///     }
    /// </remarks>
    /// <response code="200">If task list update succeeds</response>
    /// <response code="404">If task list does not exists</response>
    [HttpPatch]
    [Route("update-tasklist")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskListResponse?>> UpdateTaskList(UpdateTaskListRequest request)
    {
        UpdateTaskListCommand command = _mapper.Map<UpdateTaskListCommand>(request);
        TaskList? commandResult = await _mediator.Send(command);
        TaskListResponse response = _mapper.Map<TaskListResponse>(commandResult!);

        return StatusCode(StatusCodes.Status200OK, response);
    }

    /// <summary>
    /// Deletes a task list based on it's id.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204">If deletion succeeds.</response>
    /// <response code="404">If task list not found.</response>
    [HttpDelete]
    [Route("delete-tasklist")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTaskList(string id)
    {
        await _mediator.Send(new DeleteTaskListCommand(id));
        return StatusCode(StatusCodes.Status204NoContent);
    }
}
