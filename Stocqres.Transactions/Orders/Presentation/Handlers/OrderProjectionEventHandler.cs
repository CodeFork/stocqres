﻿using System;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Infrastructure.Projections;
using Stocqres.Transactions.Orders.Domain.Enums;
using Stocqres.Transactions.Orders.Domain.Events;

namespace Stocqres.Transactions.Orders.Presentation.Handlers
{
    public class OrderProjectionEventHandler : 
        IEventHandler<BuyOrderCreatedEvent>,
        IEventHandler<SellOrderCreatedEvent>,
        IEventHandler<OrderCancelledEvent>,
        IEventHandler<OrderFinishedEvent>
    {
        private readonly IProjectionWriter _projectionWriter;

        public OrderProjectionEventHandler(IProjectionWriter projectionWriter)
        {
            _projectionWriter = projectionWriter;
        }

        public async Task HandleAsync(BuyOrderCreatedEvent @event)
        {
            await _projectionWriter.AddAsync(new OrderProjection(@event.AggregateId, @event.WalletId, @event.CompanyId, @event.Quantity,
                OrderState.Started));
        }

        public async Task HandleAsync(OrderCancelledEvent @event)
        {
            await _projectionWriter.UpdateAsync<OrderProjection>(@event.AggregateId,
                e => { e.State = OrderState.Cancelled; });
        }

        public async Task HandleAsync(OrderFinishedEvent @event)
        {
            await _projectionWriter.UpdateAsync<OrderProjection>(@event.AggregateId,
                e => { e.State = OrderState.Finished; });
        }

        public async Task HandleAsync(SellOrderCreatedEvent @event)
        {
            await _projectionWriter.AddAsync(new OrderProjection(@event.AggregateId, @event.WalletId, @event.CompanyId, @event.Quantity,
                OrderState.Started));
        }
    }
}
