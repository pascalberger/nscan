﻿using System;
using System.Collections.Generic;
using System.Linq;
using static TddXt.NScan.Lib.Maybe;

namespace TddXt.NScan.Lib
{
  public struct Maybe
  {
    public static Maybe<T> Just<T>(T instance)
    {
      if (instance == null)
      {
        throw new ArgumentNullException(nameof(instance));
      }
      return new Maybe<T>(instance);
    }

    public static Maybe<T> Nothing<T>()
    {
      return new Maybe<T>();
    }
  }

  public struct Maybe<T>
  {
    private readonly T _instance;

    public Maybe(T instance)
    {
      HasValue = true;
      _instance = instance;
    }

    public T Value()
    {
      if (!HasValue)
      {
        throw new NoValueException<T>();
      }

      return _instance;
    }

    public bool HasValue { get; }

    public Maybe<TU> Select<TU>(Func<T, TU> func)
    {
      if (HasValue)
      {
        return Just(func(_instance));
      }
      else
      {
        return Nothing<TU>();
      }
    }

    public T Otherwise(Func<T> fallbackFunc)
    {
      if (HasValue)
      {
        return Value();
      }
      else
      {
        return fallbackFunc();
      }
    }

    public T ValueOrDefault()
    {
      return Otherwise(() => default);
    }

    public T ValueOr(T fallback)
    {
      return Otherwise(() => fallback);
    }
  }

  public static class MaybeEnumerableExtensions
  {
    public static Maybe<T> FirstMaybe<T>(this IEnumerable<T> enumerable)
    {
      if (enumerable.Any())
      {
        return Just(enumerable.First());
      }
      else
      {
        return Nothing<T>();
      }
    }
  }
}