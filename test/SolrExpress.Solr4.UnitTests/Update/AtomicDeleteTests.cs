﻿using Newtonsoft.Json.Linq;
using SolrExpress.Solr4.Update;
using SolrExpress.Update;
using Xunit;

namespace SolrExpress.Solr4.UnitTests.Update
{
    public class AtomicDeleteTests
    {
        /// <summary>
        /// Where   Using a AtomicDelete instance
        /// When    Invoking method "Execute"
        /// What    Create a valid JSON
        /// </summary>
        [Fact]
        public void AtomicDelete001()
        {
            // Arrange
            var expected = JObject.Parse(@"
            {
                ""delete"": ""123456"",
                ""commit"": {}
            }");
            var atomic = (IAtomicDelete)new AtomicDelete();

            // Act
            var actual = atomic.Execute("123456");

            // Assert
            Assert.Equal(expected.ToString(), actual.ToString());
        }

        /// <summary>
        /// Where   Using a AtomicDelete instance
        /// When    Invoking method "Execute" without any document
        /// What    Create a string.empty
        /// </summary>
        [Fact]
        public void AtomicDelete002()
        {
            // Arrange
            var atomic = (IAtomicDelete)new AtomicDelete();

            // Act
            var actual = atomic.Execute();

            // Assert
            Assert.Null(actual);
        }
    }
}
