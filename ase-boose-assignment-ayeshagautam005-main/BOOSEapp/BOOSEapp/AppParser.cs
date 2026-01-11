using System;
using System.Collections.Generic;

namespace BOOSEapp
{
    /// <summary>
    /// Defines a contract for parsing textual commands into executable command objects.
    /// </summary>
    public interface ICommandParser
    {
        /// <summary>
        /// Parses an array of command strings into a list of <see cref="ICommand"/> instances.
        /// </summary>
        /// <param name="commands">An array of command strings to parse.</param>
        /// <returns>A list of parsed <see cref="ICommand"/> objects.</returns>
        List<ICommand> ParseCommands(string[] commands);
    }

    /// <summary>
    /// Provides functionality to parse text lines into command objects using a command factory.
    /// </summary>
    public class AppParser : ICommandParser
    {
        private AppCommandFactory commandFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppParser"/> class with the specified command factory.
        /// </summary>
        /// <param name="factory">The factory used to create command instances.</param>
        public AppParser(AppCommandFactory factory)
        {
            commandFactory = factory;
        }

        /// <summary>
        /// Parses an array of command strings into a list of command objects.
        /// </summary>
        /// <param name="commands">The command strings to parse.</param>
        /// <returns>A list of <see cref="ICommand"/> objects created from the provided strings.</returns>
        /// <exception cref="CanvasException">
        /// Thrown when an error occurs while parsing an individual command line.
        /// </exception>
        public List<ICommand> ParseCommands(string[] commands)
        {
            var parsedCommands = new List<ICommand>();

            foreach (var line in commands)
            {
                var trimmedLine = line.Trim();

                // Skip empty lines and comments
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("*"))
                    continue;

                try
                {
                    var command = commandFactory.CreateCommand(trimmedLine);
                    if (command != null)
                    {
                        parsedCommands.Add(command);
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Could not parse command: {trimmedLine}");
                    }
                }
                catch (Exception ex)
                {
                    throw new CanvasException($"Error parsing command '{trimmedLine}': {ex.Message}");
                }
            }

            return parsedCommands;
        }
    }
}
