using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Api.Controllers.Common;
using TaskManagerAPI.Api.Models;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Api.Controllers;

[Authorize]
public sealed class TaskItemController : TaskManagerApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public TaskItemController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a task item.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /create-taskitem
    ///     {
    ///       "description": "My Item",
    ///       "userId": "e6301c32-76fd-4cd4-a8df-1c5378fdf160"
    ///     }
    /// </remarks>
    /// <response code="201">If task item creation succeeds</response>
    /// <response code="404">If a list with given id does not exists</response>
    [HttpPost]
    [Route("create-taskitem")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItemResponse>> CreateTaskItem(CreateTaskItemRequest request)
    {
        CreateTaskItemCommand command = _mapper.Map<CreateTaskItemCommand>(request);
        TaskItem commandResult = await _mediator.Send(command);
        TaskItemResponse response = _mapper.Map<TaskItemResponse>(commandResult!);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Returns a task item by given id.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="201">If a task item returns</response>
    /// <response code="404">If a list with given id does not exists</response>
    [HttpGet]
    [Route("get-taskitem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItemResponse>> GetTaskItem(string id)
    {
        GetTaskItemQuery query = new(id);
        TaskItem? commandResult = await _mediator.Send(query);
        TaskItemResponse response = _mapper.Map<TaskItemResponse>(commandResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    /// <summary>
    /// Returns all task items by given tasklist id.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="201">If any task item returns</response>
    /// <response code="404">If a list with given id does not exists</response>
    [HttpGet]
    [Route("get-all-taskitems-by-tasklist-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TaskItemResponse>?>> GetAllByTaskListId(string id)
    {
        GetAllTaskItemsByTaskListIdQuery query = new(id);
        IEnumerable<TaskItem>? queryResult = await _mediator.Send(query);
        IEnumerable<TaskItemResponse> response = _mapper.Map<IEnumerable<TaskItemResponse>>(queryResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    /// <summary>
    /// Updates a task item description.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /update-taskitem
    ///     {
    ///       "id": "e6301c32-76fd-4cd4-a8df-1c5378fdf160",
    ///       "description": "My Item"
    ///     }
    /// </remarks>
    /// <response code="200">If task item update succeeds</response>
    /// <response code="404">If an item with given id does not exists</response>
    [HttpPatch]
    [Route("update-taskitem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItemResponse>> UpdateTaskItem(UpdateTaskItemRequest request)
    {
        UpdateTaskItemCommand command = new(request.Id, request.Description);
        TaskItem? commandResult = await _mediator.Send(command);
        TaskItemResponse response = _mapper.Map<TaskItemResponse>(commandResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    /// <summary>
    /// Updates a task item status.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /update-taskitem-status
    ///     {
    ///       "id": "e6301c32-76fd-4cd4-a8df-1c5378fdf160",
    ///       "description": "My Item"
    ///     }
    /// </remarks>
    /// <response code="200">If task update succeeds</response>
    /// <response code="404">If a list with given id does not exists</response>
    [HttpPatch]
    [Route("update-taskitem-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItemResponse>> UpdateTaskItemStatus(UpdateTaskItemStatusRequest request)
    {
        UpdateTaskItemStatusCommand command = new(request.Id, request.IsDone);
        TaskItem? commandResult = await _mediator.Send(command);
        TaskItemResponse response = _mapper.Map<TaskItemResponse>(commandResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    /// <summary>
    /// Deletes a task item by given id.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204">If deletion succeeds</response>
    /// <response code="404">If an item with given id does not exists</response>
    [HttpDelete]
    [Route("delete-taskitem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTaskItem(string id)
    {
        await _mediator.Send(new DeleteTaskItemCommand(id));
        return StatusCode(StatusCodes.Status204NoContent);
    }
}
