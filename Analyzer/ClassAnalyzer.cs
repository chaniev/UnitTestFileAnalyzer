using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FileAnalyzer
{
    public class ClassAnalyzer
    {
        private readonly string pattern = @"(\w+)(_\w+)(_Test\b\S{0})";
        private readonly SyntaxTree m_tree;
        private readonly Regex m_regex;

        public ClassAnalyzer(string _content)
        {
            m_regex = new Regex(pattern);
            Error = new List<string>();
            m_tree = CSharpSyntaxTree.ParseText(_content);
        }

        public ClassAnalyzer(Document _document)
        {
            m_regex = new Regex(pattern);
            Error = new List<string>();
            m_tree = _document.GetSyntaxTreeAsync().Result;
        }

        public List<string> Error { get; }

        public void Parse()
        {
            var members = m_tree.GetRoot().DescendantNodes().OfType<MemberDeclarationSyntax>();

            foreach (var member in members)
            {
                var classdeclaration = member as ClassDeclarationSyntax;
                if (classdeclaration == null) continue;
                if (!IsTestClass(classdeclaration)) continue;
                foreach (var method in classdeclaration.Members)
                {
                    var methodds = method as MethodDeclarationSyntax;
                    if (methodds == null) continue;
                    var testmethod = IsTestMesthod(methodds);
                    if (!testmethod) continue;
                    var methodname = methodds.Identifier.ToString();
                    if (!checkMethodNamePattern(methodname))
                        Error.Add(methodname);
                }
            }
        }

        private bool IsTestClass(ClassDeclarationSyntax classdeclaration)
        {
            foreach (var attlist in classdeclaration.AttributeLists)
            {
                if (attlist.Attributes.Select(attribute => attribute.Name.ToString()).Any(tt => tt == "TestClass"))
                {
                    return true;
                }
            }
            return false;
        }

        private bool checkMethodNamePattern(string name)
        {
            return m_regex.IsMatch(name);
        }

        private bool IsTestMesthod(MethodDeclarationSyntax methodds)
        {
            foreach (var attlist in methodds.AttributeLists)
            {
                if (attlist.Attributes.Select(attribute => attribute.Name.ToString()).Any(tt => tt == "TestMethod"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}