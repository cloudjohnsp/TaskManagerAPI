using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Contracts;

public record UpdateNickNameRequest
(
    string Id,
    string NickName
);
