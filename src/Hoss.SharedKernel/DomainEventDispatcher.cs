// 🃏 The HossGame 🃏
// <copyright file="DomainEventDispatcher.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.SharedKernel;

#region

using Hoss.SharedKernel.Interfaces;
using MediatR;

#endregion

/// <inheritdoc />
public class DomainEventDispatcher<T> : IDomainEventDispatcher<T>
   where T : ValueObject
{
   private readonly IMediator mediator;

   /// <summary>
   ///    Creates an instance of the <see cref="DomainEventDispatcher{T}" />
   /// </summary>
   /// <param name="mediator"></param>
   public DomainEventDispatcher(IMediator mediator)
   {
      this.mediator = mediator;
   }

   #region IDomainEventDispatcher<T> Members

   /// <inheritdoc />
   public async Task DispatchAndClearEvents(IEnumerable<EntityBase<T>> entitiesWithEvents)
   {
      foreach (var entity in entitiesWithEvents)
      {
         var events = entity.Events.ToArray();
         entity.ClearDomainEvents();
         await this.PublishEventsAsync(events).ConfigureAwait(false);
      }
   }

   #endregion

   private async Task PublishEventsAsync(IEnumerable<DomainEventBase> events)
   {
      foreach (var domainEvent in events) await this.mediator.Publish(domainEvent).ConfigureAwait(false);
   }
}
