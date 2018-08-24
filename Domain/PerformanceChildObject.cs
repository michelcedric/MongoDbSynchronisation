using Elia.Soroban.Infrastructure.Mongo.Models;
using System;

namespace Domain
{
    public class PerformanceChildObject : Entity
    {
        public string MyProperty1 { get; set; }

        public Guid MyProperty2 { get; set; }

        public Guid MyProperty3 { get; set; }
    }
}
