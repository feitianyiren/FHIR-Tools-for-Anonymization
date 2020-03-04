﻿using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fhir.Anonymizer.Core.UnitTests
{
    public class FhirPartitionedExecutorTests
    {
        [Fact] 
        public async Task GivenAPartitionedExecutor_WhenExecute_ResultShouldBeReturnedInOrder()
        {
            int itemCount = 9873;
            var testConsumer = new TestFhirDataConsumer(itemCount);
            FhirPartitionedExecutor executor = new FhirPartitionedExecutor()
            {
                AnonymizedDataConsumer = testConsumer,
                RawDataReader = new TestFhirDataReader(itemCount),
                AnonymizerFunction = (content) => content,
                BatchSize = 100
            };

            await executor.ExecuteAsync();

            Assert.Equal(itemCount, testConsumer.CurrentOffset);
            Assert.Equal(99, testConsumer.BatchCount);
        }
    }

    internal class TestFhirDataConsumer : IFhirDataConsumer
    {
        public int ItemCount { set; get; }

        public int BatchSize { set; get; }

        public int CurrentOffset { set; get; } = 0;

        public int BatchCount { set; get; } = 0;

        public TestFhirDataConsumer(int itemCount)
        {
            ItemCount = itemCount;
            CurrentOffset = 0;
            BatchCount = 0;
        }

        public async Task ConsumeAsync(IEnumerable<string> data)
        {
            BatchCount++;
            foreach (string content in data)
            {
                Assert.Equal(CurrentOffset++.ToString(), content);
            }
        }
    }

    internal class TestFhirDataReader : IFhirDataReader
    {
        public int ItemCount { set; get; }

        private int CurrentOffset { set; get; }

        public TestFhirDataReader(int itemCount)
        {
            ItemCount = itemCount;
            CurrentOffset = 0;
        }

        public bool HasNext()
        {
            return ItemCount > CurrentOffset;
        }

        public string Next()
        {
            return (CurrentOffset++).ToString();
        }
    }
}
