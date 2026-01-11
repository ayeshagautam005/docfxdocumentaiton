using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a method definition, including its name, parameters, body, and return variable.
    /// </summary>
    public class MethodInfo
    {
        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of parameter names for the method.
        /// </summary>
        public List<string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the list of command lines that form the method body.
        /// </summary>
        public List<string> Body { get; set; }

        /// <summary>
        /// Gets or sets the name of the return variable, which should match the method name.
        /// </summary>
        public string ReturnVariable { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInfo"/> class with empty parameter and body lists.
        /// </summary>
        public MethodInfo()
        {
            Parameters = new List<string>();
            Body = new List<string>();
        }
    }


    /// <summary>
    /// Main form for the BOOSE Drawing Application.
    /// This form provides the user interface for entering drawing commands,
    /// executing them, and viewing the results on a drawing canvas.
    /// </summary>
    public partial class MainForm : Form
    {
        private AppCanvas canvas;
        private PictureBox pictureBox;

        /// <summary>
        /// Stores simple variables such as integers and real numbers declared in programs.
        /// Each entry maps a variable name to its current value.
        /// </summary>
        private Dictionary<string, object> variables = new Dictionary<string, object>();

        /// <summary>
        /// Stores array variables declared in programs, indexed by array name.
        /// Each array can contain multiple values accessed by index.
        /// </summary>
        private Dictionary<string, object[]> arrays = new Dictionary<string, object[]>();

        /// <summary>
        /// Stores the element type for each declared array to ensure correct type handling
        /// when reading from or writing to the array.
        /// </summary>
        private Dictionary<string, Type> arrayTypes = new Dictionary<string, Type>();

        /// <summary>
        /// Stores method definitions, indexed by method name.
        /// Each method includes its parameter list and body lines.
        /// </summary>
        private Dictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// Maintains a stack of variable scopes used during method execution,
        /// where each pushed dictionary represents a new method call scope.
        /// </summary>
        private Stack<Dictionary<string, object>> methodVariableScopes = new Stack<Dictionary<string, object>>();

        /// <summary>
        /// Gets or sets the last value returned from a method call.
        /// Methods assign their return value to a variable with the same name as the method.
        /// </summary>
        private object lastMethodReturnValue = null;

        /// <summary>
        /// Indicates whether the parser is currently collecting method body lines
        /// instead of executing them immediately.
        /// </summary>
        private bool collectingMethods = false;

        /// <summary>
        /// Stores the body lines of the method that is currently being parsed.
        /// Lines are added until the corresponding <c>end method</c> marker is encountered.
        /// </summary>
        private List<string> currentMethodBody = new List<string>();

        /// <summary>
        /// Stores the name of the method that is currently being parsed.
        /// </summary>
        private string currentMethodName = "";


        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class,
        /// sets up the drawing canvas, and displays initial library information.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitializeCanvas();
            DisplayAboutInfo();
            DemonstrateDesignPatterns();
        }

        /// <summary>
        /// Configures the drawing canvas using the current size of the canvas panel,
        /// creates a <see cref="PictureBox"/> to host the canvas bitmap, and adds it to the panel.
        /// </summary>
        private void InitializeCanvas()
        {
            canvas = new AppCanvas(canvasPanel.Width, canvasPanel.Height);
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            canvasPanel.Controls.Add(pictureBox);
            RefreshCanvas();
        }

        /// <summary>
        /// Displays information about the BOOSE drawing library in the output text box
        /// and writes the same information to the debug output for diagnostics.
        /// </summary>
        private void DisplayAboutInfo()
        {
            try
            {
                string aboutInfo = "BOOSE Drawing Library v1.0\n";

                outputTextBox.AppendText("=== BOOSE Library Information ===\r\n");
                outputTextBox.AppendText(aboutInfo + "\r\n");
                outputTextBox.AppendText("=================================\r\n\r\n");

                Debug.WriteLine(aboutInfo);
            }
            catch (Exception ex)
            {
                outputTextBox.AppendText($"Error getting library info: {ex.Message}\r\n");
                Debug.WriteLine($"Error getting library info: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the picture box to show the current state of the drawing canvas.
        /// This method is called after any drawing operation to refresh the display.
        /// </summary>
        private void RefreshCanvas()
        {
            if (canvas != null && pictureBox != null)
            {
                try
                {
                    var bitmap = canvas.getBitmap();
                    if (bitmap != null)
                    {
                        if (pictureBox.Image != null)
                        {
                            pictureBox.Image.Dispose();
                        }
                        pictureBox.Image = bitmap;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error refreshing canvas: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Handles clicks on the Execute button to run the drawing program.
        /// Reads the program text, executes it, and shows any errors that occur.
        /// </summary>
        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            try
            {
                string programText = programTextBox.Text;
                outputTextBox.AppendText($"Executing program...\r\n");

                ExecuteProgram(programText);

                outputTextBox.AppendText($"Program executed successfully\r\n\r\n");
                RefreshCanvas();
            }
            catch (CanvasException ex)
            {
                outputTextBox.AppendText($"Error: {ex.Message}\r\n\r\n");
                Debug.WriteLine($"Canvas Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                outputTextBox.AppendText($"Unexpected error: {ex.Message}\r\n\r\n");
                Debug.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Runs a drawing program by processing each line of text.
        /// Clears all previous variables and arrays before starting the new program.
        /// </summary>
        private void ExecuteProgram(string programText)
        {
            variables.Clear();
            arrays.Clear();
            arrayTypes.Clear();
            methods.Clear();
            methodVariableScopes.Clear();
            lastMethodReturnValue = null;
            collectingMethods = false;
            currentMethodBody.Clear();
            currentMethodName = "";

            canvas.Clear();
            RefreshCanvas();

            AppendOutput($"[DEBUG] Starting program execution\r\n");

            string[] lines = programText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                string trimmedLine = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("*"))
                    continue;

                AppendOutput($"[LINE {i}] Processing: '{trimmedLine}'\r\n");

                if (trimmedLine.StartsWith("method "))
                {
                    i = ProcessMethodDefinition(lines, i);
                    continue;
                }

                if (trimmedLine.StartsWith("while "))
                {
                    i = ProcessWhileBlock(lines, i) - 1;
                    continue;
                }

                if (trimmedLine.StartsWith("for "))
                {
                    i = ProcessForBlock(lines, i) - 1;
                    continue;
                }

                if (trimmedLine.StartsWith("if "))
                {
                    i = ProcessIfBlock(lines, i) - 1;
                    continue;
                }

                if (trimmedLine.StartsWith("call "))
                {
                    ProcessMethodCall(trimmedLine);
                    continue;
                }

                ProcessCommand(trimmedLine);
            }
        }

        /// <summary>
        /// Processes a method definition starting at the specified index in the program lines.
        /// Parses the method signature and collects its body until the corresponding <c>end method</c> line.
        /// </summary>
        /// <param name="lines">All program lines being executed.</param>
        /// <param name="startIndex">The index of the line that contains the method declaration.</param>
        /// <returns>
        /// The index of the last line that belongs to the method definition
        /// (typically the line that contains <c>end method</c>).
        /// </returns>
        private int ProcessMethodDefinition(string[] lines, int startIndex)
        {
            try
            {
                string methodLine = lines[startIndex].Trim();
                AppendOutput($"[METHOD DEF] Found: {methodLine}\r\n");

                // Parse method signature: "method int mulMethod int one, int two"
                string[] parts = methodLine.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 4)
                {
                    AppendOutput($"[ERROR] Invalid method definition\r\n");
                    return startIndex;
                }

                MethodInfo method = new MethodInfo();
                method.Name = parts[2];
                method.ReturnVariable = parts[2];

                // Parse parameters
                for (int i = 3; i < parts.Length; i++)
                {
                    if (parts[i] == "int" || parts[i] == "real")
                    {
                        if (i + 1 < parts.Length)
                        {
                            method.Parameters.Add(parts[i + 1]);
                            i++;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(parts[i]))
                    {
                        method.Parameters.Add(parts[i]);
                    }
                }

                // Collect method body
                int iLine = startIndex + 1;
                bool foundEndMethod = false;

                while (iLine < lines.Length && !foundEndMethod)
                {
                    string line = lines[iLine].Trim();

                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("*"))
                    {
                        iLine++;
                        continue;
                    }

                    if (line == "end method")
                    {
                        foundEndMethod = true;
                    }
                    else
                    {
                        method.Body.Add(line);
                    }

                    iLine++;
                }

                if (!foundEndMethod)
                {
                    AppendOutput($"[ERROR] Missing 'end method' for method {method.Name}\r\n");
                    return iLine;
                }

                methods[method.Name] = method;
                AppendOutput($"[METHOD DEF] Registered method '{method.Name}' with {method.Parameters.Count} parameters and {method.Body.Count} body lines\r\n");

                return iLine - 1;
            }
            catch (Exception ex)
            {
                AppendOutput($"[ERROR] Method definition error: {ex.Message}\r\n");
                return startIndex + 1;
            }
        }

        /// <summary>
        /// Processes a WHILE loop block starting at the specified index.
        /// Collects all lines between the WHILE statement and the matching <c>end while</c> and executes the loop.
        /// </summary>
        /// <param name="lines">All program lines being executed.</param>
        /// <param name="startIndex">The index of the line that contains the WHILE statement.</param>
        /// <returns>
        /// The index of the last line that belongs to the WHILE block
        /// (typically the line that contains <c>end while</c>).
        /// </returns>
        private int ProcessWhileBlock(string[] lines, int startIndex)
        {
            try
            {
                string whileLine = lines[startIndex].Trim();
                string condition = whileLine.Substring(5).Trim();

                AppendOutput($"[WHILE BLOCK] Found while: {condition}\r\n");

                List<string> loopBody = new List<string>();
                int i = startIndex + 1;
                bool foundEnd = false;

                while (i < lines.Length && !foundEnd)
                {
                    string line = lines[i].Trim();

                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("*"))
                    {
                        i++;
                        continue;
                    }

                    if (line == "end while" || line == "endwhile")
                    {
                        foundEnd = true;
                    }
                    else
                    {
                        loopBody.Add(line);
                    }

                    i++;
                }

                if (!foundEnd)
                {
                    AppendOutput($"[ERROR] Missing 'end while' for while loop\r\n");
                    return i;
                }

                AppendOutput($"[WHILE BLOCK] Loop body has {loopBody.Count} commands\r\n");

                ExecuteWhileLoop(condition, loopBody);

                return i - 1;
            }
            catch (Exception ex)
            {
                AppendOutput($"[ERROR] While block error: {ex.Message}\r\n");
                return startIndex + 1;
            }
        }

        /// <summary>
        /// Executes a WHILE loop using the specified condition and list of commands as the loop body.
        /// Supports IF statements and other nested control structures within the loop body.
        /// </summary>
        /// <param name="condition">The loop condition expression to evaluate on each iteration.</param>
        /// <param name="loopCommands">The list of commands that form the loop body.</param>
        private void ExecuteWhileLoop(string condition, List<string> loopCommands)
        {
            int iteration = 0;
            const int maxIterations = 1000;

            while (EvaluateCondition(condition) && iteration < maxIterations)
            {
                iteration++;
                AppendOutput($"[WHILE LOOP] Iteration {iteration}, condition true\r\n");

                // Process commands, handling if statements specially
                for (int i = 0; i < loopCommands.Count; i++)
                {
                    string command = loopCommands[i].Trim();

                    if (command.StartsWith("if "))
                    {
                        // Found an if statement - process it
                        i = ProcessIfInLoopBody(loopCommands, i);
                    }
                    else
                    {
                        // Process normal command
                        ProcessCommand(command);
                    }
                }
            }

            if (iteration >= maxIterations)
            {
                AppendOutput($"[WARNING] Loop stopped after {maxIterations} iterations\r\n");
            }

            AppendOutput($"[WHILE LOOP] Finished after {iteration} iterations\r\n");
        }

        /// <summary>
        /// Processes an IF–ELSE block starting at the specified index.
        /// Collects the IF and optional ELSE bodies, supports nested IF statements, and executes the appropriate branch.
        /// </summary>
        /// <param name="lines">All program lines being executed.</param>
        /// <param name="startIndex">The index of the line that contains the IF statement.</param>
        /// <returns>
        /// The index of the last line that belongs to the IF block
        /// (typically the line that contains <c>end if</c>).
        /// </returns>
        private int ProcessIfBlock(string[] lines, int startIndex)
        {
            try
            {
                string ifLine = lines[startIndex].Trim();
                string condition = ifLine.Substring(3).Trim();

                AppendOutput($"[IF BLOCK] Found: {ifLine}\r\n");

                List<string> ifBody = new List<string>();
                List<string> elseBody = new List<string>();
                int i = startIndex + 1;
                bool foundElse = false;
                bool foundEndIf = false;
                int nestedIfCount = 0;

                // Collect all lines until we find the matching "end if"
                while (i < lines.Length && !foundEndIf)
                {
                    string line = lines[i].Trim();

                    // Skip empty lines and comments
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("*"))
                    {
                        i++;
                        continue;
                    }

                    // Count nested IF statements
                    if (line.StartsWith("if "))
                    {
                        nestedIfCount++;
                    }

                    // Check for "end if"
                    if (line == "end if" || line == "endif")
                    {
                        if (nestedIfCount == 0)
                        {
                            foundEndIf = true;
                        }
                        else
                        {
                            nestedIfCount--;
                            // This "end if" belongs to a nested IF, so add it to the body
                            if (foundElse)
                            {
                                elseBody.Add(line);
                            }
                            else
                            {
                                ifBody.Add(line);
                            }
                        }
                    }
                    // Check for "else" (only at current nesting level)
                    else if (line == "else" && nestedIfCount == 0 && !foundElse)
                    {
                        foundElse = true;
                    }
                    // Add lines to appropriate body
                    else
                    {
                        if (foundElse)
                        {
                            elseBody.Add(line);
                        }
                        else
                        {
                            ifBody.Add(line);
                        }
                    }

                    i++;
                }

                if (!foundEndIf)
                {
                    AppendOutput($"[ERROR] Missing 'end if' for IF statement\r\n");
                    return i;
                }

                AppendOutput($"[IF BLOCK] IF body has {ifBody.Count} commands\r\n");
                AppendOutput($"[IF BLOCK] ELSE body has {elseBody.Count} commands\r\n");

                // Evaluate the condition
                bool conditionResult = EvaluateCondition(condition);

                if (conditionResult)
                {
                    AppendOutput($"[IF BLOCK] Condition TRUE, executing IF body\r\n");
                    // Process IF body commands
                    ProcessCommandListWithNestedControl(ifBody);
                }
                else if (elseBody.Count > 0)
                {
                    AppendOutput($"[IF BLOCK] Condition FALSE, executing ELSE body\r\n");
                    // Process ELSE body commands
                    ProcessCommandListWithNestedControl(elseBody);
                }
                else
                {
                    AppendOutput($"[IF BLOCK] Condition FALSE, no ELSE body\r\n");
                }

                return i - 1;
            }
            catch (Exception ex)
            {
                AppendOutput($"[ERROR] IF block error: {ex.Message}\r\n");
                return startIndex + 1;
            }
        }

        /// <summary>
        /// Processes a list of commands, handling nested control structures such as IF, WHILE, and FOR.
        /// </summary>
        /// <param name="commands">The list of command strings to process.</param>
        private void ProcessCommandListWithNestedControl(List<string> commands)
        {
            for (int j = 0; j < commands.Count; j++)
            {
                string command = commands[j].Trim();

                if (command.StartsWith("if "))
                {
                    // Convert to array for ProcessIfBlock
                    string[] commandArray = commands.ToArray();
                    j = ProcessIfBlock(commandArray, j) - 1; // -1 because loop will increment
                }
                else if (command.StartsWith("while "))
                {
                    // Convert to array for ProcessWhileBlock
                    string[] commandArray = commands.ToArray();
                    j = ProcessWhileBlock(commandArray, j) - 1;
                }
                else if (command.StartsWith("for "))
                {
                    // Convert to array for ProcessForBlock
                    string[] commandArray = commands.ToArray();
                    j = ProcessForBlock(commandArray, j) - 1;
                }
                else
                {
                    // Process normal command
                    ProcessCommand(command);
                }
            }
        }

        /// <summary>
        /// Processes an IF statement that appears inside a loop body.
        /// Supports nested IF statements and optional ELSE blocks within the loop.
        /// </summary>
        /// <param name="loopCommands">The list of commands that make up the loop body.</param>
        /// <param name="ifIndex">The index of the IF statement within the loop body list.</param>
        /// <returns>
        /// The index of the last command that belongs to the IF block inside the loop.
        /// </returns>
        private int ProcessIfInLoopBody(List<string> loopCommands, int ifIndex)
        {
            try
            {
                string ifLine = loopCommands[ifIndex].Trim();
                string condition = ifLine.Substring(3).Trim();

                AppendOutput($"[LOOP IF] Processing if statement: {ifLine}\r\n");

                List<string> ifBody = new List<string>();
                List<string> elseBody = new List<string>();
                int i = ifIndex + 1;
                bool foundElse = false;
                bool foundEndIf = false;
                int nestedIfCount = 0;

                while (i < loopCommands.Count && !foundEndIf)
                {
                    string line = loopCommands[i].Trim();

                    // Count nested IF statements
                    if (line.StartsWith("if "))
                    {
                        nestedIfCount++;
                    }

                    // Check for "end if"
                    if (line == "end if" || line == "endif")
                    {
                        if (nestedIfCount == 0)
                        {
                            foundEndIf = true;
                        }
                        else
                        {
                            nestedIfCount--;
                            if (foundElse)
                            {
                                elseBody.Add(line);
                            }
                            else
                            {
                                ifBody.Add(line);
                            }
                        }
                    }
                    // Check for "else" 
                    else if (line == "else" && nestedIfCount == 0 && !foundElse)
                    {
                        foundElse = true;
                    }
                    else
                    {
                        if (foundElse)
                        {
                            elseBody.Add(line);
                        }
                        else
                        {
                            ifBody.Add(line);
                        }
                    }
                    i++;
                }

                if (!foundEndIf)
                {
                    AppendOutput($"[ERROR] Missing 'end if' for if statement inside loop\r\n");
                    return i;
                }

                AppendOutput($"[LOOP IF] If body has {ifBody.Count} commands, Else body has {elseBody.Count} commands\r\n");

                // Evaluate the condition
                bool conditionResult = EvaluateCondition(condition);

                if (conditionResult)
                {
                    AppendOutput($"[LOOP IF] Condition TRUE, executing if body\r\n");
                    // Process IF body with support for nested control structures
                    ProcessCommandListWithNestedControl(ifBody);
                }
                else if (elseBody.Count > 0)
                {
                    AppendOutput($"[LOOP IF] Condition FALSE, executing else body\r\n");
                    // Process ELSE body with support for nested control structures
                    ProcessCommandListWithNestedControl(elseBody);
                }
                else
                {
                    AppendOutput($"[LOOP IF] Condition FALSE, no else body\r\n");
                }

                // Return the index after the end if
                return i - 1;
            }
            catch (Exception ex)
            {
                AppendOutput($"[ERROR] Error processing if in loop: {ex.Message}\r\n");
                return ifIndex + 1;
            }
        }

        /// <summary>
        /// Evaluates a conditional expression used by IF and WHILE statements.
        /// Supports comparison operators such as <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>, <c>==</c>, and <c>!=</c>,
        /// as well as numeric expressions that evaluate to nonzero for <see langword="true"/> and zero for <see langword="false"/>.
        /// </summary>
        /// <param name="condition">The condition expression to evaluate.</param>
        /// <returns><see langword="true"/> if the condition evaluates to true; otherwise, <see langword="false"/>.</returns>
        private bool EvaluateCondition(string condition)
        {
            try
            {
                string replacedCondition = ReplaceVariables(condition);

                if (replacedCondition.Contains(">="))
                {
                    string[] parts = replacedCondition.Split(new[] { ">=" }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        double left = ParseNumber(parts[0].Trim());
                        double right = ParseNumber(parts[1].Trim());
                        return left >= right;
                    }
                }
                else if (replacedCondition.Contains("<="))
                {
                    string[] parts = replacedCondition.Split(new[] { "<=" }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        double left = ParseNumber(parts[0].Trim());
                        double right = ParseNumber(parts[1].Trim());
                        return left <= right;
                    }
                }
                else if (replacedCondition.Contains(">"))
                {
                    string[] parts = replacedCondition.Split('>');
                    if (parts.Length == 2)
                    {
                        double left = ParseNumber(parts[0].Trim());
                        double right = ParseNumber(parts[1].Trim());
                        return left > right;
                    }
                }
                else if (replacedCondition.Contains("<"))
                {
                    string[] parts = replacedCondition.Split('<');
                    if (parts.Length == 2)
                    {
                        double left = ParseNumber(parts[0].Trim());
                        double right = ParseNumber(parts[1].Trim());
                        return left < right;
                    }
                }
                else if (replacedCondition.Contains("=="))
                {
                    string[] parts = replacedCondition.Split(new[] { "==" }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        double left = ParseNumber(parts[0].Trim());
                        double right = ParseNumber(parts[1].Trim());
                        return Math.Abs(left - right) < 0.000001;
                    }
                }
                else if (replacedCondition.Contains("!="))
                {
                    string[] parts = replacedCondition.Split(new[] { "!=" }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        double left = ParseNumber(parts[0].Trim());
                        double right = ParseNumber(parts[1].Trim());
                        return Math.Abs(left - right) > 0.000001;
                    }
                }

                object result = EvaluateExpression(condition);
                if (result is int intVal)
                    return intVal != 0;
                if (result is double doubleVal)
                    return Math.Abs(doubleVal) > 0.000001;

                return false;
            }
            catch (Exception ex)
            {
                AppendOutput($"[CONDITION ERROR] '{condition}': {ex.Message}\r\n");
                return false;
            }
        }

        /// <summary>
        /// Parses a numeric value from the specified text.
        /// Attempts direct numeric parsing first, then evaluates the text as an expression if needed.
        /// </summary>
        /// <param name="text">The text that represents a numeric value or expression.</param>
        /// <returns>The parsed numeric value as a <see cref="double"/>.</returns>
        private double ParseNumber(string text)
        {
            text = text.Trim();

            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                return result;

            object evalResult = EvaluateExpression(text);
            if (evalResult is int intVal)
                return (double)intVal;
            if (evalResult is double dblVal)
                return dblVal;

            return 0.0;
        }

        /// <summary>
        /// Processes a FOR loop block starting at the specified index.
        /// Collects all lines between the FOR statement and the matching <c>end for</c> and executes the loop.
        /// </summary>
        /// <param name="lines">All program lines being executed.</param>
        /// <param name="startIndex">The index of the line that contains the FOR statement.</param>
        /// <returns>
        /// The index of the last line that belongs to the FOR block
        /// (typically the line that contains <c>end for</c>).
        /// </returns>
        private int ProcessForBlock(string[] lines, int startIndex)
        {
            try
            {
                string forLine = lines[startIndex].Trim();
                AppendOutput($"[FOR BLOCK] Found: {forLine}\r\n");

                List<string> loopBody = new List<string>();
                int i = startIndex + 1;
                bool foundEnd = false;

                while (i < lines.Length && !foundEnd)
                {
                    string line = lines[i].Trim();

                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("*"))
                    {
                        i++;
                        continue;
                    }

                    if (line == "end for" || line == "endfor")
                    {
                        foundEnd = true;
                    }
                    else
                    {
                        loopBody.Add(line);
                    }

                    i++;
                }

                if (!foundEnd)
                {
                    AppendOutput($"[ERROR] Missing 'end for' for FOR loop\r\n");
                    return i;
                }

                AppendOutput($"[FOR BLOCK] Loop body has {loopBody.Count} commands\r\n");

                ExecuteForLoop(forLine, loopBody);

                return i - 1;
            }
            catch (Exception ex)
            {
                AppendOutput($"[ERROR] FOR block error: {ex.Message}\r\n");
                return startIndex + 1;
            }
        }

        /// <summary>
        /// Executes a FOR loop with the given parameters and loop body.
        /// The FOR statement format is: <c>for variable = start to end step increment</c>.
        /// Supports both positive and negative step values and stops after a safety limit of iterations.
        /// </summary>
        /// <param name="forStatement">The original FOR statement line to parse.</param>
        /// <param name="loopCommands">The list of commands that form the body of the FOR loop.</param>
        private void ExecuteForLoop(string forStatement, List<string> loopCommands)
        {
            try
            {
                string withoutFor = forStatement.Substring(3).Trim();
                string[] parts = withoutFor.Split(new[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 4)
                {
                    AppendOutput($"[FOR ERROR] Invalid FOR statement: {forStatement}\r\n");
                    return;
                }

                string varName = parts[0].Trim();

                int toIndex = -1;
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].ToLower() == "to")
                    {
                        toIndex = i;
                        break;
                    }
                }

                if (toIndex == -1)
                {
                    AppendOutput($"[FOR ERROR] Missing 'to' keyword in FOR statement: {forStatement}\r\n");
                    return;
                }

                string startExpr = "";
                for (int i = 1; i < toIndex; i++)
                {
                    startExpr += parts[i] + " ";
                }
                startExpr = startExpr.Trim();

                object startObj = EvaluateExpression(startExpr);
                double startValue = ConvertToDouble(startObj);

                string endExpr = "";
                for (int i = toIndex + 1; i < parts.Length; i++)
                {
                    if (parts[i].ToLower() != "step")
                    {
                        endExpr += parts[i] + " ";
                    }
                    else
                    {
                        break;
                    }
                }
                endExpr = endExpr.Trim();

                object endObj = EvaluateExpression(endExpr);
                double endValue = ConvertToDouble(endObj);

                double stepValue = 1.0;
                int stepIndex = -1;
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].ToLower() == "step")
                    {
                        stepIndex = i;
                        break;
                    }
                }

                if (stepIndex != -1 && stepIndex + 1 < parts.Length)
                {
                    string stepExpr = parts[stepIndex + 1];
                    object stepObj = EvaluateExpression(stepExpr);
                    stepValue = ConvertToDouble(stepObj);
                }

                AppendOutput($"[FOR LOOP] Parsed: {varName} = {startValue} to {endValue} step {stepValue}\r\n");

                if (stepValue == 0)
                {
                    AppendOutput($"[FOR ERROR] Step value cannot be zero\r\n");
                    return;
                }

                int iteration = 0;
                const int maxIterations = 1000;

                for (double counter = startValue;
                     (stepValue > 0 && counter <= endValue) || (stepValue < 0 && counter >= endValue);
                     counter += stepValue)
                {
                    iteration++;

                    if (iteration > maxIterations)
                    {
                        AppendOutput($"[FOR WARNING] Loop stopped after {maxIterations} iterations\r\n");
                        break;
                    }

                    variables[varName] = counter;
                    AppendOutput($"[FOR LOOP] Iteration {iteration}: {varName} = {counter}\r\n");

                    foreach (string command in loopCommands)
                    {
                        ProcessCommand(command);
                    }
                }

                AppendOutput($"[FOR LOOP] Finished after {iteration} iterations\r\n");

                variables.Remove(varName);
            }
            catch (Exception ex)
            {
                AppendOutput($"[FOR ERROR] Error executing FOR loop: {ex.Message}\r\n");
            }
        }

        /// <summary>
        /// Converts the specified object to a <see cref="double"/> value.
        /// Handles both <see cref="int"/> and <see cref="double"/> instances; returns 0.0 for unsupported types.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The numeric value represented by <paramref name="obj"/> as a <see cref="double"/>.</returns>
        private double ConvertToDouble(object obj)
        {
            if (obj is int intValue)
            {
                return (double)intValue;
            }
            else if (obj is double doubleValue)
            {
                return doubleValue;
            }
            else
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Processes a single command line from the drawing program and executes the corresponding action.
        /// Handles declarations, assignments, array operations, control keywords, and drawing commands.
        /// </summary>
        /// <param name="commandLine">The command line text to process.</param>
        private void ProcessCommand(string commandLine)
        {
            commandLine = commandLine.Trim();
            if (string.IsNullOrEmpty(commandLine)) return;

            outputTextBox.AppendText($"[PROCESS] '{commandLine}'\r\n");

            if (commandLine == "end if" || commandLine == "endif" ||
                commandLine == "end while" || commandLine == "endwhile" ||
                commandLine == "end for" || commandLine == "endfor" ||
                commandLine == "end method")
                return;

            if (commandLine.StartsWith("int "))
            {
                ProcessIntDeclaration(commandLine);
                return;
            }

            if (commandLine.StartsWith("real "))
            {
                ProcessRealDeclaration(commandLine);
                return;
            }

            if (commandLine.StartsWith("array "))
            {
                ProcessArrayDeclaration(commandLine);
                return;
            }

            if (commandLine.StartsWith("peek "))
            {
                ProcessPeek(commandLine);
                return;
            }

            if (commandLine.StartsWith("poke "))
            {
                ProcessPoke(commandLine);
                return;
            }

            if (commandLine.Contains("="))
            {
                ProcessAssignment(commandLine);
                return;
            }

            if (commandLine.StartsWith("write "))
            {
                ProcessWriteCommand(commandLine);
                return;
            }

            string processedCommand = ReplaceVariables(commandLine);

            processedCommand = processedCommand.Replace(",", " ");
            processedCommand = Regex.Replace(processedCommand, @"\s+", " ");

            string[] parts = processedCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            string commandName = parts[0].ToLower();
            List<string> evaluatedParts = new List<string> { commandName };

            for (int i = 1; i < parts.Length; i++)
            {
                string part = parts[i];

                if (part.Contains("*") || part.Contains("/") || part.Contains("+") || part.Contains("-"))
                {
                    try
                    {
                        object result = EvaluateExpression(part);
                        string resultStr = FormatNumber(result);
                        evaluatedParts.Add(resultStr);
                        outputTextBox.AppendText($"[EXPR] '{part}' -> '{resultStr}'\r\n");
                    }
                    catch (Exception ex)
                    {
                        outputTextBox.AppendText($"[EXPR ERROR] Failed to evaluate '{part}': {ex.Message}\r\n");
                        evaluatedParts.Add(part);
                    }
                }
                else
                {
                    evaluatedParts.Add(part);
                }
            }

            processedCommand = string.Join(" ", evaluatedParts);
            outputTextBox.AppendText($"[READY] Command for factory: '{processedCommand}'\r\n");

            AppCommandFactory factory = AppCommandFactory.Instance;
            ICommand drawingCommand = factory.CreateCommand(processedCommand);

            if (drawingCommand != null)
            {
                drawingCommand.Execute(canvas);
                RefreshCanvas();
            }
            else
            {
                outputTextBox.AppendText($"[ERROR] Unknown command: '{commandLine}'\r\n");
            }
        }

        /// <summary>
        /// Formats a numeric value for use in commands using invariant culture and without thousands separators.
        /// </summary>
        /// <param name="number">The numeric value to format.</param>
        /// <returns>
        /// A string representation of <paramref name="number"/> suitable for use in command text;
        /// returns <c>"0"</c> if the value is not numeric.
        /// </returns>
        private string FormatNumber(object number)
        {
            if (number is int intVal)
            {
                return intVal.ToString(CultureInfo.InvariantCulture);
            }
            else if (number is double dblVal)
            {
                return dblVal.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// Handles integer variable declarations, optionally including an initializer expression.
        /// Supports declarations such as <c>int x</c> and <c>int x = 5 + y</c>.
        /// </summary>
        /// <param name="commandLine">The full integer declaration command line.</param>
        public void ProcessIntDeclaration(string commandLine)
        {
            string declaration = commandLine.Substring(4).Trim();

            outputTextBox.AppendText($"[INT DECL] Parsing declaration: '{declaration}'\r\n");

            string varName = "";
            int value = 0;
            bool hasInitialValue = false;

            if (declaration.Contains("="))
            {
                string[] parts = declaration.Split('=');

                if (parts.Length >= 2)
                {
                    varName = parts[0].Trim();
                    string valueExpression = parts[1].Trim();

                    if (int.TryParse(valueExpression, out int parsedValue))
                    {
                        value = parsedValue;
                        hasInitialValue = true;
                    }
                    else
                    {
                        try
                        {
                            object result = EvaluateExpression(valueExpression);

                            if (result is int intResult)
                            {
                                value = intResult;
                                hasInitialValue = true;
                            }
                            else if (result is double dblResult && Math.Abs(dblResult - Math.Round(dblResult)) < 0.000001)
                            {
                                value = (int)Math.Round(dblResult);
                                hasInitialValue = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            outputTextBox.AppendText($"[INT DECL] Error evaluating expression: {ex.Message}\r\n");
                        }
                    }
                }
            }
            else
            {
                varName = declaration.Trim();
                value = 0;
                hasInitialValue = false;
            }

            if (!string.IsNullOrWhiteSpace(varName))
            {
                variables[varName] = value;
                outputTextBox.AppendText($"int {varName} = {value}\r\n");
            }
        }

        /// <summary>
        /// Handles real (floating-point) variable declarations, optionally including an initializer expression.
        /// Supports declarations such as <c>real r</c> and <c>real r = a / 2.5</c>.
        /// </summary>
        /// <param name="commandLine">The full real number declaration command line.</param>
        public void ProcessRealDeclaration(string commandLine)
        {
            string declaration = commandLine.Substring(5).Trim();

            outputTextBox.AppendText($"[REAL DECL] Parsing: '{declaration}'\r\n");

            string varName = "";
            double value = 0.0;
            bool hasInitialValue = false;

            if (declaration.Contains("="))
            {
                string[] parts = declaration.Split('=');

                if (parts.Length >= 2)
                {
                    varName = parts[0].Trim();
                    string valueExpression = parts[1].Trim();

                    if (double.TryParse(valueExpression, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedValue))
                    {
                        value = parsedValue;
                        hasInitialValue = true;
                    }
                    else
                    {
                        try
                        {
                            object result = EvaluateExpression(valueExpression);

                            if (result is double dblResult)
                            {
                                value = dblResult;
                                hasInitialValue = true;
                            }
                            else if (result is int intResult)
                            {
                                value = (double)intResult;
                                hasInitialValue = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            outputTextBox.AppendText($"[REAL DECL] Error evaluating expression: {ex.Message}\r\n");
                        }
                    }
                }
            }
            else
            {
                varName = declaration.Trim();
                value = 0.0;
                hasInitialValue = false;
            }

            if (!string.IsNullOrWhiteSpace(varName))
            {
                variables[varName] = value;
                outputTextBox.AppendText($"real {varName} = {value}\r\n");
            }
        }

        /// <summary>
        /// Processes array declarations in the format <c>array type name size</c>.
        /// Creates a new array with the specified element type, name, and size, initialized with default values.
        /// Supported element types are <c>int</c> and <c>real</c>.
        /// </summary>
        /// <param name="commandLine">The array declaration command line.</param>
        /// <example>
        /// array int nums 10    // Creates an integer array named nums with 10 elements
        /// array real prices 5  // Creates a real number array named prices with 5 elements
        /// </example>
        public void ProcessArrayDeclaration(string commandLine)
        {
            try
            {
                string rest = commandLine.Substring(6).Trim();
                outputTextBox.AppendText($"[ARRAY DECL] Parsing: '{rest}'\r\n");

                string[] parts = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 3)
                {
                    string type = parts[0].ToLower();
                    string arrayName = parts[1];
                    string sizeStr = parts[2];

                    if (int.TryParse(sizeStr, out int size))
                    {
                        if (size <= 0)
                        {
                            outputTextBox.AppendText($"[ARRAY DECL] Error: Array size must be positive\r\n");
                            return;
                        }

                        object[] array = new object[size];

                        object defaultValue;
                        Type elementType;

                        if (type == "int")
                        {
                            defaultValue = 0;
                            elementType = typeof(int);
                        }
                        else if (type == "real")
                        {
                            defaultValue = 0.0;
                            elementType = typeof(double);
                        }
                        else
                        {
                            outputTextBox.AppendText($"[ARRAY DECL] Error: Invalid type '{type}'. Must be 'int' or 'real'\r\n");
                            return;
                        }

                        for (int i = 0; i < size; i++)
                        {
                            array[i] = defaultValue;
                        }

                        arrays[arrayName] = array;
                        arrayTypes[arrayName] = elementType;

                        outputTextBox.AppendText($"Array {type} {arrayName}[{size}] created\r\n");
                    }
                    else
                    {
                        outputTextBox.AppendText($"[ARRAY DECL] Error: Invalid array size '{sizeStr}'\r\n");
                    }
                }
                else
                {
                    outputTextBox.AppendText($"[ARRAY DECL] Error: Invalid format. Expected 'array type name size'\r\n");
                }
            }
            catch (Exception ex)
            {
                AppendOutput($"[ARRAY DECL] Error: {ex.Message}\r\n");
            }
        }

        /// <summary>
        /// Processes PEEK commands to retrieve values from array elements into variables.
        /// Supports two formats: "peek variable = arrayName[index]" or "peek variable = arrayName index".
        /// The retrieved value is stored in the specified variable.
        /// </summary>
        /// <param name="commandLine">The PEEK command line to process.</param>
        /// <example>
        /// peek x = nums[5]          // Retrieves value from nums[5] into variable x
        /// peek y = prices 5         // Retrieves value from prices[5] into variable y
        /// </example>
        public void ProcessPeek(string commandLine)
        {
            try
            {
                string rest = commandLine.Substring(5).Trim();
                outputTextBox.AppendText($"[PEEK] Parsing: '{rest}'\r\n");

                int equalsIndex = rest.IndexOf('=');
                if (equalsIndex < 0)
                {
                    outputTextBox.AppendText($"[PEEK] Error: Missing '=' in command\r\n");
                    return;
                }

                string varName = rest.Substring(0, equalsIndex).Trim();
                string arrayExpr = rest.Substring(equalsIndex + 1).Trim();

                string arrayName;
                int index;

                if (arrayExpr.Contains('[') && arrayExpr.Contains(']'))
                {
                    int bracketStart = arrayExpr.IndexOf('[');
                    int bracketEnd = arrayExpr.IndexOf(']');

                    arrayName = arrayExpr.Substring(0, bracketStart).Trim();
                    string indexStr = arrayExpr.Substring(bracketStart + 1, bracketEnd - bracketStart - 1).Trim();

                    object indexObj = EvaluateExpression(indexStr);
                    if (indexObj is int idx)
                    {
                        index = idx;
                    }
                    else if (indexObj is double dbl && Math.Abs(dbl - Math.Round(dbl)) < 0.000001)
                    {
                        index = (int)Math.Round(dbl);
                    }
                    else
                    {
                        outputTextBox.AppendText($"[PEEK] Error: Invalid index expression '{indexStr}'\r\n");
                        return;
                    }
                }
                else
                {
                    string[] arrayParts = arrayExpr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (arrayParts.Length < 2)
                    {
                        outputTextBox.AppendText($"[PEEK] Error: Invalid array expression '{arrayExpr}'. Expected 'arrayName index' or 'arrayName[index]'\r\n");
                        return;
                    }

                    arrayName = arrayParts[0];
                    object indexObj = EvaluateExpression(arrayParts[1]);
                    if (indexObj is int idx)
                    {
                        index = idx;
                    }
                    else if (indexObj is double dbl && Math.Abs(dbl - Math.Round(dbl)) < 0.000001)
                    {
                        index = (int)Math.Round(dbl);
                    }
                    else
                    {
                        outputTextBox.AppendText($"[PEEK] Error: Invalid index '{indexObj}'\r\n");
                        return;
                    }
                }

                if (arrays.ContainsKey(arrayName))
                {
                    object[] array = arrays[arrayName];

                    if (index >= 0 && index < array.Length)
                    {
                        object value = array[index];
                        variables[varName] = value;
                        outputTextBox.AppendText($"Peek: {varName} = {arrayName}[{index}] = {value}\r\n");
                    }
                    else
                    {
                        outputTextBox.AppendText($"[PEEK] Error: Index {index} out of bounds for array {arrayName} (size: {array.Length})\r\n");
                    }
                }
                else
                {
                    outputTextBox.AppendText($"[PEEK] Error: Array '{arrayName}' not found\r\n");
                }
            }
            catch (Exception ex)
            {
                outputTextBox.AppendText($"[PEEK] Error: {ex.Message}\r\n");
                outputTextBox.AppendText($"[PEEK] Stack trace: {ex.StackTrace}\r\n");
            }
        }

        /// <summary>
        /// Processes POKE commands to store values into array elements.
        /// Supports two formats: "poke arrayName[index] = value" or "poke arrayName index = value".
        /// The value is automatically converted to match the array's declared type.
        /// </summary>
        /// <param name="commandLine">The POKE command line to process.</param>
        /// <example>
        /// poke nums[5] = 99          // Stores 99 at index 5 of nums array
        /// poke prices 5 = 99.99      // Stores 99.99 at index 5 of prices array
        /// </example>
        public void ProcessPoke(string commandLine)
        {
            try
            {
                string rest = commandLine.Substring(5).Trim();
                outputTextBox.AppendText($"[POKE] Parsing: '{rest}'\r\n");

                int equalsIndex = rest.LastIndexOf('=');
                if (equalsIndex < 0)
                {
                    outputTextBox.AppendText($"[POKE] Error: Missing '=' in command\r\n");
                    return;
                }

                string left = rest.Substring(0, equalsIndex).Trim();
                string right = rest.Substring(equalsIndex + 1).Trim();

                string arrayName;
                int index;

                if (left.Contains('[') && left.Contains(']'))
                {
                    int bracketStart = left.IndexOf('[');
                    int bracketEnd = left.IndexOf(']');

                    arrayName = left.Substring(0, bracketStart).Trim();
                    string indexStr = left.Substring(bracketStart + 1, bracketEnd - bracketStart - 1).Trim();

                    object indexObj = EvaluateExpression(indexStr);
                    if (indexObj is int idx)
                    {
                        index = idx;
                    }
                    else if (indexObj is double dbl && Math.Abs(dbl - Math.Round(dbl)) < 0.000001)
                    {
                        index = (int)Math.Round(dbl);
                    }
                    else
                    {
                        outputTextBox.AppendText($"[POKE] Error: Invalid index expression '{indexStr}'\r\n");
                        return;
                    }
                }
                else
                {
                    string[] leftParts = left.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (leftParts.Length < 2)
                    {
                        outputTextBox.AppendText($"[POKE] Error: Invalid format '{left}'. Expected 'arrayName index' or 'arrayName[index]'\r\n");
                        return;
                    }

                    arrayName = leftParts[0];
                    object indexObj = EvaluateExpression(leftParts[1]);
                    if (indexObj is int idx)
                    {
                        index = idx;
                    }
                    else if (indexObj is double dbl && Math.Abs(dbl - Math.Round(dbl)) < 0.000001)
                    {
                        index = (int)Math.Round(dbl);
                    }
                    else
                    {
                        outputTextBox.AppendText($"[POKE] Error: Invalid index '{indexObj}'\r\n");
                        return;
                    }
                }

                object value = EvaluateExpression(right);
                outputTextBox.AppendText($"[POKE] Value evaluated as: {value} (type: {value?.GetType().Name})\r\n");

                if (arrayTypes.ContainsKey(arrayName))
                {
                    Type elementType = arrayTypes[arrayName];
                    if (elementType == typeof(int))
                    {
                        if (value is double dblValue)
                        {
                            value = (int)Math.Round(dblValue);
                        }
                        else if (value is string strValue && double.TryParse(strValue, out double parsedDbl))
                        {
                            value = (int)Math.Round(parsedDbl);
                        }
                        outputTextBox.AppendText($"[POKE] Converting to int: {value}\r\n");
                    }
                    else if (elementType == typeof(double))
                    {
                        if (value is int intValue)
                        {
                            value = (double)intValue;
                        }
                        else if (value is string strValue && double.TryParse(strValue, out double parsedDbl))
                        {
                            value = parsedDbl;
                        }
                        outputTextBox.AppendText($"[POKE] Converting to double: {value}\r\n");
                    }
                }

                if (arrays.ContainsKey(arrayName))
                {
                    object[] array = arrays[arrayName];

                    if (index >= 0 && index < array.Length)
                    {
                        array[index] = value;
                        outputTextBox.AppendText($"Poke: {arrayName}[{index}] = {value}\r\n");
                    }
                    else
                    {
                        outputTextBox.AppendText($"[POKE] Error: Index {index} out of bounds for array {arrayName} (size: {array.Length})\r\n");
                    }
                }
                else
                {
                    outputTextBox.AppendText($"[POKE] Error: Array '{arrayName}' not found\r\n");
                }
            }
            catch (Exception ex)
            {
                outputTextBox.AppendText($"[POKE] Error: {ex.Message}\r\n");
                outputTextBox.AppendText($"[POKE] Stack trace: {ex.StackTrace}\r\n");
            }
        }

        /// <summary>
        /// Evaluates a mathematical expression that may contain variables and operators.
        /// This method replaces variables with their values, then evaluates the expression.
        /// Supports addition, subtraction, and multiplication.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <returns>The result as either an integer or double.</returns>
        public object EvaluateExpression(string expression)
        {
            try
            {
                expression = expression.Trim();
                outputTextBox.AppendText($"[EVAL] Starting evaluation of: '{expression}'\r\n");

                if (expression.StartsWith("\"") && expression.EndsWith("\""))
                {
                    return expression.Trim('"');
                }

                var sortedVariableNames = variables.Keys.OrderByDescending(k => k.Length).ToList();

                foreach (string varName in sortedVariableNames)
                {
                    string pattern = @"\b" + Regex.Escape(varName) + @"\b";
                    if (Regex.IsMatch(expression, pattern))
                    {
                        string replacementValue;
                        if (variables[varName] is double doubleVal)
                        {
                            replacementValue = doubleVal.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            replacementValue = variables[varName]?.ToString() ?? "";
                        }

                        string before = expression;
                        expression = Regex.Replace(expression, pattern, replacementValue);
                        outputTextBox.AppendText($"[EVAL] Replaced '{varName}' with '{replacementValue}': '{before}' -> '{expression}'\r\n");
                    }
                }

                expression = expression.Replace(" ", "");
                outputTextBox.AppendText($"[EVAL] After removing spaces: '{expression}'\r\n");

                // Handle subtraction 
                if (expression.Contains("-") && !expression.StartsWith("-"))
                {
                    outputTextBox.AppendText($"[EVAL] Found subtraction in expression\r\n");

                    string[] parts = expression.Split('-');
                    if (parts.Length == 2)
                    {
                        string leftStr = parts[0];
                        string rightStr = parts[1];

                        // Evaluate left and right sides
                        double left = ParseSimpleNumber(leftStr);
                        double right = ParseSimpleNumber(rightStr);

                        double result = left - right;
                        outputTextBox.AppendText($"[EVAL] Subtraction result: {left} - {right} = {result}\r\n");

                        if (Math.Abs(result - Math.Round(result)) < 0.000001)
                        {
                            return (int)Math.Round(result);
                        }
                        return result;
                    }
                }

                // Handle addition
                if (expression.Contains("+") && !expression.StartsWith("+"))
                {
                    outputTextBox.AppendText($"[EVAL] Found addition in expression\r\n");

                    string[] parts = expression.Split('+');
                    if (parts.Length == 2)
                    {
                        string leftStr = parts[0];
                        string rightStr = parts[1];

                        double left = ParseSimpleNumber(leftStr);
                        double right = ParseSimpleNumber(rightStr);

                        double result = left + right;
                        outputTextBox.AppendText($"[EVAL] Addition result: {left} + {right} = {result}\r\n");

                        if (Math.Abs(result - Math.Round(result)) < 0.000001)
                        {
                            return (int)Math.Round(result);
                        }
                        return result;
                    }
                }
                
                //handle multiplication
                if (expression.Contains("*"))
                {
                    outputTextBox.AppendText($"[EVAL] Found multiplication in expression\r\n");

                    string[] factors = expression.Split('*');
                    outputTextBox.AppendText($"[EVAL] Split into {factors.Length} factors\r\n");

                    double multiplicationResult = 1.0;
                    bool allFactorsValid = true;

                    foreach (string factor in factors)
                    {
                        outputTextBox.AppendText($"[EVAL] Processing factor: '{factor}'\r\n");

                        if (double.TryParse(factor, NumberStyles.Any, CultureInfo.InvariantCulture, out double factorValue))
                        {
                            multiplicationResult *= factorValue;
                            outputTextBox.AppendText($"[EVAL] Multiplied by {factorValue}, current result: {multiplicationResult}\r\n");
                        }
                        else
                        {
                            outputTextBox.AppendText($"[EVAL] Could not parse factor '{factor}' as a number\r\n");
                            allFactorsValid = false;
                            break;
                        }
                    }

                    if (allFactorsValid)
                    {
                        outputTextBox.AppendText($"[EVAL] Final multiplication result: {multiplicationResult}\r\n");

                        if (Math.Abs(multiplicationResult - Math.Round(multiplicationResult)) < 0.000001)
                        {
                            return (int)Math.Round(multiplicationResult);
                        }
                        return multiplicationResult;
                    }
                }

                //handle division
                if (expression.Contains("/"))
                {
                    outputTextBox.AppendText($"[EVAL] Found division in expression\r\n");

                    string[] parts = expression.Split('/');
                    if (parts.Length == 2)
                    {
                        string leftStr = parts[0];
                        string rightStr = parts[1];

                        double left = ParseSimpleNumber(leftStr);
                        double right = ParseSimpleNumber(rightStr);

                        if (Math.Abs(right) < 0.000001)
                        {
                            outputTextBox.AppendText($"[EVAL] Division by zero error\r\n");
                            return 0.0;
                        }

                        double result = left / right;
                        outputTextBox.AppendText($"[EVAL] Division result: {left} / {right} = {result}\r\n");

                        if (Math.Abs(result - Math.Round(result)) < 0.000001)
                        {
                            return (int)Math.Round(result);
                        }
                        return result;
                    }
                }

                if (double.TryParse(expression, NumberStyles.Any, CultureInfo.InvariantCulture, out double singleValue))
                {
                    outputTextBox.AppendText($"[EVAL] Parsed as single value: {singleValue}\r\n");

                    if (Math.Abs(singleValue - Math.Round(singleValue)) < 0.000001)
                    {
                        return (int)Math.Round(singleValue);
                    }
                    return singleValue;
                }

                outputTextBox.AppendText($"[EVAL] Could not parse expression: '{expression}'\r\n");
                return 0.0;
            }
            catch (Exception ex)
            {
                outputTextBox.AppendText($"[EVAL] Error evaluating expression: {ex.Message}\r\n");
                outputTextBox.AppendText($"[EVAL] Stack trace: {ex.StackTrace}\r\n");
                return 0.0;
            }
        }

        /// <summary>
        /// Parses a simple numeric value from the specified text.
        /// </summary>
        /// <param name="text">The text that should represent a numeric value.</param>
        /// <returns>
        /// The parsed value as a <see cref="double"/> if parsing succeeds; otherwise, <c>0.0</c>.
        /// </returns>
        private double ParseSimpleNumber(string text)
        {
            text = text.Trim();
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            return 0.0;
        }

        /// <summary>
        /// Handles assignment statements of the form <c>name = expression</c>
        /// by evaluating the right-hand side and storing the result in the variables dictionary.
        /// </summary>
        /// <param name="commandLine">The assignment command line to process.</param>
        public void ProcessAssignment(string commandLine)
        {
            string[] parts = commandLine.Split('=');
            if (parts.Length < 2) return;

            string left = parts[0].Trim();
            string right = parts[1].Trim();

            outputTextBox.AppendText($"[ASSIGN] {left} = {right}\r\n");

            object value = EvaluateExpression(right);

            if (value is int intValue)
            {
                variables[left] = intValue;
                outputTextBox.AppendText($"{left} = {intValue}\r\n");
            }
            else if (value is double dblValue)
            {
                variables[left] = dblValue;
                outputTextBox.AppendText($"{left} = {dblValue}\r\n");
            }
        }

        /// <summary>
        /// Handles <c>write</c> commands by evaluating expressions and writing the result text to the canvas.
        /// Supports string concatenation with <c>+</c> and expression evaluation for non-string parts.
        /// </summary>
        /// <param name="commandLine">The write command line to process.</param>
        private void ProcessWriteCommand(string commandLine)
        {
            try
            {
                AppCommandFactory factory = AppCommandFactory.Instance;
                string writeText = commandLine.Substring(6).Trim();
                outputTextBox.AppendText($"[WRITE] Processing: '{writeText}'\r\n");

                if (writeText.Contains("+"))
                {
                    string[] parts = writeText.Split('+');
                    StringBuilder result = new StringBuilder();

                    foreach (string part in parts)
                    {
                        string trimmedPart = part.Trim();

                        if (trimmedPart.StartsWith("\"") && trimmedPart.EndsWith("\""))
                        {
                            result.Append(trimmedPart.Trim('"'));
                        }
                        else
                        {
                            object evalResult = EvaluateExpression(trimmedPart);
                            result.Append(FormatResultForDisplay(evalResult));
                        }
                    }

                    string evaluatedText = result.ToString();
                    outputTextBox.AppendText($"[WRITE] Concatenated result: '{evaluatedText}'\r\n");

                    ICommand textWriteCommand = new WriteTextCommand(evaluatedText);
                    textWriteCommand.Execute(canvas);
                    RefreshCanvas();
                }
                else
                {
                    object result = EvaluateExpression(writeText);
                    string evaluatedText = FormatResultForDisplay(result);

                    ICommand textWriteCommand = new WriteTextCommand(evaluatedText);
                    textWriteCommand.Execute(canvas);
                    RefreshCanvas();
                }
            }
            catch (Exception ex)
            {
                outputTextBox.AppendText($"[WRITE] Error: {ex.Message}\r\n");
            }
        }

        /// <summary>
        /// Formats an evaluation result for display on the canvas.
        /// Applies a compact numeric format for floating-point values and default formatting for other types.
        /// </summary>
        /// <param name="result">The evaluated result to format.</param>
        /// <returns>A string representation suitable for display.</returns>
        private string FormatResultForDisplay(object result)
        {
            if (result == null) return "";

            if (result is double d)
            {
                return d.ToString("0.0##############", CultureInfo.InvariantCulture);
            }

            return result.ToString();
        }

        /// <summary>
        /// Demonstrates the design patterns used in the application (Singleton, Factory, and Command).
        /// Writes diagnostic information about the patterns and a small execution test to the output.
        /// </summary>
        private void DemonstrateDesignPatterns()
        {
            try
            {
                outputTextBox.AppendText("\n=== Design Pattern Demonstration ===\r\n");

                outputTextBox.AppendText("\n1. SINGLETON PATTERN TEST:\r\n");
                outputTextBox.AppendText("Getting AppCommandFactory.Instance twice...\r\n");

                var factory1 = AppCommandFactory.Instance;
                outputTextBox.AppendText($"First call - Factory created.\r\n");

                var factory2 = AppCommandFactory.Instance;
                outputTextBox.AppendText($"Second call - Factory retrieved.\r\n");

                bool isSingleton = ReferenceEquals(factory1, factory2);
                outputTextBox.AppendText($"Same instance: {isSingleton}\r\n");

                if (isSingleton)
                {
                    outputTextBox.AppendText("Singleton pattern confirmed.\r\n");
                }

                outputTextBox.AppendText("\n2. FACTORY PATTERN TEST:\r\n");
                outputTextBox.AppendText("Creating command objects...\r\n");

                ICommand moveCommand = factory1.CreateCommand("moveto 100 100");
                outputTextBox.AppendText($"MoveTo command created: {moveCommand != null}\r\n");

                ICommand circleCommand = factory1.CreateCommand("circle 50");
                outputTextBox.AppendText($"Circle command created: {circleCommand != null}\r\n");

                ICommand penCommand = factory1.CreateCommand("pen 255 0 0");
                outputTextBox.AppendText($"Pen command created: {penCommand != null}\r\n");

                ICommand rectCommand = factory1.CreateCommand("rect 80 60");
                outputTextBox.AppendText($"Rectangle command created: {rectCommand != null}\r\n");

                outputTextBox.AppendText("Factory pattern confirmed.\r\n");

                outputTextBox.AppendText("\n3. COMMAND PATTERN TEST:\r\n");

                outputTextBox.AppendText("Command classes found:\r\n");
                outputTextBox.AppendText("  • AppMoveTo\r\n");
                outputTextBox.AppendText("  • AppCircle\r\n");
                outputTextBox.AppendText("  • AppRectangle\r\n");
                outputTextBox.AppendText("  • SetPenColorCommand\r\n");
                outputTextBox.AppendText("  • ClearCommand\r\n");
                outputTextBox.AppendText("  • WriteTextCommand\r\n");

                outputTextBox.AppendText("Command pattern confirmed.\r\n");

                outputTextBox.AppendText("\n4. EXECUTION TEST:\r\n");
                try
                {
                    ICommand testCommand = factory1.CreateCommand("moveto 50 50");
                    if (testCommand != null)
                    {
                        testCommand.Execute(canvas);
                        outputTextBox.AppendText($"Executed MoveTo command on canvas\r\n");
                        outputTextBox.AppendText($"Canvas position: ({canvas.Xpos}, {canvas.Ypos})\r\n");
                        outputTextBox.AppendText("All patterns working correctly.\r\n");
                    }
                }
                catch (Exception ex)
                {
                    outputTextBox.AppendText($"Execution test: {ex.Message}\r\n");
                }

                outputTextBox.AppendText("\n==========================================\r\n\r\n");
            }
            catch (Exception ex)
            {
                outputTextBox.AppendText($"Error demonstrating patterns: {ex.Message}\r\n");
            }
        }

        /// <summary>
        /// Replaces variable names in a command line with their current values,
        /// matching variables as whole words and normalizing spacing around operators.
        /// </summary>
        /// <param name="commandLine">The command line that may contain variable references.</param>
        /// <returns>The command line with all known variables replaced by their formatted values.</returns>
        private string ReplaceVariables(string commandLine)
        {
            string result = commandLine;

            // Use current variables (could be global or method scope)
            var sortedVars = variables.Keys.OrderByDescending(k => k.Length).ToList();

            foreach (string varName in sortedVars)
            {
                string pattern = @"\b" + Regex.Escape(varName) + @"\b";

                if (Regex.IsMatch(result, pattern))
                {
                    string replacementValue = FormatNumber(variables[varName]);
                    string before = result;
                    result = Regex.Replace(result, pattern, replacementValue);
                    outputTextBox.AppendText($"[REPLACE] '{varName}' -> '{replacementValue}': '{before}' -> '{result}'\r\n");
                }
            }

            result = Regex.Replace(result, @"\s*\*\s*", "*");
            result = Regex.Replace(result, @"\s*/\s*", "/");
            result = Regex.Replace(result, @"\s*\+\s*", "+");
            result = Regex.Replace(result, @"\s*-\s*", "-");

            return result.Trim();
        }

        /// <summary>
        /// Compares two method names for equivalence, allowing for common typos such as duplicated letters.
        /// </summary>
        /// <param name="method1">The first method name to compare.</param>
        /// <param name="method2">The second method name to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the method names are considered equivalent; otherwise, <see langword="false"/>.
        /// </returns>
        private bool AreMethodNamesEqual(string method1, string method2)
        {
            // Exact match
            if (method1 == method2) return true;

            // Case-insensitive match
            if (string.Equals(method1, method2, StringComparison.OrdinalIgnoreCase))
                return true;

            // Remove duplicate letters and compare (for mullMethod -> mulMethod)
            string simple1 = RemoveDuplicateLetters(method1.ToLower());
            string simple2 = RemoveDuplicateLetters(method2.ToLower());

            return simple1 == simple2;
        }

        /// <summary>
        /// Removes consecutive duplicate letters from the specified text,
        /// for example converting <c>"mullMethod"</c> to <c>"mulMethod"</c>.
        /// </summary>
        /// <param name="text">The input text from which to remove duplicate letters.</param>
        /// <returns>The simplified text without consecutive duplicate characters.</returns>
        private string RemoveDuplicateLetters(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            StringBuilder result = new StringBuilder();
            result.Append(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                if (text[i] != text[i - 1])
                {
                    result.Append(text[i]);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Processes a method call command starting with <c>call</c>,
        /// resolves the target method (tolerating minor name typos), binds parameters, and executes the method body.
        /// Stores the return value under the user-specified method name.
        /// </summary>
        /// <param name="commandLine">The method call command line.</param>
        private void ProcessMethodCall(string commandLine)
        {
            try
            {
                // Get method name and parameters
                string withoutCall = commandLine.Substring(5).Trim();
                string[] parts = withoutCall.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                {
                    AppendOutput($"[ERROR] Invalid method call: {commandLine}\r\n");
                    return;
                }

                string userMethodName = parts[0];

                // Find actual method name 
                string actualMethodName = null;
                foreach (var key in methods.Keys)
                {
                    if (AreMethodNamesEqual(key, userMethodName))
                    {
                        actualMethodName = key;
                        break;
                    }
                }

                if (actualMethodName == null)
                {
                    AppendOutput($"[ERROR] Method '{userMethodName}' not found\r\n");
                    return;
                }

                MethodInfo method = methods[actualMethodName];

                if (parts.Length - 1 != method.Parameters.Count)
                {
                    AppendOutput($"[ERROR] Method '{userMethodName}' expects {method.Parameters.Count} parameters, got {parts.Length - 1}\r\n");
                    return;
                }

                AppendOutput($"[METHOD CALL] Calling '{userMethodName}' with {method.Parameters.Count} parameters\r\n");

                // Create method variable scope
                Dictionary<string, object> methodScope = new Dictionary<string, object>();

                // Set parameter values
                for (int i = 0; i < method.Parameters.Count; i++)
                {
                    string paramName = method.Parameters[i];
                    string paramValueStr = parts[i + 1];

                    object paramValue = EvaluateExpression(paramValueStr);
                    methodScope[paramName] = paramValue;

                    AppendOutput($"[METHOD CALL] Set parameter {paramName} = {paramValue}\r\n");
                }

                // Save current variables
                var savedVariables = new Dictionary<string, object>(variables);

                // Use method scope for execution
                variables = methodScope;

                // Execute method body
                foreach (string bodyLine in method.Body)
                {
                    string trimmedLine = bodyLine.Trim();

                    if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("*"))
                        continue;

                    AppendOutput($"[METHOD BODY] Processing: '{trimmedLine}'\r\n");

                    // Check for return statement like "mulMethod = one * two"
                    if (trimmedLine.StartsWith(actualMethodName + " ="))
                    {
                        string expression = trimmedLine.Substring(actualMethodName.Length + 2).Trim();

                        object result = EvaluateExpression(expression);

                        AppendOutput($"[METHOD RETURN] {actualMethodName} = {result}\r\n");

                        // Store result with USER's method name (with typo)
                        methodScope[userMethodName] = result;
                        variables[userMethodName] = result;

                        lastMethodReturnValue = result;
                    }
                    else if (trimmedLine.Contains("="))
                    {
                        ProcessAssignment(trimmedLine);
                    }
                    else
                    {
                        ProcessCommand(trimmedLine);
                    }
                }

                // Get return value
                if (methodScope.ContainsKey(userMethodName))
                {
                    lastMethodReturnValue = methodScope[userMethodName];
                }
                else
                {
                    lastMethodReturnValue = 0;
                }

                // Restore original variables
                variables = savedVariables;

                // Store return value with USER's method name
                variables[userMethodName] = lastMethodReturnValue;
                AppendOutput($"[METHOD CALL] Method '{userMethodName}' returned {lastMethodReturnValue}\r\n");

                // Clean up
                methodVariableScopes.Clear();
            }
            catch (Exception ex)
            {
                AppendOutput($"[ERROR] Method call error: {ex.Message}\r\n");
                AppendOutput($"[ERROR] Stack trace: {ex.StackTrace}\r\n");
            }
        }

        /// <summary>
        /// Clears the drawing canvas when the Clear Canvas button is clicked and reports the action in the output.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ClearCanvasButton_Click(object sender, EventArgs e)
        {
            try
            {
                canvas.Clear();
                RefreshCanvas();
                outputTextBox.AppendText("Canvas cleared\r\n");
            }
            catch (Exception ex)
            {
                outputTextBox.AppendText($"Error clearing canvas: {ex.Message}\r\n");
            }
        }

        /// <summary>
        /// Clears the output text box when the Clear Output button is clicked and redisplays the about information.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ClearOutputButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Clear();
            DisplayAboutInfo();
        }

        /// <summary>
        /// Handles the form-closing event by releasing canvas-related resources.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The form-closing event data.</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CleanupResources();
        }

        /// <summary>
        /// Releases resources used by the canvas and picture box.
        /// </summary>
        private void CleanupResources()
        {
            try
            {
                canvas?.Dispose();
                if (pictureBox?.Image != null)
                {
                    pictureBox.Image.Dispose();
                    pictureBox.Image = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during cleanup: {ex.Message}");
            }
        }

        /// <summary>
        /// Appends the specified text to the output text box, marshaling to the UI thread if required.
        /// </summary>
        /// <param name="text">The text to append to the output.</param>
        public void AppendOutput(string text)
        {
            if (outputTextBox.InvokeRequired)
            {
                outputTextBox.Invoke(new Action(() => outputTextBox.AppendText(text)));
            }
            else
            {
                outputTextBox.AppendText(text);
            }
        }

        /// <summary>
        /// Handles the Paint event of the canvas panel.
        /// The actual drawing is managed by the <see cref="AppCanvas"/> class.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The paint event data.</param> 
        private void canvasPanel_Paint(object sender, PaintEventArgs e)
        {
            // The canvas painting is handled by the AppCanvas class
        }
    }
}