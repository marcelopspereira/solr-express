﻿using System;
using System.Linq.Expressions;

namespace SolrExpress.Search.Parameter.Extension
{
    /// <summary>
    /// Extensions to configure in sort parameter
    /// </summary>
    public static class ISortParameterExtension
    {
        /// <summary>
        /// Configure expression used to find field name
        /// </summary>
		/// <param name="parameter">Parameter to configure</param>
        /// <param name="value">Expressions used to find field name</param>
        public static ISortParameter<TDocument> FieldExpression<TDocument>(this ISortParameter<TDocument> parameter, Expression<Func<TDocument, object>> value)
            where TDocument : Document
        {
            parameter.FieldExpression = value;

            return parameter;
        }

        /// <summary>
        /// Configure true to ascendent order, otherwise false
        /// </summary>
		/// <param name="parameter">Parameter to configure</param>
        /// <param name="value">True to ascendent order, otherwise false</param>
        public static ISortParameter<TDocument> Ascendent<TDocument>(this ISortParameter<TDocument> parameter, bool value)
            where TDocument : Document
        {
            parameter.Ascendent = value;

            return parameter;
        }
    }
}
