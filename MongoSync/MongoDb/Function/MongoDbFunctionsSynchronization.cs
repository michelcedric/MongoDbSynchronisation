using MongoSync.MongoDb.Function.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MongoSync.MongoDb.Function
{
    /// <summary>
    /// Object use to synchrnonize mongoDb functions
    /// </summary>
    public class MongoDbFunctionsSynchronization
    {
        private MongoDbCommand _mongoCommand;

        /// <summary>
        /// Initialize the MongoDbFunctionsSynchronization with a specific url
        /// </summary>
        /// <param name="connectionStringSourceUrl">The source Url</param>
        /// <param name="connectionStringTargetUrl">The target Url</param>
        public MongoDbFunctionsSynchronization(string connectionStringSourceUrl, string connectionStringTargetUrl)
        {
            Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Initialize the synchronisation...");
            _mongoCommand = new MongoDbCommand(connectionStringSourceUrl, connectionStringTargetUrl);
            Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Synchronisation initialized");
        }

        /// <summary>
        /// Initialize the MongoDbFunctionsSynchronization use the config file
        /// </summary>
        public MongoDbFunctionsSynchronization()
        {
            Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Initialize the synchronisation...");
            _mongoCommand = new MongoDbCommand();
            Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Synchronisation initialized");
        }

        /// <summary>
        /// The path where the function file will be store
        /// </summary>
        public string FunctionFolderPath { get; set; }

        /// <summary>
        /// Launch the synchronisation
        /// </summary>
        /// <param name="writeOnDisk">Boolean to indicate if the function must be write on disk</param>
        /// <param name="checkIntegrityBeforeUpdate"></param>
        /// <returns></returns>
        public bool Execute(bool writeOnDisk, bool checkIntegrityBeforeUpdate = false)
        {

            if (writeOnDisk && string.IsNullOrEmpty(FunctionFolderPath))
            {
                Console.WriteLine("Please initialize the Path");
                return false;
            }

            var result = GetFunctions(writeOnDisk);
            if (writeOnDisk)
            {
                return UpdateFunctionsFromDisk(checkIntegrityBeforeUpdate);
            }
            else
            {
                return UpdateFunctions(result, checkIntegrityBeforeUpdate);
            }

        }

        private IEnumerable<MongoDbFunction> GetFunctions(bool writeOnDisk = false)
        {
            IEnumerable<MongoDbFunction> functions = _mongoCommand.GetFunctions();
            if (writeOnDisk)
            {
                foreach (var item in functions)
                {
                    string path = Path.Combine(FunctionFolderPath, Path.ChangeExtension(item.Name, "js"));
                    File.WriteAllText(path, item.Code);
                    Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Function {item.Name} wrote on disk : {path}");
                }
            }
            return functions;
        }

        private bool UpdateFunctionsFromDisk(bool checkIntegrityBeforeUpdate = false)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(FunctionFolderPath);
            FileInfo[] files = directoryInfo.GetFiles("*.js");

            IEnumerable<MongoDbFunction> mongoFunctions = new List<MongoDbFunction>();

            mongoFunctions = files.Select(file => new MongoDbFunction
            {
                Code = File.ReadAllText(file.FullName),
                Name = Path.GetFileNameWithoutExtension(file.Name)
            });

            bool result = _mongoCommand.UpdateFunctions(mongoFunctions, checkIntegrityBeforeUpdate);
            return result;
        }

        private bool UpdateFunctions(IEnumerable<MongoDbFunction> mongoFunctions, bool checkIntegrityBeforeUpdate = false)
        {

            var result = _mongoCommand.UpdateFunctions(mongoFunctions, checkIntegrityBeforeUpdate);
            if (!result)
            {
                return false;
            }

            return true;
        }

    }
}
