using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GodelTech.XUnit.Logging.IntegrationTests.Fakes.Business;
using GodelTech.XUnit.Logging.IntegrationTests.Fakes.Business.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace GodelTech.XUnit.Logging.IntegrationTests.DependencyInjection
{
    public sealed class LoggingExtensionsTests : IDisposable
    {
        private static readonly string[] FakeJsonStrings = new string[]
        {
            "null",
            "\"\"",
            "\"Test Value\""
        };

        private readonly AppTestFixture _fixture;

        public LoggingExtensionsTests(ITestOutputHelper output)
        {
            _fixture = new AppTestFixture();
            _fixture.Output = output;
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
        public async Task UseTestLogging_WhenList_Success()
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

            var a = _fixture.TestLoggerContextAccessor.TestLoggerContext.Entries;
        }
    }
}