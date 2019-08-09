﻿using System;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.ProjectScopedRules
{
  public class HasTargetFrameworkRuleSpecification
  {

    [Fact]
    public void ShouldValidateProjectForTargetFrameworkWhenChecked()
    {
      //GIVEN
      var targetFramework = Any.String();
      var rule = new HasTargetFrameworkRule(targetFramework, Any.Instance<IProjectScopedRuleViolationFactory>(), Any.String());
      var project = Substitute.For<IProjectScopedRuleTarget>();
      var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();

      //WHEN
      rule.Check(project, analysisReportInProgress);

      //THEN
      project.Received(1).ValidateTargetFrameworkWith((ITargetFrameworkCheck)rule, analysisReportInProgress);
    }

    [Fact]
    public void ShouldGiveItsDescriptionWhenConvertedToString()
    {
        //GIVEN
        var ruleDescription = Any.String();
        var targetFramework = Any.String();
        var rule = new HasTargetFrameworkRule(targetFramework, 
          Any.Instance<IProjectScopedRuleViolationFactory>(), 
          ruleDescription);

        //WHEN
        var stringRepresentation = rule.ToString();

        //THEN
        stringRepresentation.Should().Be(ruleDescription + " " + targetFramework);
    }


        [Fact]
        public void ShouldReportNothingWhenProjectMatchesAssemblyNameAndTargetFramework()
        {
          //GIVEN
          var targetFramework = Any.String();
          var expectedTargetFramework = Any.String();
          var violationFactory = Substitute.For<IProjectScopedRuleViolationFactory>();
          var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();
          var ruleDescription = Any.String();
          var assemblyName = Any.String();
          var violation = Any.Instance<RuleViolation>();
          var rule = new HasTargetFrameworkRule(expectedTargetFramework, violationFactory, ruleDescription);

          violationFactory.ProjectScopedRuleViolation(
            ruleDescription + " " + expectedTargetFramework, $"Project {assemblyName} has target framework {targetFramework}").Returns(violation);

          //WHEN
          rule.ApplyTo(assemblyName, targetFramework, analysisReportInProgress);

          //THEN
          analysisReportInProgress.Received(1).Add(violation);
        }

    }

}