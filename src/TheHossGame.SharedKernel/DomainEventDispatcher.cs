// ---
// 🃏 The HossGame 🃏
// <copyright file="DomainEventDispatcher.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel;

using MediatR;
using TheHossGame.SharedKernel.Interfaces;

/// <inheritdoc/>
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEventDispatcher"/> class.
    /// </summary>
    /// <param name="mediator">The mediatR implementation.</param>
    public DomainEventDispatcher(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task DispatchAndClearEvents(IEnumerable<EntityBase> entitiesWithEvents)
    {
       foreach (var entity in entitiesWithEvents)
       {
          var events = entity.Events.ToArray();
          entity.ClearDomainEvents();
          await this.PublishEventsAsync(events).ConfigureAwait(false);
       }
    }

    private async Task PublishEventsAsync(IEnumerable<DomainEventBase> events)
    {
        foreach (var domainEvent in events)
        {
            await this.mediator.Publish(domainEvent).ConfigureAwait(false);
        }
    }
}
