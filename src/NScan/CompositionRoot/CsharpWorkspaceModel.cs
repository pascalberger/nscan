using System.Collections.Generic;
using System.IO;
using System.Linq;
using TddXt.NScan.App;
using TddXt.NScan.Xml;

namespace TddXt.NScan.CompositionRoot
{
  public interface IWorkspaceModel
  {
    Dictionary<ProjectId, IDotNetProject> LoadProjects();
  }

  public class CsharpWorkspaceModel : IWorkspaceModel
  {
    private readonly ISupport _support;
    private readonly IReadOnlyList<XmlProject> _xmlProjects;

    public CsharpWorkspaceModel(ISupport support, IReadOnlyList<XmlProject> xmlProjects)
    {
      _support = support;
      _xmlProjects = xmlProjects;
    }

    //todo this is pulled into unit test scope, so write UTs for it...
    private (ProjectId, DotNetStandardProject) CreateProject(XmlProject xmlProject)
    {
      return (new ProjectId(xmlProject.AbsolutePath), new DotNetStandardProject(
        DetermineAssemblyName(xmlProject),
        new ProjectId(xmlProject.AbsolutePath), 
        ProjectReferences(xmlProject).Select(MapToProjectId).ToArray(), 
        PackageReferences(xmlProject),
        _support));
    }

    private static string DetermineAssemblyName(XmlProject xmlProject)
    {
      return xmlProject.PropertyGroups.First().AssemblyName ?? Path.GetFileNameWithoutExtension(xmlProject.AbsolutePath);
    }

    private static ProjectId MapToProjectId(XmlProjectReference dto)
    {
      return new ProjectId(dto.Include);
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

    public static IEnumerable<XmlProjectReference> ProjectReferences(XmlProject xmlProject)
    {
      var xmlItemGroups = xmlProject.ItemGroups.Where(ig => ig.ProjectReferences.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups.First().ProjectReferences;
      }

      return new List<XmlProjectReference>();
    }

    private IEnumerable<PackageReference> PackageReferences(XmlProject xmlProject)
    {
      var xmlItemGroups = xmlProject.ItemGroups.Where(ig => ig.PackageReferences.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups.First().PackageReferences.Select(r => new PackageReference(r.Include, r.Version));
      }

      return new List<PackageReference>();
    }
  }
}