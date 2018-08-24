using AutoMapper;
using System.Collections.Generic;

namespace Elia.Soroban.Infrastructure.Mapping
{
    /// <summary>
    /// Manage extensions method to use mapping
    /// </summary>
    public static class MapExtensions
    {
        /// <summary>
        /// Map a list objet to TDestination
        /// </summary>
        /// <typeparam name="TDestination">The type of object you want mapped</typeparam>
        /// <param name="list">The list object to map</param>
        /// <returns></returns>
        public static IEnumerable<TDestination> Map<TDestination>(this IEnumerable<object> list)
        {
            return Mapper.Map<IEnumerable<TDestination>>(list);
        }

        /// <summary>
        /// Map an object
        /// </summary>
        /// <typeparam name="TDestination">The type of object you want mapped</typeparam>
        /// <param name="objectToMap"></param>
        /// <returns></returns>
        public static TDestination Map<TDestination>(this object objectToMap)
        {
            return Mapper.Map<TDestination>(objectToMap);
        }
    }
}
