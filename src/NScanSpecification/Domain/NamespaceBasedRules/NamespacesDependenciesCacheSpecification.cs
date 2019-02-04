﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.NamespaceBasedRules;
using Xunit;

namespace TddXt.NScan.Specification.Domain.NamespaceBasedRules
{
  public class NamespacesDependenciesCacheSpecification
  {
    [Fact]
    public void ShouldReturnNoCyclesWhenNoMappingsWereAdded()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();

      //WHEN
      var cycles = cache.RetrieveCycles();

      //THEN
      cycles.Should().BeEmpty();
    }

    [Fact]
    public void ShouldReturnNoCyclesWhenMappingsAddedDoNotMakeTheCycle()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = AnyRoot.Root.Any.String();
      var namespace2 = AnyRoot.Root.Any.String();
      var namespace3 = AnyRoot.Root.Any.String();

      cache.AddMapping(namespace1, namespace2);
      cache.AddMapping(namespace2, namespace3);

      //WHEN
      var cycles = cache.RetrieveCycles();

      //THEN
      cycles.Should().BeEmpty();
    }

    [Fact]
    public void ShouldBeAbleToRetrieveSingleDirectCycle()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = AnyRoot.Root.Any.String();
      var namespace2 = AnyRoot.Root.Any.String();

      cache.AddMapping(namespace1, namespace2);
      cache.AddMapping(namespace2, namespace1);

      //WHEN
      var cycles = cache.RetrieveCycles();

      //THEN
      cycles.Single().Should().BeEquivalentTo(new List<string> {namespace1, namespace2, namespace1});
    }

    [Fact]
    public void ShouldBeAbleToRetrieveSingleIndirectCycle()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = AnyRoot.Root.Any.String();
      var namespace2 = AnyRoot.Root.Any.String();
      var namespace3 = AnyRoot.Root.Any.String();

      cache.AddMapping(namespace1, namespace2);
      cache.AddMapping(namespace2, namespace3);
      cache.AddMapping(namespace3, namespace1);

      //WHEN
      var cycles = cache.RetrieveCycles();

      //THEN
      cycles.Single().Should().BeEquivalentTo(new List<string> {namespace1, namespace2, namespace3, namespace1});
    }

    [Fact]
    public void ShouldBeAbleToRetrieveMultipleIndirectCycle()
    {
      //GIVEN
      var cache = new NamespacesDependenciesCache();
      var namespace1 = AnyRoot.Root.Any.String();
      var namespace2 = AnyRoot.Root.Any.String();
      var namespace3 = AnyRoot.Root.Any.String();
      var namespace4 = AnyRoot.Root.Any.String();

      cache.AddMapping(namespace1, namespace2);
      cache.AddMapping(namespace2, namespace1);
      
      cache.AddMapping(namespace1, namespace3);
      cache.AddMapping(namespace3, namespace4);
      cache.AddMapping(namespace4, namespace1);

      //WHEN
      var cycles = cache.RetrieveCycles();

      //THEN
      cycles[0].Should().BeEquivalentTo(new List<string> {namespace1, namespace2, namespace1});
      cycles[1].Should().BeEquivalentTo(new List<string> {namespace1, namespace3, namespace4, namespace1});
      cycles.Should().HaveCount(2);
    }

  }
}