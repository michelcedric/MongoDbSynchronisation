using MongoDB.Bson;
using MongoDB.Driver;
using MongoSync.MongoDb.Function.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoSync.MongoDb
{
    /// <summary>
    /// Command to manipulate mongo DB
    /// </summary>
    public class MongoDbCommand
    {
        private MongoDbDataContext _context;

        /// <summary>
        /// Initialize the command to manipulate mongo document
        /// </summary>
        /// <param name="connectionStringSourceUrl"></param>
        /// <param name="connectionStringTargetUrl"></param>
        public MongoDbCommand(string connectionStringSourceUrl, string connectionStringTargetUrl)
        {
            _context = new MongoDbDataContext(connectionStringSourceUrl, connectionStringTargetUrl);
        }

        /// <summary>
        /// Initialize the command to manipulate mongo document
        /// </summary>
        public MongoDbCommand()
        {
            _context = new MongoDbDataContext();
        }

        /// <summary>
        /// Get all function from source
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MongoDbFunction> GetFunctions()
        {
            Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Get function from source started...");
            IMongoCollection<BsonDocument> sysJs = _context.SourceMongoDbDatabase.GetCollection<BsonDocument>("system.js");
            List<BsonDocument> documents = sysJs.Find(Builders<BsonDocument>.Filter.Empty).ToList();
            List<MongoDbFunction> mongoFunctions = new List<MongoDbFunction>();

            mongoFunctions.AddRange(documents.Select(document => new MongoDbFunction()
            {
                Name = document.Elements.FirstOrDefault().Value.AsString,
                Code = document.Elements.Skip(1).Take(1).FirstOrDefault().Value.AsBsonJavaScript.Code
            }));

            Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Get function from source ended");
            return mongoFunctions;
        }

        /// <summary>
        /// Update or create multiple functions
        /// </summary>
        /// <param name="mongoFunctions">The list of function</param>
        /// <param name="checkIntegrityBeforeUpdate">Boolean indicate if a check of function on source must be done before update</param>
        /// <returns>Return true if the function updated success otherwise false</returns>
        public bool UpdateFunctions(IEnumerable<MongoDbFunction> mongoFunctions, bool checkIntegrityBeforeUpdate = false)
        {
            if (VerifyScripts(checkIntegrityBeforeUpdate))
            {
                foreach (MongoDbFunction mongoFunction in mongoFunctions)
                {
                    bool result = UpdateFunction(mongoFunction);
                    if (!result) break;
                }
            }
            return true;
        }

        private bool UpdateFunction(MongoDbFunction mongoFunction, bool checkIntegrityBeforeUpdate = false)
        {
            if (VerifyScripts(checkIntegrityBeforeUpdate))
            {
                IMongoCollection<BsonDocument> sysJs = _context.TargetMongoDbDatabase.GetCollection<BsonDocument>("system.js");

                BsonDocument codeDocument = new BsonDocument("_id", mongoFunction.Name)
                {
                    new BsonElement("value", new BsonJavaScript(mongoFunction.Code))
                };

                ReplaceOneResult result = sysJs.ReplaceOne(Builders<BsonDocument>.Filter.Eq("_id", mongoFunction.Name), codeDocument, new UpdateOptions { IsUpsert = true });
                if (result.UpsertedId == null)
                {
                    Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Function : {mongoFunction.Name} updated in mongoDb");
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Function : {mongoFunction.Name} created in mongoDb");
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool VerifyScripts(bool checkIntegrityBeforeUpdate)
        {
            if (checkIntegrityBeforeUpdate)
            {
                try
                {
                    JsonCommand<BsonDocument> loadServerScripts = new JsonCommand<BsonDocument>("{ eval: 'db.loadServerScripts()' }");
                    _context.SourceMongoDbDatabase.RunCommand(loadServerScripts);
                    Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Script on source valid");
                }
                catch (MongoCommandException exception)
                {
                    Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Script on source invalid");
                    Console.WriteLine(exception.ErrorMessage);
                    return false;
                }
            }

            return true;
        }
    }
}

