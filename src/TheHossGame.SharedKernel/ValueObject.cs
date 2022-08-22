// ---
// 🃏 The HossGame 🃏
// <copyright file="ValueObject.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel;

using System.Reflection;

// source: https://github.com/jhewlett/ValueObject

/// <summary>
/// A base class to implement member wise equality in value objects.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed", Justification = "This is the value base class. It is expected tha the equality comparison would be the same for all implementing classes.")]
public abstract class ValueObject : IEquatable<ValueObject>
{
    private List<PropertyInfo>? properties;
    private List<FieldInfo>? fields;

    /// <summary>
    /// Value and member wise equality comparison.
    /// </summary>
    /// <param name="obj1">The first object to compare.</param>
    /// <param name="obj2">The second object to compare.</param>
    /// <returns>Whether the first and second object are equal.</returns>
    public static bool operator ==(ValueObject? obj1, ValueObject? obj2)
    {
        if (object.Equals(obj1, null))
        {
            if (object.Equals(obj2, null))
            {
                return true;
            }

            return false;
        }

        return obj1.Equals(obj2);
    }

    /// <summary>
    /// Value and member wise inequality comparison.
    /// </summary>
    /// <param name="obj1">The first object to compare.</param>
    /// <param name="obj2">The second object to compare.</param>
    /// <returns>Whether the first and second object are not equal.</returns>
    public static bool operator !=(ValueObject? obj1, ValueObject? obj2)
    {
        return !(obj1 == obj2);
    }

    /// <summary>
    /// Wether other object is equal to this.
    /// </summary>
    /// <param name="other">The other object.</param>
    /// <returns>Wether the other object is equal to this.</returns>
    public bool Equals(ValueObject? other)
    {
        return this.Equals(other as object);
    }

    /// <summary>
    /// Member wise equality check.
    /// </summary>
    /// <param name="obj">The object to compare to.</param>
    /// <returns>Whether the other object is equal to this.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || this.GetType() != obj.GetType())
        {
            return false;
        }

        return this.GetProperties().All(p => this.PropertiesAreEqual(obj, p))
            && this.GetFields().All(f => this.FieldsAreEqual(obj, f));
    }

    /// <summary>
    /// Memberwise hash calculation.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            foreach (var prop in this.GetProperties())
            {
                var value = prop.GetValue(this, null);
                hash = HashValue(hash, value);
            }

            foreach (var field in this.GetFields())
            {
                var value = field.GetValue(this);
                hash = HashValue(hash, value);
            }

            return hash;
        }
    }

    private static int HashValue(int seed, object? value)
    {
        var currentHash = value != null
            ? value.GetHashCode()
            : 0;

        return (seed * 23) + currentHash;
    }

    private bool PropertiesAreEqual(object obj, PropertyInfo p)
    {
        return object.Equals(p.GetValue(this, null), p.GetValue(obj, null));
    }

    private bool FieldsAreEqual(object obj, FieldInfo f)
    {
        return object.Equals(f.GetValue(this), f.GetValue(obj));
    }

    private IEnumerable<PropertyInfo> GetProperties()
    {
        if (this.properties == null)
        {
            this.properties = this.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                .ToList();

            // Not available in Core
        }

        return this.properties;
    }

    private IEnumerable<FieldInfo> GetFields()
    {
        if (this.fields == null)
        {
            this.fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                .ToList();
        }

        return this.fields;
    }
}
