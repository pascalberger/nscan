﻿using System;

namespace NScanSpecification.Lib.AutomationLayer
{
  public class HasNoCircularUsingsMessage : GenericReportedMessage<HasNoCircularUsingsMessage>
  {
    public HasNoCircularUsingsMessage(string returnValue) : base(returnValue)
    {
    }

    public HasNoCircularUsingsMessage CycleFound(string projectName, params string[] cyclePath)
    {
      return NewInstance(this + Environment.NewLine +
                         $"Discovered cycle(s) in project {projectName}:{Environment.NewLine}" +
                         Format(cyclePath));
    }

    private static string Format(string[] cyclePath)
    {
      return PathFormat.For("Cycle 1", cyclePath);
    }

    protected override HasNoCircularUsingsMessage NewInstance(string str)
    {
      return new HasNoCircularUsingsMessage(str);
    }

    public static HasNoCircularUsingsMessage HasNoCircularUsings(string projectGlob)
    {
      return new HasNoCircularUsingsMessage(TestRuleFormats.FormatNoCircularUsingsRule(projectGlob));
    }
  }
}