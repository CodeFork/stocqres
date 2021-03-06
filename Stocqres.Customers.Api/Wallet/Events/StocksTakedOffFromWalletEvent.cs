﻿using System;
using Stocqres.Core.Events;

namespace Stocqres.Customers.Api.Wallet.Events
{
    public class StocksTakedOffFromWalletEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }

        public StocksTakedOffFromWalletEvent(Guid aggregateId, Guid companyId, Guid orderId, int quantity)
        {
            AggregateId = aggregateId;
            CompanyId = companyId;
            OrderId = orderId;
            Quantity = quantity;
        }
    }
}
