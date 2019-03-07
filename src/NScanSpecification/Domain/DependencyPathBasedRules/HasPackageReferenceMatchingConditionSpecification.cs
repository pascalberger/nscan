﻿using FluentAssertions;
using GlobExpressions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.DependencyPathBasedRules
{
  public class HasPackageReferenceMatchingConditionSpecification
  {
    [Fact]
    public void ShouldMatchDependingOnWhetherTheDependencyHasMatchingPackageReference()
    {
      //GIVEN
      var packagePattern = Any.Instance<Glob>();
      var condition = new HasPackageReferenceMatchingCondition(packagePattern);
      var depending = Any.Instance<IProjectSearchResult>();
      var dependency = Substitute.For<IReferencedProject>();
      var dependencyAnswer = Any.Boolean();

      dependency.HasPackageReferenceMatching(packagePattern).Returns(dependencyAnswer);

      //WHEN
      var matches = condition.Matches(depending, dependency);

      //THEN
      matches.Should().Be(dependencyAnswer);
    }
  }
}