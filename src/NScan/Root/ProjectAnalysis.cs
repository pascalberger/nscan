using System.Collections.Generic;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  public class ProjectAnalysis : ISpecificKindOfRuleAnalysis<ProjectScopedRuleUnionDto>
  {
    private readonly IProjectScopedRuleSet _projectScopedRuleSet;
    private readonly IProjectScopedRuleFactory _projectScopedRuleFactory;

    public ProjectAnalysis(
      IProjectScopedRuleSet projectScopedRuleSet,
      IProjectScopedRuleFactory projectScopedRuleFactory)
    {
      _projectScopedRuleSet = projectScopedRuleSet;
      _projectScopedRuleFactory = projectScopedRuleFactory;
    }

    public void PerformOn(ISolution solution, IAnalysisReportInProgress analysisReportInProgress)
    {
      solution.Check(_projectScopedRuleSet, analysisReportInProgress);
    }

    public void Add(IEnumerable<ProjectScopedRuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(new CreateProjectScopedRuleVisitor(_projectScopedRuleFactory, _projectScopedRuleSet));
      }
    }
  }
}