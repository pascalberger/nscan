﻿using System;
using System.Collections.Generic;
using MyTool;
using MyTool.App;
using MyTool.CompositionRoot;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace MyToolSpecification
{
  public class PathCacheSpecification
  {
    [Fact]
    public void ShouldPassNewStartingPathToEachProjectToGatherDependencies()
    {
      //GIVEN
      var dependencyPathFactory = Substitute.For<IDependencyPathFactory>();
      var pathCache = new PathCache(dependencyPathFactory);
      var project1 = Substitute.For<IDotNetProject>();
      var project2 = Substitute.For<IDotNetProject>();
      var project3 = Substitute.For<IDotNetProject>();
      var dependencyStartingPath1 = Any.Instance<IDependencyPathInProgress>();
      var dependencyStartingPath2 = Any.Instance<IDependencyPathInProgress>();
      var dependencyStartingPath3 = Any.Instance<IDependencyPathInProgress>();

      dependencyPathFactory.NewDependencyPathFor((IFinalDependencyPathDestination)pathCache).Returns(
        dependencyStartingPath1,
        dependencyStartingPath2,
        dependencyStartingPath3);

      //WHEN
      pathCache.BuildStartingFrom(project1, project2, project3);

      //THEN
      project1.Received(1).FillAllBranchesOf(dependencyStartingPath1);
      project2.Received(1).FillAllBranchesOf(dependencyStartingPath2);
      project3.Received(1).FillAllBranchesOf(dependencyStartingPath3);
    }

    
  }
}