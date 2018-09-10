﻿using System;
using System.Collections.Generic;
using System.Linq;
using MyTool.App;
using static MyTool.SearchResult;

namespace MyTool
{
  public class IndependentOfProjectRule : IDependencyRule
  {
    private readonly string _dependingAssemblyName;
    private readonly string _dependencyAssemblyName;

    public IndependentOfProjectRule(string dependingAssemblyName, string dependencyAssemblyName)
    {
      _dependingAssemblyName = dependingAssemblyName;
      _dependencyAssemblyName = dependencyAssemblyName;
    }

    public void Check(IReadOnlyList<IReferencedProject> path, IAnalysisReportInProgress report)
    {
      var depending = Find(path, _dependingAssemblyName);
      var dependency = Find(path, _dependencyAssemblyName);

      if (dependency.Found && depending.Found && depending.IsBefore(dependency))
      {
        report.ViolationOf(
          DependencyDescriptions.IndependentOf(_dependingAssemblyName, _dependencyAssemblyName),  
          depending.SegmentEndingWith(dependency, path));
      }
      else
      {
        report.Ok(
          DependencyDescriptions.IndependentOf(_dependingAssemblyName, _dependencyAssemblyName));
      }
    }

    private SearchResult<IReferencedProject> Find(IReadOnlyList<IReferencedProject> path, string assemblyName)
    {
      if (path.Any(p => p.HasAssemblyName(assemblyName)))
      {
        return path.Select(ItemFound).First(p => p.Value.HasAssemblyName(assemblyName));
      }
      else
      {
        return ItemNotFound();
      }
    }
  }

  internal class SearchResult<T>
  {
    private readonly T _instance;
    private readonly int _index; 

    public SearchResult(T instance, int index)
    {
      _instance = instance;
      _index = index;
    }

    public bool Found  => _instance != null;
    public T Value => Found ? _instance : throw new NoValueException(typeof(T));

    public List<T> SegmentEndingWith(SearchResult<T> second, IEnumerable<T> path) => path.ToList().GetRange(_index, second._index - _index + 1);

    public bool IsBefore(SearchResult<T> dependency)
    {
      return _index < dependency._index;
    }
  }

  internal static class SearchResult
  {
    public static SearchResult<T> ItemFound<T>(T instance, int i)
    {
      return new SearchResult<T>(instance, i);
    }

    public static SearchResult<IReferencedProject> ItemNotFound()
    {
      return new SearchResult<IReferencedProject>(null, 0);
    }
  }

  public class NoValueException : Exception
  {
    public NoValueException(Type type) : base("No value of type " + type + " in search result")
    {
      
    }
  }
}