﻿using Newtonsoft.Json.Linq;
using SolrExpress.Builder;
using SolrExpress.Search;
using SolrExpress.Search.Parameter;
using SolrExpress.Search.Parameter.Validation;
using SolrExpress.Utility;
using System.Collections.Generic;
using System.Linq;

namespace SolrExpress.Solr5.Search.Parameter
{
    [AllowMultipleInstances]
    [FacetRangeType]
    [FieldMustBeIndexedTrue]
    public sealed class FacetRangeParameter<TDocument> : BaseFacetRangeParameter<TDocument>, ISearchItemExecution<JObject>
        where TDocument : Document
    {
        private JProperty _result;

        public FacetRangeParameter(ExpressionBuilder<TDocument> expressionBuilder, ISolrExpressServiceProvider<TDocument> serviceProvider)
        {
            this.ExpressionBuilder = expressionBuilder;
            this.ServiceProvider = serviceProvider;
        }

        public void AddResultInContainer(JObject container)
        {
            var jObj = (JObject)container["facet"] ?? new JObject();
            jObj.Add(this._result);
            container["facet"] = jObj;
        }

        public void Execute()
        {
            var array = new List<JProperty>
            {
                new JProperty("field", this.ExpressionBuilder.GetFieldName(this.FieldExpression))
            };

            JProperty domain = null;
            if (this.Excludes?.Any() ?? false)
            {
                var excludeValue = new JObject(new JProperty("excludeTags", new JArray(this.Excludes)));
                domain = new JProperty("domain", excludeValue);
            }
            if (this.Filter != null)
            {
                var filter = new JProperty("filter", this.Filter.Execute());
                domain = domain ?? new JProperty("domain", new JObject());
                ((JObject)domain.Value).Add(filter);
            }
            if (domain != null)
            {
                array.Add(domain);
            }

            if (this.Minimum.HasValue)
            {
                array.Add(new JProperty("mincount", this.Minimum.Value));
            }

            if (!string.IsNullOrWhiteSpace(this.Gap))
            {
                array.Add(new JProperty("gap", this.Gap));
            }

            if (!string.IsNullOrWhiteSpace(this.Start))
            {
                array.Add(new JProperty("start", this.Start));
            }

            if (!string.IsNullOrWhiteSpace(this.End))
            {
                array.Add(new JProperty("end", this.End));
            }

            if (this.HardEnd)
            {
                array.Add(new JProperty("hardend", true));
            }

            if (this.CountBefore || this.CountAfter)
            {
                var content = new List<string>();
                if (this.CountBefore)
                {
                    content.Add("before");
                }

                if (this.CountAfter)
                {
                    content.Add("after");
                }

                array.Add(new JProperty("other", new JArray(content.ToArray())));
            }

            if (this.SortType.HasValue)
            {
                ParameterUtil.GetFacetSort(this.SortType.Value, out string typeName, out string sortName);

                array.Add(new JProperty("sort", new JObject(new JProperty(typeName, sortName))));
            }

            this._result = new JProperty(this.AliasName, new JObject(new JProperty("range", new JObject(array.ToArray()))));
        }
    }
}