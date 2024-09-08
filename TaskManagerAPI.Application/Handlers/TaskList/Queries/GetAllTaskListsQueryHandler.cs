using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers.Queries;

public class GetAllTaskListsQueryHandler : IRequestHandler<GetAllTaskListsQuery, IEnumerable<TaskList>?>
{
    private readonly ITaskListRepository _taskListRepository;
    public GetAllTaskListsQueryHandler(ITaskListRepository taskListRepository)
    {
        _taskListRepository = taskListRepository;
    }
    public async Task<IEnumerable<TaskList>?> Handle(GetAllTaskListsQuery query, CancellationToken cancellationToken)
        => await _taskListRepository
            .GetAllAsync();
}
