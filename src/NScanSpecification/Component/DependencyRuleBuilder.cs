﻿using System.Threading.Tasks;
using GlobExpressions;
using TddXt.NScan.CompositionRoot;
using static TddXt.NScan.ForFun.Maybe;

namespace TddXt.NScan.Specification.Component
{
  public interface IFullDependingPartStated
  {
    IFullRuleConstructed IndependentOfProject(string dependentAssemblyName);
    IFullRuleConstructed IndependentOfPackage(string packageName);
    IFullRuleConstructed IndependentOfAssembly(string assemblyName);
  }

  public interface IProjectNameStated : IFullDependingPartStated
  {
    IFullDependingPartStated Except(string exclusionPattern);
  }

  public interface IRuleDefinitionStart
  {
    IProjectNameStated Project(string dependingAssemblyName);
  }

  public interface IFullRuleConstructed
  {
    RuleDto Build();
  }

  public class DependencyRuleBuilder : IRuleDefinitionStart, IFullRuleConstructed, IProjectNameStated
  {
    private string _dependingPattern;
    private Glob _dependencyPattern;
    private string _dependencyType;
    private string _ruleName;
    private ForFun.Maybe<string> _exclusionPattern = Nothing<string>();

    public IProjectNameStated Project(string dependingAssemblyName)
    {

      _dependingPattern = dependingAssemblyName;
      return this;
    }

    public IFullRuleConstructed IndependentOfProject(string dependentAssemblyName)
    {
      _dependencyPattern = new Glob(dependentAssemblyName);
      _dependencyType = "project";
      _ruleName = "independentOf";
      return this;
    }

    public IFullRuleConstructed IndependentOfPackage(string packageName)
    {
      _dependencyPattern = new Glob(packageName);
      _dependencyType = "package";
      _ruleName = "independentOf";
      return this;

    }

    public IFullRuleConstructed IndependentOfAssembly(string assemblyName)
    {
      _dependencyPattern = new Glob(assemblyName);
      _dependencyType = "assembly";
      _ruleName = "independentOf";
      return this;
    }

    public IFullDependingPartStated Except(string exclusionPattern)
    {
      _exclusionPattern = Just(exclusionPattern);
      return this;
    }

    public RuleDto Build()
    {
      return new RuleDto
      {
        DependingPattern = _exclusionPattern
          .Select(p => Pattern.WithExclusion(_dependingPattern, p))
          .Otherwise(() => Pattern.WithoutExclusion(_dependingPattern)),
        DependencyPattern = _dependencyPattern,
        DependencyType = _dependencyType,
        RuleName = _ruleName
      };
    }

    public static IRuleDefinitionStart Rule()
    {
      return new DependencyRuleBuilder();
    }
  }
}