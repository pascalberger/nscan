﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TddXt.NScan.App;
using TddXt.NScan.CompositionRoot;
using TddXt.NScan.Xml;

namespace TddXt.NScan.Specification
{
  public class NScanDriver
  {
    private readonly ISupport _consoleSupport = new ConsoleSupport();
    private readonly List<XmlProject> _xmlProjects = new List<XmlProject>();
    private readonly List<(string, string)> _independentOfProjectRules = new List<(string, string)>();
    private readonly List<(string, string)> _independentOfPackageRules = new List<(string, string)>();
    private Analysis _analysis;

    public XmlProjectDsl HasProject(string assemblyName)
    {
      var xmlProject = new XmlProject()
      {
        AbsolutePath = AbsolutePathFor(assemblyName),
        PropertyGroups = new List<XmlPropertyGroup>()
        {
          new XmlPropertyGroup()
          {
            AssemblyName = assemblyName
          }
        },
        ItemGroups = new List<XmlItemGroup>()
      };
      _xmlProjects.Add(xmlProject);
      return new XmlProjectDsl(xmlProject);
    }

    public static string AbsolutePathFor(string assemblyName)
    {
      return @"C:\" + assemblyName + ".cs";
    }

    public void AddIndependentOfProjectRule(string dependingAssemblyName, string dependentAssemblyName)
    {
      _independentOfProjectRules.Add((dependingAssemblyName, dependentAssemblyName));
    }

    public void AddIndependentOfPackageRule(string projectName, string packageName)
    {
      _independentOfPackageRules.Add((projectName, packageName));
    }

    public void PerformAnalysis()
    {
      _analysis = Analysis.PrepareFor(_xmlProjects, _consoleSupport);

      foreach (var (depending, dependent) in _independentOfProjectRules)
      {
        _analysis.IndependentOfProject(depending, dependent);
      }

      foreach (var (depending, packageName) in _independentOfPackageRules)
      {
        _analysis.IndependentOfPackage(depending, packageName);
      }

      _analysis.Run();
    }

    public void ReportShouldContainText(string expected)
    {
      _analysis.Report.Should().Contain(expected);
      //todo what bout return code?
    }

    public void ShouldIndicateSuccess()
    {
      _analysis.ReturnCode.Should().Be(0);
    }

    public void ShouldIndicateFailure()
    {
      _analysis.ReturnCode.Should().Be(-1);
    }

  }

  public class XmlProjectDsl
  {
    private readonly XmlProject _xmlProject;

    public XmlProjectDsl(XmlProject xmlProject)
    {
      _xmlProject = xmlProject;
      _xmlProject.ItemGroups = _xmlProject.ItemGroups ?? new List<XmlItemGroup>();
    }

    public void WithReferences(params string[] names)
    {
      _xmlProject.ItemGroups.Add(
        new XmlItemGroup() {ProjectReferences = ProjectReferencesFrom(names)}
      );
    }

    public void WithPackages(params string[] packageNames)
    {
      _xmlProject.ItemGroups.Add(
        new XmlItemGroup() {PackageReferences = PackageReferencesFrom(packageNames)}
      );
    }

    private IReadOnlyList<XmlPackageReference> PackageReferencesFrom(string[] packageNames)
    {
      return packageNames.Select(name => new XmlPackageReference()
      {
        Include = name,
        Version = "0.0.0"
      }).ToList();
    }

    private static List<XmlProjectReference> ProjectReferencesFrom(string[] names)
    {
      return names.Select(n => new XmlProjectReference()
      {
        Include = NScanDriver.AbsolutePathFor(n)
      }).ToList();
    }


  }
}