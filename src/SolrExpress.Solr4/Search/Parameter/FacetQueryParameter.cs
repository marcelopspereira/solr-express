﻿using SolrExpress.Search;
using SolrExpress.Search.Parameter;
using SolrExpress.Search.Parameter.Validation;
using SolrExpress.Utility;
using System.Collections.Generic;

namespace SolrExpress.Solr4.Search.Parameter
{
    [AllowMultipleInstances]
    // TODO: Think about this, no implements ISearchItemFieldExpressions<> or ISearchItemFieldExpression<>
    //[FieldMustBeIndexedTrue]
    public sealed class FacetQueryParameter<TDocument> : BaseFacetQueryParameter<TDocument>, ISearchItemExecution<List<string>>
        where TDocument : Document
    {
        private readonly List<string> _result = new List<string>();

        public FacetQueryParameter(ISolrExpressServiceProvider<TDocument> serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public void AddResultInContainer(List<string> container)
        {
            if (!container.Contains("facet=true"))
            {
                container.Add("facet=true");
            }

            container.AddRange(this._result);
        }

        public void Execute()
        {
            Checker.IsNull(this.Query);
            Checker.IsTrue<UnsupportedFeatureException>(this.Filter != null);

            var query = this.Query.Execute();

            this._result.Add($"facet.query={ParameterUtil.GetFacetName(this.Excludes, this.AliasName, query)}");

            if (this.SortType.HasValue)
            {
                Checker.IsTrue<UnsupportedFeatureException>(this.SortType.Value == FacetSortType.CountDesc || this.SortType.Value == FacetSortType.IndexDesc);

                ParameterUtil.GetFacetSort(this.SortType.Value, out string typeName, out string dummy);

                this._result.Add($"f.{this.AliasName}.facet.sort={typeName}");
            }

            if (this.Minimum.HasValue)
            {
                this._result.Add($"f.{this.AliasName}.facet.mincount={this.Minimum.Value}");
            }

            if (this.Limit.HasValue)
            {
                this._result.Add($"f.{this.AliasName}.facet.limit={this.Limit.Value}");
            }
        }
    }
}
