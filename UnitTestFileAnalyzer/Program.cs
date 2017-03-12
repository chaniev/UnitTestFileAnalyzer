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
            string filecontent = @"
[TestClass]
public class Sample
{
   public string FooProperty {get; set;}
   
   [TestMethod]
   public void FooMethod()
   {
   }

   [TestMethod]
   public void FooMethod_Test()
   {
   }

}";
            var sp = @"C:\Users\Sony\Documents\Visual Studio 2015\Projects\UnitTestNameDiagnosticAnalyzer\UnitTestNameDiagnosticAnalyzer.sln";
            var pac = new ProjectAnalyzerCollection(sp);
            pac.Analyze();
            pac.Save(Console.WriteLine);
            //var classAnalyzer=new ClassAnalyzer(filecontent);
            //classAnalyzer.Parse();
            //Console.WriteLine("Incorrect method names");
            //foreach (var methodname in classAnalyzer.Error)
            //{
            //    Console.WriteLine(methodname);
            //}
            Console.ReadLine();
        }

    }
}