using System.Linq;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain
{
  public class NoCircularUsingsRule : INamespacesBasedRule
  {
    private readonly NoCircularUsingsRuleComplementDto _ruleDto;
    private readonly IRuleViolationFactory _ruleViolationFactory;

    public NoCircularUsingsRule(NoCircularUsingsRuleComplementDto ruleDto, IRuleViolationFactory ruleViolationFactory)
    {
      _ruleDto = ruleDto;
      _ruleViolationFactory = ruleViolationFactory;
    }

    public string Description()
    {
      return $"{_ruleDto.ProjectAssemblyNamePattern.Description()} {_ruleDto.RuleName}";
    }

    public void Evaluate(
      string projectAssemblyName, 
      INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report)
    {
      var cycles = namespacesCache.RetrieveCycles();
      if (cycles.Any())
      {
        report.Add(_ruleViolationFactory.NoCyclesRuleViolation(Description(), projectAssemblyName, cycles));
      }
    }
  }
}