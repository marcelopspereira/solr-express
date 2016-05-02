﻿using SolrExpress.Core.Query.Parameter;
using System.Collections.Generic;

namespace SolrExpress.Solr4.Query.Parameter
{
    public sealed class AnyParameter : IAnyParameter, IParameter<List<string>>
    {
        private string _name;
        private string _value;

        /// <summary>
        /// True to indicate multiple instances of the parameter, otherwise false
        /// </summary>
        public bool AllowMultipleInstances { get; } = true;

        /// <summary>
        /// Execute the creation of the parameter
        /// </summary>
        /// <param name="container">Container to parameters to request to SOLR</param>
        public void Execute(List<string> container)
        {
            container.Add($"{this._name}={this._value}");
        }

        /// <summary>
        /// Configure current instance
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        public IAnyParameter Configure(string name, string value)
        {
            this._name = name;
            this._value = value;

            return this;
        }
    }
}