﻿using System;

namespace NScanSpecification.Lib.AutomationLayer
{
  public class HasCorrectNamespacesMessage : GenericReportedMessage<HasCorrectNamespacesMessage>
  {
    public HasCorrectNamespacesMessage(string returnValue) : base(returnValue)
    {
    }

    public HasCorrectNamespacesMessage ButFoundIncorrectNamespaceFor(string fileName, string actualNamespace)
    {
      return NewInstance(ToString() +
                         $" but the file {fileName} has incorrect namespace {actualNamespace}");
    }

    public HasCorrectNamespacesMessage ButNoNamespaceDeclaredIn(string fileName)
    {
      return NewInstance(ToString() + $" but the file {fileName} has no namespace declared");
    }

    public HasCorrectNamespacesMessage ButHasMultipleNamespaces(string fileName, params string[] namespaces)
    {
      return NewInstance(ToString() + $" but the file {fileName} declares multiple namespaces: {String.Join(", ", namespaces)}");
    }


    public HasCorrectNamespacesMessage ExpectedNamespace(string projectName, string rootNamespace)
    {
      return NewInstance(ToString() + $"{Environment.NewLine}" + $"{projectName} has root namespace {rootNamespace}");
    }

    protected override HasCorrectNamespacesMessage NewInstance(string str)
    {
      return new HasCorrectNamespacesMessage(str);
    }

    public static HasCorrectNamespacesMessage HasCorrectNamespaces(string projectGlob)
    {
      return new HasCorrectNamespacesMessage(TestRuleFormats.FormatCorrectNamespacesRule(projectGlob));
    }

  }
}