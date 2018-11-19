using TddXt.NScan.RuleInputData;

namespace TddXt.NScan.Domain
{
  public interface IRuleFactory
  {
    IDependencyRule CreateDependencyRuleFrom(IndependentRuleComplementDto independentRuleComplementDto);
  }
}