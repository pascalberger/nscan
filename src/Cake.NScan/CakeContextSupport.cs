﻿using System;
using Cake.Core.Diagnostics;
using TddXt.NScan.App;
using TddXt.NScan.RuleInputData;

namespace Cake.NScan
{
  public class CakeContextSupport : INScanSupport
  {
    private readonly ICakeLog _contextLog;

    public CakeContextSupport(ICakeLog contextLog)
    {
      _contextLog = contextLog;
    }

    public void Report(Exception exceptionFromResolution)
    {
      _contextLog.Write(Verbosity.Minimal, LogLevel.Error, exceptionFromResolution.ToString());
    }

    public void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException, string projectFilePath)
    {
      _contextLog.Write(Verbosity.Minimal, LogLevel.Warning,
        $"Invalid format - skipping {projectFilePath} because of {invalidOperationException}");
    }

    public void LogIndependentRule(IndependentRuleComplementDto independentRuleComplementDto)
    {
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, $"Discovered rule: {independentRuleComplementDto.DependingPattern.Description()} {independentRuleComplementDto.RuleName} {independentRuleComplementDto.DependencyType}:{independentRuleComplementDto.DependencyPattern.Pattern}" + "");
    }

    public void LogNamespacesRule(CorrectNamespacesRuleComplementDto dto)
    {
      _contextLog.Write(Verbosity.Diagnostic, LogLevel.Debug, $"Discovered rule: {dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }
  }
}