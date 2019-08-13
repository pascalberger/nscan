﻿using System.Diagnostics.CodeAnalysis;
using NScanSpecification.Component.AutomationLayer;
using NScanSpecification.Lib.AutomationLayer;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;
using static NScanSpecification.Lib.AutomationLayer.DependencyRuleBuilder;

namespace NScanSpecification.Component
{
  [SuppressMessage("ReSharper", "TestFileNameWarning")]
  public class IndependentOfPackageRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenNoProjectHasSpecifiedPackageReference()
    {
      //GIVEN
      var projectName = Any.String(); 
      var packageName = Any.String();
      var context = new NScanDriver();
      context.HasProject(projectName);

      context.Add(RuleDemandingThat().Project(projectName).IndependentOfPackage(packageName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        ProjectIndependentOfMessage.ProjectIndependentOfPackage(projectName, packageName).Ok());
      context.ShouldIndicateSuccess();
    }

    [Fact]
    public void ShouldReportFailureWhenProjectsHasSpecifiedPackageReferenceDirectly()
    {
      //GIVEN
      var projectName = Any.String();
      var packageName = Any.String();
      var context = new NScanDriver();
      context.HasProject(projectName).WithPackages(packageName);

      context.Add(RuleDemandingThat().Project(projectName).IndependentOfPackage(packageName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        ProjectIndependentOfMessage.ProjectIndependentOfPackage(projectName, packageName).Error()
        .ViolationPath(projectName));
      context.ShouldIndicateFailure();
    }

    [Fact]
    public void ShouldReportIndirectRuleBreak()
    {
      //GIVEN
      var projectName = Any.String();
      var projectName2 = Any.String();
      var packageName = Any.String();
      var context = new NScanDriver();
      context.HasProject(projectName).WithReferences(projectName2);
      context.HasProject(projectName2).WithPackages(packageName);

      context.Add(RuleDemandingThat().Project(projectName).IndependentOfPackage(packageName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        ProjectIndependentOfMessage.ProjectIndependentOfPackage(projectName, packageName).Error()
          .ViolationPath(projectName, projectName2));
      context.ShouldIndicateFailure();
    }
  }
}