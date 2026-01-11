using System;
using System.Text.RegularExpressions;

namespace BOOSEapp
{
    /// <summary>
    /// Provides a singleton factory for creating <see cref="ICommand"/> instances from text input.
    /// </summary>
    public sealed class AppCommandFactory
    {
        private static AppCommandFactory _instance;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandFactory"/> class.
        /// This constructor is private to enforce the singleton pattern.
        /// </summary>
        private AppCommandFactory()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="AppCommandFactory"/>.
        /// </summary>
        public static AppCommandFactory Instance
        {
            get
            {
                // Double-check locking for thread safety
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new AppCommandFactory();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Creates an <see cref="ICommand"/> instance from the specified command line text.
        /// </summary>
        /// <param name="commandLine">
        /// The textual representation of the command, including any arguments,
        /// for example: <c>"moveto 100 200"</c> or <c>"circle 50"</c>.
        /// </param>
        /// <returns>
        /// An <see cref="ICommand"/> that corresponds to the parsed command text,
        /// or <see langword="null"/> if the command is not recognized or cannot be parsed.
        /// </returns>
        /// <exception cref="CanvasException">
        /// Thrown when an error occurs while creating the command from the specified text.
        /// </exception>
        public ICommand CreateCommand(string commandLine)
        {
            if (string.IsNullOrWhiteSpace(commandLine))
                return null;

            // Clean up the input
            commandLine = commandLine.Trim();
            string normalized = commandLine.Replace(",", " ");
            normalized = Regex.Replace(normalized, @"\s+", " ");

            string[] parts = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return null;

            string command = parts[0].ToLower();

            try
            {
                switch (command)
                {
                    case "moveto":
                        // Parse x and y coordinates
                        if (parts.Length >= 3 &&
                            int.TryParse(parts[1], out int x) &&
                            int.TryParse(parts[2], out int y))
                        {
                            return new AppMoveTo(x, y);
                        }
                        break;

                    case "circle":
                        // Parse radius
                        if (parts.Length >= 2 &&
                            int.TryParse(parts[1], out int radius))
                        {
                            return new AppCircle(radius);
                        }
                        break;

                    case "rect":
                    case "rectangle":
                        // Parse width and height
                        if (parts.Length >= 3 &&
                            int.TryParse(parts[1], out int width) &&
                            int.TryParse(parts[2], out int height))
                        {
                            return new AppRectangle(width, height);
                        }
                        break;

                    case "pen":
                        // Parse RGB values
                        if (parts.Length >= 4)
                        {
                            if (int.TryParse(parts[1], out int r) &&
                                int.TryParse(parts[2], out int g) &&
                                int.TryParse(parts[3], out int b))
                            {
                                return new SetPenColorCommand(r, g, b);
                            }
                        }
                        break;

                    case "clear":
                        return new ClearCommand();

                    case "reset":
                        return new ResetCommand();

                    case "write":
                        if (parts.Length >= 2)
                        {
                            // Get everything after "write"
                            string text = commandLine.Substring(5).Trim();
                            return new WriteTextCommand(text);
                        }
                        break;

                    case "int":
                        return new AppInt(commandLine);

                    case "real":
                        return new AppReal(commandLine);

                    case "array":
                        return new AppArray(commandLine);

                    case "while":
                        return new AppWhile(commandLine);

                    case "for":
                        return new AppFor(commandLine);

                    case "if":
                        return new AppIf(commandLine);

                    case "method":
                        return new AppMethod(commandLine);
                }
            }
            catch (Exception ex)
            {
                throw new CanvasException($"Error creating command '{commandLine}': {ex.Message}");
            }

            // Command not recognized
            return null;
        }
    }
}
