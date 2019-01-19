using System.Collections.Generic;
using System.IO;
using System.Linq;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingSolution;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.Domain
{
  public interface IWorkspaceModel
  {
    Dictionary<ProjectId, IDotNetProject> LoadProjects();
  }

  public class CsharpWorkspaceModel : IWorkspaceModel
  {
    private readonly INScanSupport _support;
    private readonly IReadOnlyList<XmlProject> _xmlProjects;
    private readonly IRuleViolationFactory _ruleViolationFactory;

    public CsharpWorkspaceModel(
      INScanSupport support, 
      IReadOnlyList<XmlProject> xmlProjects,
      IRuleViolationFactory ruleViolationFactory)
    {
      _support = support;
      _xmlProjects = xmlProjects;
      _ruleViolationFactory = ruleViolationFactory;
    }

    public Dictionary<ProjectId, IDotNetProject> LoadProjects()
    {
      var projects = new Dictionary<ProjectId, IDotNetProject>();
      foreach (var xmlProject in _xmlProjects)
      {
        var (id, project) = CreateProject(xmlProject);
        projects.Add(id, project);
      }

      return projects;
    }

    //todo this is pulled into unit test scope, so write UTs for it...
    private (ProjectId, DotNetStandardProject) CreateProject(XmlProject xmlProject)
    {
      var assemblyName = DetermineAssemblyName(xmlProject);
      var dotNetStandardProject = new DotNetStandardProject(assemblyName,
        IdOf(xmlProject), 
        CsharpProjectReferencesExtraction.ProjectReferences(xmlProject).Select(MapToProjectId).ToArray(),
        PackageReferences(xmlProject),
        AssemblyReferences(xmlProject), 
        SourceCodeFiles(xmlProject), 
        new NamespacesDependenciesCache(),
        _support);
      return (new ProjectId(xmlProject.AbsolutePath), dotNetStandardProject);
    }

    private static ProjectId IdOf(XmlProject xmlProject)
    {
      return new ProjectId(xmlProject.AbsolutePath);
    }

    private List<SourceCodeFile> SourceCodeFiles(XmlProject xmlProject)
    {
      return xmlProject.SourceCodeFiles.Select(scf => new SourceCodeFile(scf, _ruleViolationFactory)).ToList();
    }

    private static string DetermineAssemblyName(XmlProject xmlProject)
    {
      return xmlProject.PropertyGroups.First().AssemblyName ?? Path.GetFileNameWithoutExtension(xmlProject.AbsolutePath);
    }

    private static ProjectId MapToProjectId(XmlProjectReference dto)
    {
      return new ProjectId(dto.Include);
    }

    private static IReadOnlyList<AssemblyReference> AssemblyReferences(XmlProject xmlProject)
    {
      var xmlItemGroups = xmlProject.ItemGroups.Where(
        ig => ig.AssemblyReferences != null && ig.AssemblyReferences.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups
          .First()
          .AssemblyReferences
          .Select(r => new AssemblyReference(r.Include, r.HintPath)).ToList();
      }

      return new List<AssemblyReference>();
    }

    private IReadOnlyList<PackageReference> PackageReferences(XmlProject xmlProject)
    {
      var xmlItemGroups = xmlProject.ItemGroups.Where(
        ig => ig.PackageReferences !=  null && ig.PackageReferences.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups
          .First()
          .PackageReferences
          .Select(r => new PackageReference(r.Include, r.Version)).ToList();
      }

      return new List<PackageReference>();
    }
  }
}