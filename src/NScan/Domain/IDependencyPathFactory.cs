﻿using TddXt.NScan.App;

namespace TddXt.NScan.Domain
{
  public interface IDependencyPathFactory
  {
    IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination);
  }
}