using System;
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

        private static readonly Action<ILogger, Exception> LogGetListDebugCallback
            = LoggerMessage.Define(
                LogLevel.Debug,
                new EventId(0, nameof(GetList)),
                "Start request"
            );

        private static readonly Action<ILogger, Exception> LogGetListInformationCallback
            = LoggerMessage.Define(
                LogLevel.Information,
                new EventId(0, nameof(GetList)),
                "Get items from service"
            );

        [HttpGet]
        [ProducesResponseType(typeof(IList<string>), StatusCodes.Status200OK)]
        public IActionResult GetList()
        {
            LogGetListDebugCallback(
                _logger,
                null
            );

            LogGetListInformationCallback(
                _logger,
                null
            );
            var list = _service.GetList();

            return Ok(list);
        }
    }
}