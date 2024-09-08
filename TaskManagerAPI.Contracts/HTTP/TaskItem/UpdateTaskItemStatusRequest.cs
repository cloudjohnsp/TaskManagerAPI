﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Contracts.HTTP;

public record UpdateTaskItemStatusRequest 
(
    string Id,
    bool IsDone
);
