﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Contracts;

public record LoginResult
(
    User User,
    string Token
);
