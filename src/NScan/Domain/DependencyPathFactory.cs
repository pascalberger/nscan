using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public class DependencyPathFactory : IDependencyPathFactory
  {
    public IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination)
    {
      return new DependencyPathInProgress(
        destination, 
        projects => new ProjectDependencyPath(projects, new ProjectFoundSearchResultFactory()), new List<IReferencedProject>());
    }
  }
}