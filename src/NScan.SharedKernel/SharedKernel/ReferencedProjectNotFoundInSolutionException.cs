﻿using System;
using System.Collections.Generic;

namespace NScan.SharedKernel.SharedKernel
{
  public class ReferencedProjectNotFoundInSolutionException : Exception
  {
    public ReferencedProjectNotFoundInSolutionException(
      string message, KeyNotFoundException keyNotFoundException)
      : base(message, keyNotFoundException)
    {
    }
  }
}