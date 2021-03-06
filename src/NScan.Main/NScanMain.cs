using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Functional.Maybe;
using NScan.Adapter.ReadingCSharpSolution.ReadingProjects;
using NScan.Adapter.ReadingRules;
using NScan.Domain;
using NScan.Lib;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using NScan.SharedKernel.WritingProgramOutput.Ports;
using Sprache;

namespace TddXt.NScan
{
  public static class NScanMain
  {
    /// <summary>
    /// Entry point
    /// </summary>
    /// <param name="inputArguments">arguments</param>
    /// <param name="output">output for report</param>
    /// <param name="support">logging stuff</param>
    /// <returns></returns>
    public static int Run(
      InputArgumentsDto inputArguments, 
      INScanOutput output, 
      INScanSupport support)
    {
      try
      {
        //SpinWait.SpinUntil(() => Debugger.IsAttached);

        output.WriteVersion(Versioning.VersionOf(Assembly.GetExecutingAssembly()));

        var csharpProjectDtos = ReadCsharpProjects(inputArguments, support);
        var analysis = Analysis.PrepareFor(csharpProjectDtos, support);

        var rulesString = ReadRulesTextFrom(inputArguments);
        
        var dependencyPathDtos = ParserRulePreface.Then(ParseDependencyPathBasedRule.Complement).Many().Parse(rulesString).WhereValueExist();
        LogDependencyPathRules(dependencyPathDtos, support);
        analysis.AddDependencyPathRules(dependencyPathDtos);
        
        var projectScopedDtos = ParserRulePreface.Then(ParseProjectScopedRule.Complement).Many().Parse(rulesString).WhereValueExist();
        analysis.AddProjectScopedRules(projectScopedDtos);
        LogProjectScopedRules(projectScopedDtos, support);

        
        var namespaceBasedDtos = ParserRulePreface.Then(ParseNamespaceBasedRule.Complement).Many().Parse(rulesString).WhereValueExist();
        LogNamespaceBasedRules(namespaceBasedDtos, support);
        analysis.AddNamespaceBasedRules(namespaceBasedDtos);

        analysis.Run();
        output.WriteAnalysisReport(analysis.Report);
        return analysis.ReturnCode;
      }
      catch (Exception e)
      {
        support.Report(e);
        return -2;
      }
    }

    private static IEnumerable<CsharpProjectDto> ReadCsharpProjects(InputArgumentsDto inputArguments, INScanSupport support)
    {
      var paths = ProjectPaths.From(
        inputArguments.SolutionPath.OrThrow().ToString(),
        support);
      return paths.LoadXmlProjects();
    }

    private static string ReadRulesTextFrom(InputArgumentsDto inputArguments)
    {
      return File.ReadAllText(inputArguments.RulesFilePath.OrThrow().ToString());
    }

    private static void LogDependencyPathRules(
      IEnumerable<DependencyPathBasedRuleUnionDto> enumerable,
      INScanSupport support)
    {
      foreach (var ruleUnion in enumerable)
      {
        ruleUnion.Accept(new RuleLoggingVisitor(support));
      }
    }
    private static void LogProjectScopedRules(
      IEnumerable<ProjectScopedRuleUnionDto> enumerable,
      INScanSupport support)
    {
      foreach (var ruleUnion in enumerable)
      {
        ruleUnion.Accept(new RuleLoggingVisitor(support));
      }
    }
    private static void LogNamespaceBasedRules(
      IEnumerable<NamespaceBasedRuleUnionDto> enumerable,
      INScanSupport support)
    {
      foreach (var ruleUnion in enumerable)
      {
        ruleUnion.Accept(new RuleLoggingVisitor(support));
      }
    }
  }
}