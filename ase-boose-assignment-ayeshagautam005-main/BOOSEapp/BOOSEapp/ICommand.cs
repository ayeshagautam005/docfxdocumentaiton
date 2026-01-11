namespace BOOSEapp
{
    /// <summary>
    /// Defines a command in the drawing program that follows the Command pattern to encapsulate a drawing operation.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command on the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to execute the command.</param>
        void Execute(ICanvas canvas);
    }
}
