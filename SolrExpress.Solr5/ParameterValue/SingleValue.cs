﻿using SolrExpress.Core.Helper;
using SolrExpress.Core.Query;
using System;
using System.Linq.Expressions;

namespace SolrExpress.Solr5.ParameterValue
{
    /// <summary>
    /// Single value parameter
    /// </summary>
    public sealed class SingleValue<TDocument> : IQueryParameterValue
        where TDocument : IDocument
    {
        private readonly string _value;

        /// <summary>
        /// Create a single solr parameter value
        /// </summary>
        /// <param name="expression">Expression used to find the property name</param>
        /// <param name="value">Value of the filter</param>
        public SingleValue(Expression<Func<TDocument, object>> expression, string value)
        {
            var fieldName = UtilHelper.GetPropertyNameFromExpression(expression);

            this._value = string.Concat(fieldName, ":", value);
        }

        /// <summary>
        /// Execute parameter value generator
        /// </summary>
        /// <returns>Result of the value generator</returns>
        public string Execute()
        {
            return _value;
        }
    }
}
