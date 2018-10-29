﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.NScan.App;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification
{
  public class DependencyPathInProgressSpecification
  {
    [Fact]
    public void ShouldAddFinalizedPathWithFinalAndClonedProjectsInOrder()
    {
      //GIVEN
      var destination = Substitute.For<IFinalDependencyPathDestination>();
      var initialProjects = Any.List<IReferencedProject>();
      var projectDependencyPathFactory = Substitute.For<IProjectDependencyPathFactory>();
      var newDependencyPath = Any.Instance<IProjectDependencyPath>();
      var additionalProject = Any.Instance<IReferencedProject>();
      var finalProject = Any.Instance<IReferencedProject>();

      var dependencyPathInProgress = new DependencyPathInProgress(destination, 
        projectDependencyPathFactory);

      projectDependencyPathFactory.Invoke(
          Concatenated(initialProjects, additionalProject, finalProject)).Returns(newDependencyPath);

      var clonedPath = dependencyPathInProgress.CloneWith(additionalProject);

      //WHEN
      clonedPath.FinalizeWith(finalProject);

      //THEN
      destination.Received(1).Add(newDependencyPath);
    }

    private static IReadOnlyList<IReferencedProject> Concatenated(
      List<IReferencedProject> alreadyAggregatedProjects, 
      params IReferencedProject[] additionalProjects)
    {
      return Arg<IReadOnlyList<IReferencedProject>>.That(
        path => path.Should().BeEquivalentTo(alreadyAggregatedProjects.Concat(additionalProjects).ToList()));
    }
  }
}