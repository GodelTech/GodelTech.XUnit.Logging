using System;
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

        private static readonly Action<ILogger, Exception> LogGetListInformationCallback
            = LoggerMessage.Define(
                LogLevel.Information,
                new EventId(0, nameof(GetList)),
                "Get list"
            );

        public IList<string> GetList()
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                LogGetListInformationCallback(
                    _logger,
                    null
                );
            }

            return Items
                .ToList();
        }
    }
}