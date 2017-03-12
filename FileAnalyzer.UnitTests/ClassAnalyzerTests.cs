using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileAnalyzer.UnitTests
{
    [TestClass]
    public class ClassAnalyzerTests
    {
        [TestMethod]
        public void Parse_ErrorIfNotContaintPostfix_Test()
        {
            const string filecontent = @"
 [TestClass]
 public class Sample
 {
    [TestMethod]
    public void FooMethod()
    {
    }
 }";
            const string expectedError = "FooMethod";
            var target = new ClassAnalyzer(filecontent);
            target.Parse();
            Assert.IsTrue(target.Error.Contains(expectedError));
        }

        [TestMethod]
        public void Parse_NoErrorIfContaintPostfix_Test()
        {
            const string filecontent = @"
 [TestClass]
 public class Sample
 {
    [TestMethod]
    public void FooMethod_Descr_Test()
    {
    }
 }";
            var target = new ClassAnalyzer(filecontent);
            target.Parse();
            Assert.IsFalse(target.Error.Any());
        }

        [TestMethod]
        public void Parse_ErrorIfNotContaintDescriptionPostfix_Test()
        {
            const string filecontent = @"
 [TestClass]
 public class Sample
 {
    [TestMethod]
    public void FooMethod_Test()
    {
    }
 }";
            const string expected = "FooMethod_Test";
            var target = new ClassAnalyzer(filecontent);
            target.Parse();
            Assert.IsTrue(target.Error.Contains(expected));
        }
    }
}
