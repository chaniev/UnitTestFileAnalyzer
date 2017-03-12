using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp; 
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace UnitTestFileAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            var sp = @"C:\Users\Sony\Documents\Visual Studio 2015\Projects\UnitTestNameDiagnosticAnalyzer\UnitTestNameDiagnosticAnalyzer.sln";
            var pac = new ProjectAnalyzerCollection(sp);
            pac.Analyze();
            pac.Save(Console.WriteLine);
            Console.ReadLine();
        }

    }
}