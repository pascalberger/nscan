using System.Collections.Generic;
using GlobExpressions;
using Sprache;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.ReadingRules.Adapters
{
  public static class ParseRule
  {
    private static readonly Parser<IEnumerable<char>> Spaces = Parse.WhiteSpace.AtLeastOnce();
    private static readonly Parser<IEnumerable<char>> OptionalSpacesUntilEol = Parse.WhiteSpace.Until(Parse.LineTerminator);
    private static readonly Parser<string> TextUntilWhitespace = Parse.AnyChar.Until(Spaces).Text();
    private static readonly Parser<IEnumerable<char>> ExceptKeyword = Parse.String("except");
    private static readonly Parser<string> TextUntilEol = Parse.AnyChar.Until(Parse.LineTerminator).Text().Token();
    private static readonly Parser<IEnumerable<char>> IndependentOfKeyword = Parse.String(RuleNames.IndependentOf);

    public static Parser<RuleUnionDto> FromLine()
    {
      return from depending in TextUntilWhitespace
        from optionalException in ExceptKeyword.Token().Then(_ => TextUntilWhitespace).Optional()
        from ruleUnion in Complement(DependingPattern(depending, optionalException))
        select ruleUnion;
    }

    private static Parser<RuleUnionDto> Complement(
      Pattern dependingPattern)
    {
      return IndependentOfRuleComplement(dependingPattern)
        .Or(HasCorrectNamespacesRuleComplement(dependingPattern))
        .Or(HasNoCircularUsingsRuleComplement(dependingPattern));
    }

    private static Parser<RuleUnionDto> HasNoCircularUsingsRuleComplement(Pattern dependingPattern)
    {
      return Parse.String(RuleNames.HasNoCircularUsings).Then(_ => OptionalSpacesUntilEol).Return(
        RuleUnionDto.With(new NoCircularUsingsRuleComplementDto
        {
          ProjectAssemblyNamePattern = dependingPattern,
        }));
    }

    private static Parser<RuleUnionDto>
      HasCorrectNamespacesRuleComplement(Pattern dependingPattern)
    {
      return Parse.String(RuleNames.HasCorrectNamespaces).Then(_ => OptionalSpacesUntilEol).Return(
        RuleUnionDto.With(new CorrectNamespacesRuleComplementDto()
        {
          ProjectAssemblyNamePattern = dependingPattern,
        }));
    }

    private static Parser<RuleUnionDto>
      IndependentOfRuleComplement(Pattern dependingPattern)
    {

      return IndependentOfKeyword
        .Then(_ => from dependencyType in Parse.AnyChar.Until(Parse.Char(':')).Text().Token()
          from dependency in TextUntilEol
          select RuleUnionDto.With(new IndependentRuleComplementDto
            {
              DependingPattern = dependingPattern,
              DependencyPattern = new Glob(dependency),
              DependencyType = dependencyType,
            }));
    }

    private static Pattern DependingPattern(string depending, IOption<string> optionalException)
    {
      return optionalException.IsDefined ? Pattern.WithExclusion(depending, optionalException.Get()) : Pattern.WithoutExclusion(depending);
    }
  }

  public enum RuleTypes
  {
    PathRule, 
    ProjectScopedRule
  }
}