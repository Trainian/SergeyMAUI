using Xunit;
using Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Tests.Services
{
    public class ScanApiServiceTests
    {
        private ScanApiService _service;

        public ScanApiServiceTests()
        {
            _service = new ScanApiService();
        }

        [Fact()]
        public async Task TestConnectionToApi_Connected()
        {
            var result = await _service.TestConnectionToApiAsync();

            Assert.True(result);
        }

    }
}