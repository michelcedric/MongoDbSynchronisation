using Elia.Soroban.Infrastructure.Mongo.Models;
using MongoDB.Extensions.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class PerformanceRootObject : Entity
    {
        public string MyProperty1 { get; set; }
        [BsonIndex]
        public Guid MyProperty2 { get; set; }
        public Guid MyProperty3 { get; set; }

        [BsonIndex]
        public Guid MyProperty4 { get; set; }

        public List<PerformanceChildObject> Childs { get; set; }
    }
}
