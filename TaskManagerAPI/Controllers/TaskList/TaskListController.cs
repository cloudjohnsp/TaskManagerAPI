using MediatR;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Api.Controllers.Common;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

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

    [HttpPost]
    [Route("create-tasklist")]
    public async Task<ActionResult<TaskListResponse>> CreateTaskList(CreateTaskListRequest request)
    { 
        CreateTaskListCommand command = _mapper.Map<CreateTaskListCommand>(request);
        TaskList commandResult = await _mediator.Send(command);
        TaskListResponse response = _mapper.Map<TaskListResponse>(commandResult);
        return StatusCode(StatusCodes.Status201Created, response);
    }
      
    [HttpGet]
    [Route("get-tasklist")]
    public async Task<ActionResult<TaskListResponse?>> GetTaskList(string id)
    {
        TaskList? commandResult = await _mediator.Send(new GetTaskListQuery(id));
        TaskListResponse response = _mapper.Map<TaskListResponse>(commandResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpGet]
    [Route("get-all-tasklists")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<TaskListResponse>?>> GetAllTaskLists()
    {
        IEnumerable<TaskList>? commandResult = await _mediator.Send(new GetAllTaskListsQuery());
        IEnumerable<TaskListResponse> response = _mapper.Map<IEnumerable<TaskListResponse>>(commandResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpPatch]
    [Route("update-tasklist")]
    public async Task<ActionResult<TaskListResponse?>> UpdateTaskList(UpdateTaskListRequest request)
    {
        UpdateTaskListCommand command = _mapper.Map<UpdateTaskListCommand>(request);
        TaskList? commandResult = await _mediator.Send(command);
        TaskListResponse response = _mapper.Map<TaskListResponse>(commandResult!);

        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpDelete]
    [Route("delete-tasklist")]
    public async Task<IActionResult> DeleteTaskList(string id)
    {
        await _mediator.Send(new DeleteTaskListCommand(id));
        return StatusCode(StatusCodes.Status204NoContent);
    }
}
