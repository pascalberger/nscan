﻿using NScan.SharedKernel;

namespace NScan.Domain
{
  public interface IReferencingProject
  {
    void AddReferencedProject(ProjectId projectId, IReferencedProject referencedProject);
    void ResolveReferencesFrom(ISolutionContext solution);
    bool IsRoot();
    void ResolveAsReferencing(IReferencedProject project);
  }
}