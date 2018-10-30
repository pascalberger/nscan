using TddXt.NScan.App;

namespace TddXt.NScan.CompositionRoot
{
  public class DescribedCondition : IDescribedDependencyCondition
  {
    private readonly IDependencyCondition _dependencyCondition;
    private readonly string _description;

    public DescribedCondition(IDependencyCondition dependencyCondition, string description)
    {
      _dependencyCondition = dependencyCondition;
      _description = description;
    }

    public bool Matches(IProjectSearchResult depending, IReferencedProject dependency)
    {
      return _dependencyCondition.Matches(depending, dependency);
    }

    public string Description()
    {
      return _description;
    }
  }
}