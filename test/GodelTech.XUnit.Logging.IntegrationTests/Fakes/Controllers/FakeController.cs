using System.Collections.Generic;
using GodelTech.XUnit.Logging.IntegrationTests.Fakes.Business.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GodelTech.XUnit.Logging.IntegrationTests.Fakes.Controllers
{
    [ApiController]
    [Route("fakes")]
    public class FakeController : ControllerBase
    {
        private readonly IFakeService _service;
        private readonly ILogger<FakeController> _logger;

        public FakeController(
            IFakeService service,
            ILogger<FakeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<string>), StatusCodes.Status200OK)]
        public IActionResult GetList()
        {
            _logger.LogDebug("Start request");

            _logger.LogInformation("Get items from service");
            var list = _service.GetList();

            return Ok(list);
        }
    }
}