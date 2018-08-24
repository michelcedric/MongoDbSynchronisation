namespace MongoSync.MongoDb.Function.Model
{
    /// <summary>
    /// Represent a function of mongo DB
    /// </summary>
    public class MongoDbFunction
    {
        /// <summary>
        /// Gets or sets the name of function
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code of the function
        /// </summary>
        public string Code { get; set; }
    }
}
