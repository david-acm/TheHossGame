// 🃏 The HossGame 🃏
// <copyright file="PlayerName.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using Ardalis.GuardClauses;
using TheHossGame.SharedKernel;

public class PlayerName : ValueObject
{
   public PlayerName(string name)
   {
      Guard.Against.NullOrEmpty(name);
      Guard.Against.InvalidLength(name, nameof(name), 1, 30);

      this.Name = name;
   }

   public string Name { get; }
}

public static class GuardExtensions
{
   public static void InvalidLength(this IGuardClause guardClause, string input, string parameterName, int minLength, int maxLength)
   {
      if (guardClause is null)
      {
         throw new ArgumentNullException(nameof(guardClause));
      }

      if (input?.Length > minLength && input?.Length <= maxLength)
      {
         return;
      }

      throw new ArgumentException($"{nameof(parameterName)} \"{input}\" Length should be longer than {minLength} and shorter than {maxLength}", parameterName);
   }
}