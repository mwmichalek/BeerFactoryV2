using System;

using Skooter.Core.Services;

using Xunit;

namespace Skooter.Core.Tests.XUnit
{
    // TODO WTS: Add appropriate unit tests.
    public class Tests
    {
        [Fact]
        public void Test1()
        {
        }

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Fact]
        public void EnsureSampleDataServiceReturnsContentGridData()
        {
            var dataService = new SampleDataService();
            var actual = dataService.GetContentGridData();

            Assert.NotEmpty(actual);
        }
    }
}
