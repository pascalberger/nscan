﻿using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;

namespace NScan.Domain
{
  public interface IReferencedProject
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject);
    void ResolveAsReferenceOf(IReferencingProject project);
    void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress);
  }
}