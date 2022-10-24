// 🃏 The HossGame 🃏
// <copyright file="GuardExtensions.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.SharedKernel;

#region

using System.Diagnostics.CodeAnalysis;

#endregion

/// <summary>
///    Custom common guard clauses.
/// </summary>
public static class GuardExtensions
{
   /// <summary>
   ///    Guards input from invalid length.
   /// </summary>
   /// <param name="input">The input to validate.</param>
   /// <param name="parameterName">The parameter name.</param>
   /// <param name="minLength">The minimum lenght of the input.</param>
   /// <param name="maxLength">The max lenght of th input.</param>
   [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Needed for extension method")]
   public static void InvalidLength(string input, string parameterName, int minLength, int maxLength)
   {
      if (input.Length > minLength && input.Length <= maxLength)
      {
         return;
      }

      throw new ArgumentException($"{nameof(parameterName)} \"{input}\" Length should be longer than {minLength} and shorter than {maxLength}", parameterName);
   }
}
