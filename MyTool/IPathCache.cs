using MyTool.App;

namespace MyTool
{
  public interface IPathCache
  {
    void BuildStartingFrom(params IDotNetProject[] rootProjects);
    void Check(IDependencyRule ruleSet, IAnalysisReportInProgress report);
  }
}