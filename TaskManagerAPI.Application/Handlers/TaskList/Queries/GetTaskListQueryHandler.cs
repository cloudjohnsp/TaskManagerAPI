﻿using MediatR;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers.Queries;

public class GetTaskListQueryHandler : IRequestHandler<GetTaskListQuery, TaskList?>
{
    private readonly ITaskListRepository _taskListRepository;
    public GetTaskListQueryHandler(ITaskListRepository taskListRepository)
    {
        _taskListRepository = taskListRepository;
    }
    public async Task<TaskList?> Handle(GetTaskListQuery query, CancellationToken cancellationToken)
        => await _taskListRepository
            .GetAsync(query.Id) ?? throw new TaskListNotFoundException(query.Id);

}
