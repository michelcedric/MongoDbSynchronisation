using CommandLine;

namespace MongoSync.CommandLine
{
    /// <summary>
    /// Represent the data option fo the application
    /// </summary>
    public class ConsoleOptions
    {
        /// <summary>
        /// Indicate if the applilcation run without user interaction
        /// </summary>
        [Option('q', "quiet", Required = false, HelpText = "Run without user interaction")]
        public bool Silent { get; set; }

        /// <summary>
        /// Specified the source url database
        /// </summary>
        [Option('s', "source", Required = false, HelpText = "Specified the source url database")]
        public string Source { get; set; }

        /// <summary>
        /// Specified the target url database
        /// </summary>
        [Option('t', "target", Required = false, HelpText = "Specified the target url database")]
        public string Target { get; set; }

        /// <summary>
        /// Indicate if you want to execute the function synchronisation
        /// </summary>
        [Option('f', "function", Required = false, HelpText = "Execute function synchronisation")]
        public bool ExecuteFunction { get; set; }

        /// <summary>
        /// Path to write function script disk
        /// </summary>
        [Option('p', "path", Required = false, HelpText = "Path to write function script disk")]
        public string Path { get; set; }

        /// <summary>
        /// Indicate if you want to store the function on disk
        /// </summary>
        [Option('w', "write", Required = false, HelpText = "Write the function on disk - Don't forget to initialize the path -p/--path")]
        public bool WriteFunctionOnDisk { get; set; }

        /// <summary>
        /// Indicate if you want to check function integrity before launc synchronisation
        /// </summary>
        [Option('c', "check", Required = false, HelpText = "Check function integrity before launc synchronisation")]
        public bool CheckFunctionIntegrity { get; set; }
    }
}
