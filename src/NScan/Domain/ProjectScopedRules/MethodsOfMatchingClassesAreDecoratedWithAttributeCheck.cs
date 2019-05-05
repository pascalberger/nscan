using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
  public class MethodsOfMatchingClassesAreDecoratedWithAttributeCheck : ISourceCodeFileContentCheck
  {
    private readonly HasAttributesOnRuleComplementDto _ruleDto;

    public MethodsOfMatchingClassesAreDecoratedWithAttributeCheck(HasAttributesOnRuleComplementDto ruleDto)
    {
      _ruleDto = ruleDto;
    }

    public void ApplyTo(ISourceCodeFileInNamespace sourceCodeFile, string ruleDescription, IAnalysisReportInProgress report)
    {
      sourceCodeFile.EvaluateMethodsHavingCorrectAttributes(
        report, 
        _ruleDto.ClassNameInclusionPattern, 
        _ruleDto.MethodNameInclusionPattern, 
        ruleDescription);
    }
  }
}