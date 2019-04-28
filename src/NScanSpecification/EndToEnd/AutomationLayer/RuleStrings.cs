using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public static class RuleStrings
  {
    public static string String(this NoCircularUsingsRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }

    public static string String(this CorrectNamespacesRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }

    public static string String(this IndependentRuleComplementDto dto)
    {
      return $"{dto.DependingPattern.Description()} {dto.RuleName} {dto.DependencyType}:{dto.DependencyPattern.Pattern}";
    }

    public static string String(this IsAnnotatedRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName} {dto.ClassNameInclusionPattern.Description()}:{dto.MethodNameInclusionPattern.Description()}";
    }

    public static string String(this RuleUnionDto dto)
    {
      return dto.Match(String, String, String, String);
    }
  }
}