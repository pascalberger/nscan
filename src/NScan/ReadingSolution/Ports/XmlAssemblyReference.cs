﻿using System.Xml.Serialization;
using AtmaFileSystem;

namespace TddXt.NScan.ReadingSolution.Ports
{
#nullable disable
  [XmlRoot(ElementName = "Reference")]
  public class XmlAssemblyReference
  {
    [XmlElement(ElementName = "HintPath")]
    public string HintPath { get; set; }

    [XmlAttribute(AttributeName = "Include")]
    public string Include { get; set; }
  }
}