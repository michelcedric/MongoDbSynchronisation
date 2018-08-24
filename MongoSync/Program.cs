using CommandLine;
using MongoSync.CommandLine;
using MongoSync.MongoDb.Function;
using System;

namespace MongoSync
{
    
    class Program
    {
       
        static void Main(string[] args)
        {
            var consoleOptions = new ConsoleOptions();

            ParserResult<ConsoleOptions> parserResult = Parser.Default.ParseArguments<ConsoleOptions>(args);

            if (parserResult.Tag == ParserResultType.Parsed)
            {
                var parsed = parserResult as Parsed<ConsoleOptions>;
                consoleOptions = parsed.Value;

                Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Process started ...");
                bool result = true;
                try
                {
                    if (consoleOptions.ExecuteFunction)
                    {
                        result = ExecuteFunctionSynchronisation(consoleOptions);
                    }
                }
                catch (Exception exception)
                {
                    result = false;
                    Console.WriteLine(exception.Message);
                }

                if (result)
                {
                    Environment.ExitCode = 0;
                    Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Proccess successufully done");
                }
                else
                {
                    Environment.ExitCode = -1;
                    Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Proccess ended with error...");
                    Parser.Default.ParseArguments<ConsoleOptions>(new[] { "--help" });
                }
            }
            
            if (!consoleOptions.Silent)
            {
                Console.ReadKey();
            }
        }

        private static bool ExecuteFunctionSynchronisation(ConsoleOptions option)
        {
            bool result;
            MongoDbFunctionsSynchronization mongoFunctionsSynchronization;

            if (!string.IsNullOrEmpty(option.Source) && !string.IsNullOrEmpty(option.Target))
            {
                mongoFunctionsSynchronization = new MongoDbFunctionsSynchronization(option.Source, option.Target);
            }
            else
            {
                mongoFunctionsSynchronization = new MongoDbFunctionsSynchronization();
            }

            mongoFunctionsSynchronization.FunctionFolderPath = option.Path;
            result = mongoFunctionsSynchronization.Execute(option.WriteFunctionOnDisk, option.CheckFunctionIntegrity);
            return result;
        }
    }
}
;