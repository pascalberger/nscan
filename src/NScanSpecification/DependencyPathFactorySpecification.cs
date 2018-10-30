﻿using FluentAssertions;
using NSubstitute;
using TddXt.NScan.CompositionRoot;
using TddXt.XFluentAssert.Root;
using Xunit;

namespace TddXt.NScan.Specification
{
  public class DependencyPathFactorySpecification
  {
    [Fact]
    public void ShouldCreateDependencyPathInProgressHoldingThePassedDestination()
    {
      //GIVEN
      var factory = new DependencyPathFactory();
      var destination = Substitute.For<IFinalDependencyPathDestination>();

      //WHEN
      var dependencyPathInProgress = factory.NewDependencyPathFor(destination);

      //THEN
      dependencyPathInProgress.GetType().Should().Be<DependencyPathInProgress>();
      dependencyPathInProgress.Should().DependOn(destination);
    }
  }
}
