﻿using SolrExpress.Search.Query;

namespace SolrExpress.Search.Parameter.Extension
{
    /// <summary>
    /// Extensions to configure boost parameter
    /// </summary>
    public static class IBoostParameterExtension
    {
        /// <summary>
        /// Configure query used to make boost
        /// </summary>
		/// <param name="parameter">Parameter to configure</param>
        /// <param name="query">Query used to make boost</param>
        public static IBoostParameter<TDocument> Query<TDocument>(this IBoostParameter<TDocument> parameter, SearchQuery<TDocument> query)
            where TDocument : Document
        {
            parameter.Query = query;

            return parameter;
        }

        /// <summary>
        /// Configure boost type used in calculation
        /// </summary>
		/// <param name="parameter">Parameter to configure</param>
        /// <param name="value">Boost type used in calculation</param>
        public static IBoostParameter<TDocument> BoostFunctionType<TDocument>(this IBoostParameter<TDocument> parameter, BoostFunctionType value)
            where TDocument : Document
        {
            parameter.BoostFunctionType = value;

            return parameter;
        }
    }
}