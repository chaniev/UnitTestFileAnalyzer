using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UnitTestFileAnalyzer
{
    public class ClassAnalyzer
    {
        private string m_content;
        private SyntaxTree m_tree;

        public ClassAnalyzer(string _content)
        {
            m_content = _content;
            Error = new List<string>();
            m_tree = CSharpSyntaxTree.ParseText(m_content);

        }

        public ClassAnalyzer(Document _document)
        {
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
                foreach (var attribute in attlist.Attributes)
                {
                    var tt = attribute.Name.ToString();
                    if (tt == "TestClass") return true;
                }
            }
            return false;
        }

        private bool checkMethodNamePattern(string name)
        {
            if (name.Contains("_Test")) return true;
            return false;
        }

        private bool IsTestMesthod(MethodDeclarationSyntax methodds)
        {
            foreach (var attlist in methodds.AttributeLists)
            {
                foreach (var attribute in attlist.Attributes)
                {
                    var tt = attribute.Name.ToString();
                    if (tt == "TestMethod") return true;
                }
            }
            return false;
        }
    }
}