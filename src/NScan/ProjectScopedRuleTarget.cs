using System.Collections.Generic;
using NScan.Lib;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;

namespace NScan.Domain
{
  public class ProjectScopedRuleTarget : IProjectScopedRuleTarget //bug move
  {
    private readonly string _assemblyName;
    private readonly IReadOnlyList<ISourceCodeFile> _sourceCodeFiles;
    private readonly string _targetFramework;

    public ProjectScopedRuleTarget(
      string assemblyName,
      IReadOnlyList<ISourceCodeFile> sourceCodeFiles, 
      string targetFramework)
    {
      _assemblyName = assemblyName;
      _sourceCodeFiles = sourceCodeFiles;
      _targetFramework = targetFramework;
    }

    public void AnalyzeFiles(IProjectFilesetScopedRule rule, IAnalysisReportInProgress report)
    {
      rule.Check(_sourceCodeFiles, report);
    }

    public bool HasProjectAssemblyNameMatching(Pattern pattern)
    {
      return pattern.IsMatch(_assemblyName);
    }

    public void ValidateTargetFrameworkWith(ITargetFrameworkCheck targetFrameworkCheck,
      IAnalysisReportInProgress analysisReportInProgress)
    {
      targetFrameworkCheck.ApplyTo(_assemblyName, _targetFramework, analysisReportInProgress);
    }
  }
}