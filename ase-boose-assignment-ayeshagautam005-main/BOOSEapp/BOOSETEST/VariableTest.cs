using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSEapp;
using System.Reflection;
using System;

namespace BOOSETEST
{
    /// <summary>
    /// Contains tests that verify variable and array declaration, storage, and retrieval
    /// in the BOOSE scripting engine.
    /// </summary>
    [TestClass]
    public class VariableTest
    {
        private MainForm mainForm;
        private FieldInfo variablesField;
        private FieldInfo arraysField;

        /// <summary>
        /// Initializes a <see cref="MainForm"/> instance and obtains reflection
        /// access to its internal variables and arrays dictionaries before each test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Create MainForm instance for testing
            mainForm = new MainForm();

            // Get private fields using reflection
            variablesField = typeof(MainForm).GetField(
                "variables",
                BindingFlags.NonPublic | BindingFlags.Instance);
            arraysField = typeof(MainForm).GetField(
                "arrays",
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
        /// Verifies that an integer variable can be declared and initialized with a value.
        /// </summary>
        [TestMethod]
        public void Int_Declaration()
        {
            // Test declaring integer variable with value
            mainForm.ProcessIntDeclaration("int x = 5");

            // Get variables and check if x = 5
            var variables = GetVariables();
            Assert.AreEqual(5, variables["x"]);
        }

        /// <summary>
        /// Verifies that a real (floating‑point) variable can be declared and initialized with a value.
        /// </summary>
        [TestMethod]
        public void Real_Declaration()
        {
            // Test declaring real (decimal) variable
            mainForm.ProcessRealDeclaration("real pi = 3.14");

            // Get variables and check if pi = 3.14
            var variables = GetVariables();
            double value = Convert.ToDouble(variables["pi"]);
            Assert.AreEqual(3.14, value, 0.001);  // Allow small floating-point error
        }

        /// <summary>
        /// Verifies that an integer array can be declared with the requested size.
        /// </summary>
        [TestMethod]
        public void Array_Declaration()
        {
            // Test declaring integer array with size 10
            mainForm.ProcessArrayDeclaration("array int data 10");

            // Get arrays and check if data array has 10 elements
            var arrays = GetArrays();
            Assert.AreEqual(10, arrays["data"].Length);
        }

        /// <summary>
        /// Verifies that a value can be stored in an array element using a POKE command.
        /// </summary>
        [TestMethod]
        public void Array_Poke()
        {
            // First create array
            mainForm.ProcessArrayDeclaration("array int data 10");

            // Test storing value at index 5
            mainForm.ProcessPoke("poke data[5] = 99");

            // Get arrays and check if data[5] = 99
            var arrays = GetArrays();
            Assert.AreEqual(99, arrays["data"][5]);
        }

        /// <summary>
        /// Verifies that a value can be read from an array element into a variable using a PEEK command.
        /// </summary>
        [TestMethod]
        public void Array_Peek()
        {
            // Setup: create array and store value
            mainForm.ProcessArrayDeclaration("array int data 10");
            mainForm.ProcessPoke("poke data[3] = 42");

            // Test reading value from array into variable
            mainForm.ProcessPeek("peek val = data[3]");

            // Get variables and check if val = 42
            var variables = GetVariables();
            Assert.AreEqual(42, variables["val"]);
        }

        /// <summary>
        /// Retrieves the internal variables dictionary from the <see cref="MainForm"/> instance.
        /// </summary>
        /// <returns>The dictionary that maps variable names to their current values.</returns>
        private System.Collections.Generic.Dictionary<string, object> GetVariables()
        {
            return variablesField.GetValue(mainForm) as System.Collections.Generic.Dictionary<string, object>;
        }

        /// <summary>
        /// Retrieves the internal arrays dictionary from the <see cref="MainForm"/> instance.
        /// </summary>
        /// <returns>The dictionary that maps array names to their backing value arrays.</returns>
        private System.Collections.Generic.Dictionary<string, object[]> GetArrays()
        {
            return arraysField.GetValue(mainForm) as System.Collections.Generic.Dictionary<string, object[]>;
        }
    }
}
