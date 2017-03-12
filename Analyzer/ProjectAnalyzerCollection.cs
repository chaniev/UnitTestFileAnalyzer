using System;
using System.Collections.Generic;
using System.Linq;
using FileAnalyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace Analyzer
{
    public class ProjectAnalyzerCollection
    {
        private readonly IEnumerable<Project> m_projects;

        public string SolutionPath { get; }

        public Dictionary<Tuple<string, string>, List<string>> Errors { get; } = new Dictionary<Tuple<string, string>, List<string>>();

        public ProjectAnalyzerCollection(string _solutionPath)
        {
            SolutionPath = _solutionPath;
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution currSolution = workspace.OpenSolutionAsync(SolutionPath).Result;
            m_projects = currSolution.Projects;
        }

        public void Analyze()
        {
            foreach (var project in m_projects)
            {
                foreach (var document in project.Documents)
                {
                    var ca = new ClassAnalyzer(document);
                    ca.Parse();
                    if (ca.Error.Any())
                    {
                       var key=new Tuple<string, string>(project.Name, document.Name);
                       Errors.Add(key, ca.Error);
                    } 
                }
            }
        }

        public void Save(Action<string> _action)
        {
            foreach (var error in Errors)
            {
                _action.Invoke($"project name: {error.Key.Item1} file name: {error.Key.Item2}");
                foreach (var methodName in error.Value)
                {
                    _action.Invoke($"{methodName}");
                }
                _action.Invoke(string.Empty);
            }
        }
    }
}