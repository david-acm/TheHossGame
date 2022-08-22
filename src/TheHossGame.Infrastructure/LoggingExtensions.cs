// 🃏 The HossGame 🃏
// <copyright file="LoggingExtensions.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Infrastructure.Logging;

using Microsoft.Extensions.Logging;

public static class LoggingExtensions
{
    private static readonly Action<ILogger, string, Exception> Warning = DefineWarningAction();
    private static readonly Action<ILogger, string, Exception> Error = DefineErrorAction();
    private static readonly Action<ILogger, string, Exception> Information = DefineInformationAction();

    public static void LogWarning(this ILogger logger, string message)
    {
        Warning(logger, message, null!);
    }

    public static void LogError(this ILogger logger, string message)
    {
        Error(logger, message, null!);
    }

    public static void LogInformation(this ILogger logger, string message)
    {
        Information(logger, message, null!);
    }

    private static Action<ILogger, string, Exception> DefineWarningAction()
    {
        return LoggerMessage.Define<string>(
                    LogLevel.Warning,
                    new EventId(2, nameof(Warning)),
                    "⚠ Warning: '{Message}'");
    }

    private static Action<ILogger, string, Exception> DefineErrorAction()
    {
        return LoggerMessage.Define<string>(
                    LogLevel.Error,
                    new EventId(1, nameof(Warning)),
                    "⛔ Error: '{Message}'");
    }

    private static Action<ILogger, string, Exception> DefineInformationAction()
    {
        return LoggerMessage.Define<string>(
                    LogLevel.Information,
                    new EventId(1, nameof(Warning)),
                    "ℹ Information: '{Message}'");
    }
}
