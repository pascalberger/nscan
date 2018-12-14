﻿using TddXt.NScan.Specification.Component;
using Xunit;
using static System.Environment;

namespace TddXt.NScan.Specification.EndToEnd
{
  public class CorrectNamespacesRuleFeatureSpecification
  {
    //TODO happy path

    [Fact]
    public void ShouldNotReportErrorWhenMultipleFilesOfSingleProjectAreInCorrectNamespace()
    {
      //GIVEN
        using (var context = new NScanE2EDriver())
        {
            context.HasProject("MyProject")
                .WithRootNamespace("MyProject")
                .WithFile("lol1.cs", "MyProject")
                .WithFile("lol2.cs", "MyProject");
            context.Add(DependencyRuleBuilder.Rule().Project("*MyProject*").HasCorrectNamespaces());

            //WHEN
            context.PerformAnalysis();

            //THEN
            context.ReportShouldContainText($"*MyProject* hasCorrectNamespaces: [OK]");
        }
    }
    
    [Fact]
    public void ShouldReportErrorWhenMultipleFilesOfSingleProjectAreInWrongNamespaceEvenThoughSomeAreInTheRightOne() //TODO split into two tests
    {
      //GIVEN
        using (var context = new NScanE2EDriver())
        {
            context.HasProject("MyProject")
                .WithRootNamespace("MyProject")
                .WithFile("lol1.cs", "WrongNamespace")
                .WithFile("lol2.cs", "WrongNamespace")
                .WithFile("lol3.cs", "MyProject")
                .WithFile("Domain\\lol4.cs", "MyProject.Domain")
                .WithFile("Domain\\lol5.cs", "MyProject.Domain");
            context.Add(DependencyRuleBuilder.Rule().Project("*MyProject*").HasCorrectNamespaces());

            //WHEN
            context.PerformAnalysis();

            //THEN
            context.ReportShouldContainText($"*MyProject* hasCorrectNamespaces: [ERROR]{NewLine}" +
                                            $"MyProject has root namespace MyProject but the file lol1.cs located in project root folder has incorrect namespace WrongNamespace{NewLine}" +
                                            $"MyProject has root namespace MyProject but the file lol2.cs located in project root folder has incorrect namespace WrongNamespace{NewLine}" +
                                            $"MyProject has root namespace MyProject but the file lol5.cs located in project root folder has incorrect namespace WrongNamespace");
            context.ReportShouldNotContainText("lol3");
            context.ReportShouldNotContainText("lol4");
        }
    }

  }
}
