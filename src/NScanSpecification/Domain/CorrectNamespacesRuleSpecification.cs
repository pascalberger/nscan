﻿using System.Collections.Generic;
using NSubstitute;
using TddXt.NScan.Domain;
using TddXt.NScan.ReadingRules.Ports;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class CorrectNamespacesRuleSpecification
  {
    [Fact]
    public void ShouldMakeProjectAnalyzeFilesWithItselfWhenProjectMatchesAPattern()
    {
      //GIVEN
      var dto = Any.Instance<CorrectNamespacesRuleComplementDto>();
      var rule = new CorrectNamespacesRule(dto);
      var report = Any.Instance<IAnalysisReportInProgress>();
      var project = Substitute.For<IProjectScopedRuleTarget>();

      project.HasProjectAssemblyNameMatching(dto.ProjectAssemblyNamePattern).Returns(true);

      //WHEN
      rule.Check(project, report);

      //THEN
      project.Received(1).AnalyzeFiles(rule, report);
    }

    [Fact]
    public void ShouldNotMakeAnalyzeFilesWithItselfWhenProjectDoesNotMatchAPattern()
    {
      //GIVEN
      var dto = Any.Instance<CorrectNamespacesRuleComplementDto>();
      var rule = new CorrectNamespacesRule(dto);
      var report = Any.Instance<IAnalysisReportInProgress>();
      var project = Substitute.For<IProjectScopedRuleTarget>();

      project.HasProjectAssemblyNameMatching(dto.ProjectAssemblyNamePattern).Returns(false);

      //WHEN
      rule.Check(project, report);

      //THEN
      project.DidNotReceive().AnalyzeFiles(Arg.Any<IProjectScopedRule>(), Arg.Any<IAnalysisReportInProgress>());
    }

    [Fact]
    public void ShouldAnalyzeNamespaceCorrectnessOnEachAnalyzedFiles()
    {
      //GIVEN
      var correctNamespacesRuleComplementDto = Any.Instance<CorrectNamespacesRuleComplementDto>();
      var rule = new CorrectNamespacesRule(
        correctNamespacesRuleComplementDto);
      var file1 = Substitute.For<ISourceCodeFile>();
      var file2 = Substitute.For<ISourceCodeFile>();
      var file3 = Substitute.For<ISourceCodeFile>();
      var files = new List<ISourceCodeFile>
      {
        file1, file2, file3
      };
      var report = Any.Instance<IAnalysisReportInProgress>();

      //WHEN
      rule.Check(files, report);

      //THEN
      Received.InOrder(() =>
      {
        file1.Received(1).EvaluateNamespacesCorrectness(report, rule.ToString());
        file2.Received(1).EvaluateNamespacesCorrectness(report, rule.ToString());
        file3.Received(1).EvaluateNamespacesCorrectness(report, rule.ToString());
        report.FinishedChecking(correctNamespacesRuleComplementDto.ProjectAssemblyNamePattern +
                                " hasCorrectNamespaces");
      });
    }
  }
}
