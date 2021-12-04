using GodelTech.XUnit.Logging.IntegrationTests.Fakes.Business.Contracts;
using System.Collections.Generic;
using System.Linq;

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

        public IList<string> GetList()
        {
            return Items
                .ToList();
        }
    }
}