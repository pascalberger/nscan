﻿using System.Collections.Generic;
using System.Linq;
using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;

namespace NScan.Domain
{
  public class DotNetStandardSolution : ISolution, ISolutionContext
  {
    private readonly IPathCache _pathCache;
    private readonly IReadOnlyDictionary<ProjectId, IDotNetProject> _projectsById;
    private readonly IReadOnlyList<INamespaceBasedRuleTarget> _namespaceBasedRuleTargets;
    private readonly IReadOnlyList<IProjectScopedRuleTarget> _projectScopedRuleTargets;

    public DotNetStandardSolution(
      IReadOnlyDictionary<ProjectId, IDotNetProject> projectsById,
      IPathCache pathCache, 
      IReadOnlyList<INamespaceBasedRuleTarget> namespaceBasedRuleTargets, 
      IReadOnlyList<IProjectScopedRuleTarget> projectScopedRuleTargets)
    {
      _projectsById = projectsById;
      _namespaceBasedRuleTargets = namespaceBasedRuleTargets;
      _pathCache = pathCache;
      _projectScopedRuleTargets = projectScopedRuleTargets;
    }

    public void ResolveAllProjectsReferences()
    {
      //backlog use the analysis report to write what projects are skipped - write a separate acceptance test for that
      foreach (var referencingProject in _projectsById.Values)
      {
        referencingProject.ResolveReferencesFrom(this);
      }
    }

    public void PrintDebugInfo()
    {
      foreach (var project in _projectsById.Values.Where(v => v.IsRoot()))
      {
        project.Print(0);
      }
    }

    public void Check(IPathRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
    {
      ruleSet.Check(_pathCache, analysisReportInProgress);
    }

    public void Check(IProjectScopedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
    {
      ruleSet.Check(_projectScopedRuleTargets, analysisReportInProgress);
    }

    public void Check(INamespacesBasedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
    {
      ruleSet.Check(_namespaceBasedRuleTargets, analysisReportInProgress);
    }

    public void BuildCache()
    {
      _pathCache.BuildStartingFrom(RootProjects());
      foreach (var dotNetProject in _namespaceBasedRuleTargets)
      {
        dotNetProject.RefreshNamespacesCache();
      }
    }

    public void ResolveReferenceFrom(IReferencingProject referencingProject, ProjectId referencedProjectId)
    {
      try
      {
        var referencedProject = _projectsById[referencedProjectId];

        referencingProject.ResolveAsReferencing(referencedProject);
        referencedProject.ResolveAsReferenceOf(referencingProject);
      }
      catch (KeyNotFoundException e)
      {
        throw new ReferencedProjectNotFoundInSolutionException(
          CouldNotFindProjectFor(referencedProjectId), e);
      }
    }

    private static string CouldNotFindProjectFor(ProjectId referencedProjectId)
    {
      return $"Could not find referenced project {referencedProjectId} " +
             "probably because it was in an incompatible format and was skipped during project collection phase.";
    }

    private IDependencyPathBasedRuleTarget[] RootProjects()
    {
      return Projects().Where(project => project.IsRoot()).ToArray();
    }

    private IReadOnlyList<IDotNetProject> Projects()
    {
      return _projectsById.Values.ToList();
    }
  }
}