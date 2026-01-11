using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSEapp;
using System.Reflection;
using System;

namespace BOOSETEST
{
    /// <summary>
    /// Contains integration-style tests that verify loop and conditional behavior
    /// executed through the <see cref="MainForm"/> scripting engine.
    /// </summary>
    [TestClass]
    public class LoopTests
    {
        private MainForm mainForm;
        private System.Reflection.MethodInfo executeProgramMethod;
        private FieldInfo variablesField;

        /// <summary>
        /// Initializes the test fixture by creating a <see cref="MainForm"/> instance
        /// and obtaining reflection handles to the program execution method and variables field.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Create MainForm instance
            mainForm = new MainForm();

            // Get ExecuteProgram method using reflection (it's private)
            executeProgramMethod = typeof(MainForm).GetMethod(
                "ExecuteProgram",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            // Get variables field using reflection
            variablesField = typeof(MainForm).GetField("variables",
                BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// Cleans up resources associated with the <see cref="MainForm"/> after each test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            // Clean up MainForm
            mainForm.Dispose();
        }

        /// <summary>
        /// Verifies that a simple <c>while</c> loop increments a counter variable
        /// until the loop condition becomes false.
        /// </summary>
        [TestMethod]
        public void While_Loop()
        {
            // Program with simple while loop
            string program = @"int count = 0
                               while count < 3
                               count = count + 1
                               end while";

            // Execute the program
            executeProgramMethod.Invoke(mainForm, new object[] { program });

            // Check if count reached 3
            Assert.AreEqual(5, GetVariables()["count"]);
        }

        /// <summary>
        /// Verifies that a <c>while</c> loop with a countdown condition executes
        /// the expected number of iterations and updates dependent variables correctly.
        /// </summary>
        [TestMethod]
        public void While_With_Condition()
        {
            // Program with while loop that counts down
            string program = @"int x = 10
                               int y = 0
                               while x > 0
                               y = y + 2
                               x = x - 1
                               end while";

            // Execute the program
            executeProgramMethod.Invoke(mainForm, new object[] { program });

            // Check if y = 20 (10 iterations * 2)
            Assert.AreEqual(20, GetVariables()["y"]);
        }

        /// <summary>
        /// Verifies that a <c>for</c> loop correctly accumulates a running total over a range of values.
        /// </summary>
        [TestMethod]
        public void For_Loop()
        {
            // Program with for loop to sum numbers
            string program = @"int sum = 0
                               for i = 1 to 5
                               sum = sum + i
                               end for";

            // Execute the program
            executeProgramMethod.Invoke(mainForm, new object[] { program });

            // Check if sum = 15 (1+2+3+4+5)
            Assert.AreEqual(15, GetVariables()["sum"]);
        }

        /// <summary>
        /// Verifies that an <c>if</c>-<c>else</c> statement executes the correct branch
        /// based on the evaluation of its condition.
        /// </summary>
        [TestMethod]
        public void If_Else()
        {
            // Program with if-else statement
            string program = @"int grade = 0
                               int score = 75
                               if score >= 80
                               grade = 1
                               else
                               grade = 2
                               end if";

            // Execute the program
            executeProgramMethod.Invoke(mainForm, new object[] { program });

            // Check if grade = 2 (else branch executed)
            Assert.AreEqual(2, GetVariables()["grade"]);
        }

        /// <summary>
        /// Retrieves the internal variables dictionary from the <see cref="MainForm"/> instance using reflection.
        /// </summary>
        /// <returns>
        /// The dictionary that maps variable names to their current values in the executed program.
        /// </returns>
        private System.Collections.Generic.Dictionary<string, object> GetVariables()
        {
            return variablesField.GetValue(mainForm) as System.Collections.Generic.Dictionary<string, object>;
        }
    }
}