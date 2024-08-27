using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Api.Controllers.Common;
using TaskManagerAPI.Api.Helpers;
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

    [HttpPost]
    [Route("create-taskitem")]
    public async Task<ActionResult<TaskItemResponse>> CreateTaskItem(TaskItemRequest request)
    {
        CreateTaskItemCommand command = _mapper.Map<CreateTaskItemCommand>(request);
        TaskItem commandResult = await _mediator.Send(command);
        TaskItemResponse response = _mapper.Map<TaskItemResponse>(commandResult!);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpGet]
    [Route("get-taskitem")]
    public async Task<ActionResult<TaskItemResponse>> GetTaskItem(string id)
    {
        GetTaskItemQuery query = new(id);
        TaskItem? commandResult = await _mediator.Send(query);
        TaskItemResponse response = _mapper.Map<TaskItemResponse>(commandResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpPut]
    [Route("update-taskitem")]
    public async Task<ActionResult<TaskItemResponse>> UpdateTaskItem(UpdateTaskItemRequest request)
    {
        UpdateTaskItemCommand command = new(request.Id, request.Description, request.IsDone);
        TaskItem? commandResult = await _mediator.Send(command);
        TaskItemResponse response = _mapper.Map<TaskItemResponse>(commandResult!);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpDelete]
    [Route("delete-taskitem")]
    public async Task<IActionResult> DeleteTaskItem(string id)
    {
        await _mediator.Send(new DeleteTaskItemCommand(id));
        return StatusCode(StatusCodes.Status204NoContent);
    }
}
