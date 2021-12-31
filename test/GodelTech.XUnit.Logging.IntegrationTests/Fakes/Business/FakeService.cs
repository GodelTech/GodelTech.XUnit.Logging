using GodelTech.XUnit.Logging.IntegrationTests.Fakes.Business.Contracts;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace GodelTech.XUnit.Logging.IntegrationTests.Fakes.Business
{
    public class FakeService : IFakeService
    {
        private static readonly IReadOnlyList<string> Items = new List<string>
        {
            null,
            string.Empty,
            "Test Value"
        };

        private readonly ILogger<FakeService> _logger;

        public FakeService(ILogger<FakeService> logger)
        {
            _logger = logger;
        }

        public IList<string> GetList()
        {
            _logger.LogInformation("Get list");

            return Items
                .ToList();
        }
    }
}