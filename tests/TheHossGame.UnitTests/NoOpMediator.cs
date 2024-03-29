﻿// 🃏 The HossGame 🃏
// <copyright file="NoOpMediator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests;

using MediatR;
using System.Runtime.CompilerServices;

public class NoOpMediator : IMediator
{
    public Task Publish(object notification, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
      where TNotification : INotification
    {
        return Task.CompletedTask;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<TResponse>(default!);
    }

    public Task<object?> Send(object request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<object?>(default);
    }

    public async IAsyncEnumerable<TResponse> CreateStream<TResponse>(
        IStreamRequest<TResponse> request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        yield break;
    }

    public async IAsyncEnumerable<object?> CreateStream(
        object request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        yield break;
    }
}
