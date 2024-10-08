﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Contracts.HTTP;

public sealed record CreateUserResponse
(
    string Id,
    string NickName,
    string Role,
    DateTime CreatedAt,
    DateTime LastUpdatedAt
);
