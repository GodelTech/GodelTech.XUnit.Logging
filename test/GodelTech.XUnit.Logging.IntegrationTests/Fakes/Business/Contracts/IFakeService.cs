using System.Collections.Generic;

namespace GodelTech.XUnit.Logging.IntegrationTests.Fakes.Business.Contracts
{
    public interface IFakeService
    {
        IList<string> GetList();
    }
}