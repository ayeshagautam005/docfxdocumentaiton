using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSEapp;
using System.Reflection;
using System;

namespace BOOSETEST
{
    /// <summary>
    /// Contains tests that verify method declaration, invocation, and return behavior
    /// in the BOOSE scripting engine.
    /// </summary>
    [TestClass]
    public class MethodTest
    {
        private MainForm mainForm;
        private System.Reflection.MethodInfo executeProgramMethod;

        /// <summary>
        /// Initializes a <see cref="MainForm"/> instance and obtains a handle
        /// to its private <c>ExecuteProgram</c> method before each test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Create MainForm instance for testing
            mainForm = new MainForm();

            // Get private ExecuteProgram method using reflection
            executeProgramMethod = typeof(MainForm).GetMethod(
                "ExecuteProgram",
                BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// Disposes the <see cref="MainForm"/> instance after each test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            // Dispose MainForm after test
            mainForm?.Dispose();
        }

        /// <summary>
        /// Verifies that a method taking two parameters can be declared, called,
        /// and that its return value is stored and read correctly.
        /// </summary>
        [TestMethod]
        public void Method_Call()
        {
            // Program with add method that takes two parameters
            string program = @"method int add int x, int y
                               add = x + y
                               end method
                               
                               call add 5, 3
                               int result = add";

            // Execute the program
            executeProgramMethod.Invoke(mainForm, new object[] { program });

            // Get variables and check result (5 + 3 = 8)
            var variables = GetVariables();
            Assert.AreEqual(8, variables["result"]);
        }

        /// <summary>
        /// Verifies that multiple methods can be declared and called independently,
        /// and that each returns the correct value.
        /// </summary>
        [TestMethod]
        public void Multiple_Methods()
        {
            // Program with two methods: square and cube
            string program = @"method int square int n
                               square = n * n
                               end method
                               
                               method int cube int n
                               cube = n * n * n
                               end method
                               
                               call square 4
                               int sq = square
                               
                               call cube 3
                               int cb = cube";

            // Execute the program
            executeProgramMethod.Invoke(mainForm, new object[] { program });

            // Check both results
            var variables = GetVariables();
            Assert.AreEqual(16, variables["sq"]);  // 4² = 16
            Assert.AreEqual(27, variables["cb"]);  // 3³ = 27
        }

        /// <summary>
        /// Verifies that a method using if-else logic returns the maximum of two values.
        /// </summary>
        [TestMethod]
        public void Method_With_Condition()
        {
            // Program with max method that uses if-else
            string program = @"method int max int x, int y
                               if x > y
                               max = x
                               else
                               max = y
                               end if
                               end method
                               
                               call max 10, 20
                               int result = max";

            // Execute the program
            executeProgramMethod.Invoke(mainForm, new object[] { program });

            // Check result (max of 10 and 20 is 20)
            var variables = GetVariables();
            Assert.AreEqual(20, variables["result"]);
        }

        /// <summary>
        /// Verifies that a parameterless method can be declared, called,
        /// and that its return value is retrieved correctly.
        /// </summary>
        [TestMethod]
        public void Parameterless_Method()
        {
            // Program with method that takes no parameters
            string program = @"method int getValue
                               getValue = 42
                               end method
                               
                               call getValue
                               int val = getValue";

            // Execute the program
            executeProgramMethod.Invoke(mainForm, new object[] { program });

            // Check result (should return 42)
            var variables = GetVariables();
            Assert.AreEqual(42, variables["val"]);
        }

        /// <summary>
        /// Retrieves the internal variables dictionary from the <see cref="MainForm"/> instance.
        /// </summary>
        /// <returns>The dictionary that maps variable names to their current values.</returns>
        private System.Collections.Generic.Dictionary<string, object> GetVariables()
        {
            // Get private variables field using reflection
            var variablesField = typeof(MainForm).GetField(
                "variables",
                BindingFlags.NonPublic | BindingFlags.Instance);

            return variablesField.GetValue(mainForm) as System.Collections.Generic.Dictionary<string, object>;
        }
    }
}
