using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public interface IPathCache
  {
    void BuildStartingFrom(params IDependencyPathBasedRuleTarget[] rootProjects);
    void Check(IDependencyRule rule, IAnalysisReportInProgress report);
  }
}