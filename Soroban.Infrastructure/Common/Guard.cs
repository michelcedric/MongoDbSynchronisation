using System;
using System.Collections.Generic;
using System.Linq;

namespace Elia.Soroban.Infrastructure.Common
{
    public static class Guard
    {
        //public static void That(bool isValid, string property, string prefix = "INVALID")
        //{
        //    if (!isValid)
        //        throw new BusinessException($"{prefix}_{property}");
        //}

        //public static void ThatAreEqual(string expected, string actual, string property, string prefix = "INVALID")
        //{
        //    var equal = expected.Equals(actual);
        //    That(equal, property, prefix);
        //}

        //public static void Against(bool condition, string property, string prefix = "INVALID")
        //{
        //    if (condition)
        //        throw new BusinessException($"{prefix}_{property}");
        //}

        //public static void AgainstNull(object value, string message)
        //{
        //    if (value == null)
        //        throw new ArgumentNullException($"{message}");
        //}

        /// <summary>
        /// Guard against string to big
        /// </summary>
        public static void AgainstOverSize(string value, int maxSize, string message, string prefix = "OVERSIZE")
        {
            if (value.Length > maxSize)
                throw new OverflowException($"{prefix}_{message}");
        }

        public static void AgainstNull<TProperty>(TProperty value, string message, string prefix = "REQUIRED")
            where TProperty : class
        {
            if (value == null)
                throw new ArgumentNullException($"{prefix}_{message}");
        }

        public static void AgainstDefault<TProperty>(TProperty value, string message, string prefix = "REQUIRED")
            where TProperty : struct
        {
            if (EqualityComparer<TProperty>.Default.Equals(value, default(TProperty)))
                throw new ArgumentNullException($"{prefix}_{message}");
        }

        /// <summary>
        /// Validate that the string is not null of empty
        /// </summary>
        public static void AgainstNullOrEmpty(string value, string property, string prefix = "REQUIRED")
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException($"{prefix}_{property}");
        }

        public static void AgainstNullOrEmpty(Guid value, string property, string prefix = "REQUIRED")
        {
            if (value == null || value == Guid.Empty)
                throw new ArgumentNullException($"{prefix}_{property}");
        }

        public static void AgainstNullOrEmpty(IEnumerable<object> value, string property, string prefix = "REQUIRED")
        {
            if (value == null || !value.Any())
                throw new ArgumentNullException($"{prefix}_{property}");
        }
    }
}