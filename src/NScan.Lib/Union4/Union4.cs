﻿using System;

namespace NScan.Lib.Union4
{
  public abstract class Union3<T1, T2, T3, T4>
  {
    private readonly object? _value = null;

    protected Union3(T1 o)
    {
      AssertNotNull(o!);
      _value = o;
    }
    protected Union3(T2 o)
    {
      AssertNotNull(o!);
      _value = o;
    }
    protected Union3(T3 o)
    {
      AssertNotNull(o!);
      _value = o;
    }

    protected Union3(T4 o)
    {
      AssertNotNull(o!);
      _value = o;
    }

    private static void AssertNotNull(object o)
    {
      if (o == null)
      {
        throw new ArgumentNullException(nameof(o));
      }
    }

    public void Accept(IUnion3Visitor<T1, T2, T3, T4> visitor)
    {
      switch (_value)
      {
        case T1 o:
          visitor.Visit(o);
          break;
        case T2 o:
          visitor.Visit(o);
          break;
        case T3 o:
          visitor.Visit(o);
          break;
        case T4 o:
          visitor.Visit(o);
          break;
        default:
          throw new InvalidOperationException($"Unknown rule name {_value}");
      }
    }

    public TReturn Accept<TReturn>(IUnion4TransformingVisitor<T1, T2, T3, T4, TReturn> transformingVisitor)
    {
      return _value switch
      {
        T1 o => transformingVisitor.Visit(o),
        T2 o => transformingVisitor.Visit(o),
        T3 o => transformingVisitor.Visit(o),
        T4 o => transformingVisitor.Visit(o),
        _ => throw new InvalidOperationException($"Unknown rule name {_value}")
      };
    }
  }
}