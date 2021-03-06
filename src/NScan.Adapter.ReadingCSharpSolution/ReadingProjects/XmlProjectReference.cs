﻿using System.Xml.Serialization;
using AtmaFileSystem;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
{
#nullable disable
  [XmlRoot(ElementName = "ProjectReference")]
  public class XmlProjectReference
  {
    [XmlAttribute(AttributeName = "Include")]
    public string Include { get; set; }

    [XmlIgnore]
    public AbsoluteFilePath FullIncludePath { get; set; }
  }
}