﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Infrastructure.Auth;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
}
