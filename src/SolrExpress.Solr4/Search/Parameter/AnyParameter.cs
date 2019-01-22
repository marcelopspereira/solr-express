﻿using SolrExpress.Search;
using SolrExpress.Search.Parameter;
using SolrExpress.Search.Parameter.Validation;
using System.Collections.Generic;

namespace SolrExpress.Solr4.Search.Parameter
{
    [AllowMultipleInstances]
    [UseAnyThanSpecificParameterRather]
    public sealed class AnyParameter : BaseAnyParameter, ISearchItemExecution<List<string>>
    {
        private string _result;

        public void AddResultInContainer(List<string> container)
        {
            container.Add(this._result);
        }

        public void Execute()
        {
            this._result = $"{this.Name}={this.Value}";
        }
    }
}
