using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodoPad.Models;

namespace TodoPadTests
{
    [TestClass]
    public class RowParseTests
    {
        [TestMethod]
        public void BasicParseTest()
        {
            Row testRow = new Row("Initial version of +Todopad @Coding");
            Assert.IsNotNull(testRow, "Row shouldn't be null.");

            Assert.IsTrue(testRow.HasContexts, "Row should have contexts.");
            Assert.AreEqual(testRow.Contexts[0], "+Todopad", "Row should have correct context.");
            Assert.IsTrue(testRow.HasProjects, "Row should have projects.");
            Assert.AreEqual(testRow.Projects[0], "@Coding", "Row should have correct project.");
        }

        [TestMethod]
        public void InvalidParseTest()
        {
            Row testRow = new Row("");
            Assert.IsFalse(testRow.HasText, "Row shouldn't have text.");
            Assert.IsFalse(testRow.HasContexts, "Row shouldn't have contexts.");
            Assert.IsFalse(testRow.HasProjects, "Row shouldn't have projects.");
        }

        [TestMethod]
        public void CompletedParseTest()
        {
            Row testRow = new Row("x 2013-04-19 (B) due:2013-04-20 +CS3241 Lab 5 @Coding +University");
            Assert.IsTrue(testRow.IsCompleted, "Row should be completed.");
            Assert.IsTrue(testRow.HasContexts, "Row should have contexts.");
            Assert.IsTrue(testRow.HasProjects, "Row should have projects.");
            Assert.IsTrue(testRow.HasPriority, "Row should have priority.");

            Assert.AreEqual(testRow.CompletionDate, new DateTime(2013, 04, 19), "Row should have correct completion date.");
        }

        [TestMethod]
        public void AdvancedParseTest()
        {
            Row testRow = new Row("(B) 2013-04-19 due:2013-04-20 +CS3241 Lab 5 @Coding +University");
            Assert.IsFalse(testRow.IsCompleted, "Row shouldn't be completed.");
            Assert.IsTrue(testRow.HasContexts, "Row should have contexts.");
            Assert.IsTrue(testRow.HasProjects, "Row should have projects.");
            Assert.IsTrue(testRow.HasPriority, "Row should have priority.");

            Assert.AreEqual(testRow.CreationDate, new DateTime(2013, 04, 19), "Row should have correct creation date.");

            testRow = new Row("2013-04-19 due:2013-04-20 +CS3241 Lab 5 @Coding +University");
            Assert.IsFalse(testRow.IsCompleted, "Row shouldn't be completed.");
            Assert.IsTrue(testRow.HasContexts, "Row should have contexts.");
            Assert.IsTrue(testRow.HasProjects, "Row should have projects.");
            Assert.IsFalse(testRow.HasPriority, "Row shouldn't have priority.");

            Assert.AreEqual(testRow.CreationDate, new DateTime(2013, 04, 19), "Row should have correct creation date.");
        }
    }
}
