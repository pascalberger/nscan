﻿using FluentAssertions;
using TddXt.XFluentAssert.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace MyTool.CompositionRoot
{
  public class RuleFactorySpecification
  {
    [Fact]
    public void ShouldCreateIndependentOfProjectRuleWithPassedIds()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      
      //WHEN
      var dependingId = Any.ProjectId();
      var dependencyId = Any.ProjectId();
      var rule = ruleFactory.CreateIndependentOfProjectRule(dependingId, dependencyId);

      //THEN
      rule.GetType().Should().Be<IndependentOfProjectRule>();
      rule.Should().DependOn(dependingId);
      rule.Should().DependOn(dependencyId);
    }
  }
}