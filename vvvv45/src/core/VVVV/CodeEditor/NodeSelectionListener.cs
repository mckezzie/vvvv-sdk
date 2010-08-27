﻿using System;
using VVVV.Core.Model;
using VVVV.Core.Model.CS;
using VVVV.PluginInterfaces.V2;

namespace VVVV.HDE.CodeEditor
{
	public class NodeSelectionListener : INodeSelectionListener
	{
		private CodeEditorForm FCodeEditorForm;
		
		public NodeSelectionListener(CodeEditorForm codeEditorForm)
		{
			FCodeEditorForm = codeEditorForm;
		}
		
		public void NodeSelectionChangedCB(INode[] nodes)
		{
			foreach (var node in nodes)
			{
				var nodeInfo = node.GetNodeInfo();
				var executable = nodeInfo.Executable;
				
				if (executable == null)
					continue;
				
				var project = executable.Project;
				
				if (project == null)
					continue;
				
				if (project is CSProject)
				{
					// Try to find file where NodeInfo is defined.
					bool found = false;
					foreach (var doc in project.Documents)
					{
						if (doc is CSDocument)
						{
							var csDoc = doc as CSDocument;
							var parseInfo = csDoc.ParseInfo;
							var compilationUnit = parseInfo.MostRecentCompilationUnit;
							
							if (compilationUnit != null)
							{
								foreach (var clss in compilationUnit.Classes)
								{
									foreach (var attribute in clss.Attributes)
									{
										var attributeType = attribute.AttributeType;
										if (attributeType.Name == typeof(PluginInfoAttribute).Name)
										{
											// Check name
											string name = null;
											if (attribute.NamedArguments.ContainsKey("Name"))
												name = (string) attribute.NamedArguments["Name"];
											else if (attribute.PositionalArguments.Count >= 0)
												name = (string) attribute.PositionalArguments[0];
											
											if (name != nodeInfo.Name)
												continue;
											
											// Check category
											string category = null;
											if (attribute.NamedArguments.ContainsKey("Category"))
												category = (string) attribute.NamedArguments["Category"];
											else if (attribute.PositionalArguments.Count >= 1)
												category = (string) attribute.PositionalArguments[1];
											
											if (category != nodeInfo.Category)
												continue;

											// Possible match
											bool match = true;
											
											// Check version
											if (nodeInfo.Version != null)
											{
												string version = null;
												if (attribute.NamedArguments.ContainsKey("Version"))
													version = (string) attribute.NamedArguments["Version"];
												else if (attribute.PositionalArguments.Count >= 2)
													version = (string) attribute.PositionalArguments[2];
												
												match = version == nodeInfo.Version;
											}
											
											if (match)
											{
												found = true;
												var tabPage = FCodeEditorForm.Open(csDoc);
												var codeEditor = tabPage.Controls[0] as CodeEditor;
												codeEditor.JumpTo(attribute.Region.BeginLine - 1);
												break;
											}
										}
									}
									
									if (found) break;
								}
							}
						}
						
						if (found) break;
					}
				}
				else
				{
					// Open all documents
					foreach (var doc in project.Documents)
					{
						if (doc is ITextDocument)
						{
							// TODO: FCodeEditorPlugin.Open(doc as ITextDocument, nodeInfo);
							FCodeEditorForm.Open(doc as ITextDocument);
						}
					}
				}
			}
		}
	}
}