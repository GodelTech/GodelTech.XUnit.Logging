using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GodelTech.XUnit.Logging.IntegrationTests.Fakes.Business;
using GodelTech.XUnit.Logging.IntegrationTests.Fakes.Business.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace GodelTech.XUnit.Logging.IntegrationTests.DependencyInjection
{
    public sealed class WebHostBuilderExtensionsTests : IDisposable
    {
        private static readonly string[] FakeJsonStrings = new string[]
        {
            "null",
            "\"\"",
            "\"Test Value\""
        };

        private readonly AppTestFixture _fixture;

        public WebHostBuilderExtensionsTests(ITestOutputHelper output)
        {
            _fixture = new AppTestFixture
            {
                Output = output
            };
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }

        private HttpClient CreateClient()
        {
            return _fixture
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureServices(
                                services =>
                                {
                                    services.AddTransient<IFakeService, FakeService>();

                                    services.AddControllers();
                                }
                            );

                        builder
                            .Configure(
                                app =>
                                {
                                    app.UseRouting();

                                    app.UseEndpoints(
                                        endpoints =>
                                        {
                                            endpoints.MapControllers();
                                        }
                                    );
                                }
                            );
                    }
                )
                .CreateClient();
        }

        [Fact]
        public void ConfigureTestLogging_Registered()
        {
            // Arrange & Act & Assert
            var services = _fixture.Services.GetServices<ILoggerProvider>();

            var service = Assert.Single(services);
            Assert.IsType<TestLoggerProvider>(service);
        }

        [Fact]
        public async Task ConfigureTestLogging_Success()
        {
            // Arrange
            var client = CreateClient();

            // Act
            var result = await client.GetAsync(
                new Uri(
                    "/fakes",
                    UriKind.Relative
                )
            );

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(
                "[" + string.Join(',', FakeJsonStrings) + "]",
                await result.Content.ReadAsStringAsync()
            );

            var logs = _fixture
                .TestLoggerContextAccessor
                .TestLoggerContext
                .Entries;

            var controllerLog = Assert.Single(
                logs.Where(
                    x =>
                        x.CategoryName ==
                        "GodelTech.XUnit.Logging.IntegrationTests.Fakes.Controllers.FakeController"
                )
            );
            Assert.Equal(
                "Get items from service",
                controllerLog.Message
            );

            var serviceLog = Assert.Single(
                logs.Where(
                    x =>
                        x.CategoryName ==
                        "GodelTech.XUnit.Logging.IntegrationTests.Fakes.Business.FakeService"
                )
            );
            Assert.Equal(
                "Get list",
                serviceLog.Message
            );
        }
    }
}
