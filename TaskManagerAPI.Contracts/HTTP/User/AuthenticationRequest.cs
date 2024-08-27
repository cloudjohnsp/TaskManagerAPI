using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Contracts.HTTP;

public sealed record AuthenticationRequest
(
    string NickName,
    string Password
);
