﻿using Newtonsoft.Json.Linq;
using SolrExpress.Builder;
using SolrExpress.Search;
using SolrExpress.Search.Parameter;
using SolrExpress.Search.Parameter.Validation;
using SolrExpress.Utility;

namespace SolrExpress.Solr5.Search.Parameter
{
    [FieldMustBeIndexedTrue]
    public sealed class SpatialFilterParameter<TDocument> : BaseSpatialFilterParameter<TDocument>, ISearchItemExecution<JObject>
        where TDocument : Document
    {
        private JProperty _result;

        public SpatialFilterParameter(ExpressionBuilder<TDocument> expressionBuilder)
        {
            this.ExpressionBuilder = expressionBuilder;
        }

        public void AddResultInContainer(JObject container)
        {
            var jObj = (JObject)container["params"] ?? new JObject();
            jObj.Add(this._result);
            container["params"] = jObj;
        }

        public void Execute()
        {
            var fieldName = this.ExpressionBuilder.GetFieldName(this.FieldExpression);

            var formule = ParameterUtil.GetSpatialFormule(
                fieldName,
                this.FunctionType,
                this.CenterPoint,
                this.Distance);

            this._result = new JProperty("fq", formule);
        }
    }
}