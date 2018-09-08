﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Domain;

namespace Stocqres.Infrastructure
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserAsync(Guid userId);
    }
}
