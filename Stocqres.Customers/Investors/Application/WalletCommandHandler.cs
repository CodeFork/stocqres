﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Domain.Commands.Wallet;

namespace Stocqres.Customers.Investors.Application
{
    public class WalletCommandHandler : ICommandHandler<CreateWalletCommand>
    {
        public Task HandleAsync(CreateWalletCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
