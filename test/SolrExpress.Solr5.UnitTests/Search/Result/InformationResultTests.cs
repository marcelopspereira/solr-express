﻿using Moq;
using Newtonsoft.Json;
using SolrExpress.Search.Parameter;
using SolrExpress.Search.Result;
using SolrExpress.Solr5.Search.Result;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SolrExpress.Solr5.UnitTests.Search.Result
{
    public class InformationResultTests
    {
        /// <summary>
        /// Where   Using a InformationResult instance
        /// When    Invoking method "Execute" using happy path configurations
        /// What    Create correct Information instance with data provided by SOLR result
        /// </summary>
        [Fact]
        public void InformationResultFact001()
        {
            // Arrange
            const string jsonPlainText = @"
            {
              ""responseHeader"": {
                ""status"": 0,
                ""QTime"": 111
              },
              ""response"": {
                ""numFound"": 222,
                ""start"": 0,
                ""docs"": []
            }
            }";

            var jsonReader = new JsonTextReader(new StringReader(jsonPlainText));

            var searchParameters = new List<ISearchParameter>
            {
                new Mock<IOffsetParameter<TestDocument>>().SetupProperty(q => q.Value, 20).Object,
                new Mock<ILimitParameter<TestDocument>>().SetupProperty(q => q.Value, 10).Object
            };

            var result = (IInformationResult<TestDocument>)new InformationResult<TestDocument>();

            // Act
            while (jsonReader.Read())
            {
                result.Execute(searchParameters, jsonReader.TokenType, jsonReader.Path, jsonReader);
            }

            // Assert
            Assert.Equal(222, result.Data.DocumentCount);
            Assert.Equal(TimeSpan.FromMilliseconds(111), result.Data.ElapsedTime);
            Assert.True(result.Data.HasNextPage);
            Assert.True(result.Data.HasPreviousPage);
            Assert.False(result.Data.IsFirstPage);
            Assert.False(result.Data.IsLastPage);
            Assert.Equal(23, result.Data.PageCount);
            Assert.Equal(3, result.Data.PageNumber);
            Assert.Equal(10, result.Data.PageSize);
            Assert.Null(result.Data.NextCursorMark);
        }

        /// <summary>
        /// Where   Using a InformationResult instance
        /// When    Invoking method "Execute" using ILimitParameter = 0
        /// What    Create correct Information instance with data provided by SOLR result
        /// </summary>
        [Fact]
        public void InformationResultFact002()
        {
            // Arrange
            const string jsonPlainText = @"
            {
              ""responseHeader"": {
                ""status"": 0,
                ""QTime"": 111
              },
              ""response"": {
                ""numFound"": 0,
                ""start"": 0,
                ""docs"": []
            }
            }";

            var jsonReader = new JsonTextReader(new StringReader(jsonPlainText));

            var searchParameters = new List<ISearchParameter>
            {
                new Mock<IOffsetParameter<TestDocument>>().SetupProperty(q => q.Value, 0).Object,
                new Mock<ILimitParameter<TestDocument>>().SetupProperty(q => q.Value, 0).Object
            };

            var result = (IInformationResult<TestDocument>)new InformationResult<TestDocument>();

            // Act
            while (jsonReader.Read())
            {
                result.Execute(searchParameters, jsonReader.TokenType, jsonReader.Path, jsonReader);
            }

            // Assert
            Assert.Equal(0, result.Data.DocumentCount);
            Assert.Equal(TimeSpan.FromMilliseconds(111), result.Data.ElapsedTime);
            Assert.False(result.Data.HasNextPage);
            Assert.False(result.Data.HasPreviousPage);
            Assert.True(result.Data.IsFirstPage);
            Assert.True(result.Data.IsLastPage);
            Assert.Equal(0, result.Data.PageCount);
            Assert.Equal(0, result.Data.PageNumber);
            Assert.Equal(0, result.Data.PageSize);
            Assert.Null(result.Data.NextCursorMark);
        }

        /// <summary>
        /// Where   Using a InformationResult instance
        /// When    Invoking method "Execute" using ICursorMark when nextCursorMark is not equal to cursorMark
        /// What    Create correct Information instance with data provided by SOLR result
        /// </summary>
        [Fact]
        public void InformationResultFact003()
        {
            // Arrange
            const string jsonPlainText = @"
            {
              ""responseHeader"": {
                ""status"": 0,
                ""QTime"": 111
              },
              ""response"": {
                ""numFound"": 222,
                ""start"": 0,
                ""docs"": []
            },
            ""nextCursorMark"": ""AoEoMDU3OUIwMDI=""
            }";

            var jsonReader = new JsonTextReader(new StringReader(jsonPlainText));

            var searchParameters = new List<ISearchParameter>
            {
                new Mock<IOffsetParameter<TestDocument>>().SetupProperty(q => q.Value, 0).Object,
                new Mock<ILimitParameter<TestDocument>>().SetupProperty(q => q.Value, 10).Object,
                new Mock<ICursorMarkParameter>().SetupProperty(q => q.CursorMark, "*").Object
            };

            var result = (IInformationResult<TestDocument>)new InformationResult<TestDocument>();

            // Act
            while (jsonReader.Read())
            {
                result.Execute(searchParameters, jsonReader.TokenType, jsonReader.Path, jsonReader);
            }

            // Assert
            Assert.Equal(222, result.Data.DocumentCount);
            Assert.Equal(TimeSpan.FromMilliseconds(111), result.Data.ElapsedTime);
            Assert.True(result.Data.HasNextPage);
            Assert.Null(result.Data.HasPreviousPage);
            Assert.Null(result.Data.IsFirstPage);
            Assert.False(result.Data.IsLastPage);
            Assert.Null(result.Data.PageCount);
            Assert.Null(result.Data.PageNumber);
            Assert.Equal(10, result.Data.PageSize);
            Assert.Equal("AoEoMDU3OUIwMDI=", result.Data.NextCursorMark);
        }

        /// <summary>
        /// Where   Using a InformationResult instance
        /// When    Invoking method "Execute" using ICursorMark when nextCursorMark is equal to cursorMark
        /// What    Create correct Information instance with data provided by SOLR result
        /// </summary>
        [Fact]
        public void InformationResultFact004()
        {
            // Arrange
            const string jsonPlainText = @"
            {
              ""responseHeader"": {
                ""status"": 0,
                ""QTime"": 111
              },
              ""response"": {
                ""numFound"": 222,
                ""start"": 0,
                ""docs"": []
            },
            ""nextCursorMark"": ""AoEoMDU3OUIwMDI=""
            }";

            var jsonReader = new JsonTextReader(new StringReader(jsonPlainText));

            var searchParameters = new List<ISearchParameter>
            {
                new Mock<IOffsetParameter<TestDocument>>().SetupProperty(q => q.Value, 0).Object,
                new Mock<ILimitParameter<TestDocument>>().SetupProperty(q => q.Value, 10).Object,
                new Mock<ICursorMarkParameter>().SetupProperty(q => q.CursorMark, "AoEoMDU3OUIwMDI=").Object
            };

            var result = (IInformationResult<TestDocument>)new InformationResult<TestDocument>();

            // Act
            while (jsonReader.Read())
            {
                result.Execute(searchParameters, jsonReader.TokenType, jsonReader.Path, jsonReader);
            }

            // Assert
            Assert.Equal(222, result.Data.DocumentCount);
            Assert.Equal(TimeSpan.FromMilliseconds(111), result.Data.ElapsedTime);
            Assert.False(result.Data.HasNextPage);
            Assert.Null(result.Data.HasPreviousPage);
            Assert.Null(result.Data.IsFirstPage);
            Assert.True(result.Data.IsLastPage);
            Assert.Null(result.Data.PageCount);
            Assert.Null(result.Data.PageNumber);
            Assert.Equal(10, result.Data.PageSize);
            Assert.Equal("AoEoMDU3OUIwMDI=", result.Data.NextCursorMark);
        }
    }
}
