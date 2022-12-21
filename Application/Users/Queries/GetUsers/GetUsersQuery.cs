﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggr.Application.Users.Queries.GetUsers
{
    public record class GetUsersQuery : IRequest<IEnumerable<UsersQueryDto>>;
}
