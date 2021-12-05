using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;

namespace GodelTech.XUnit.Logging.IntegrationTests
{
    public class AppTestFixture : WebApplicationFactory<TestStartup>
    {
        public ITestOutputHelper Output { get; set; }

        public ITestLoggerContextAccessor TestLoggerContextAccessor { get; } = new TestLoggerContextAccessor();

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host
                .CreateDefaultBuilder()
                .ConfigureWebHostDefaults(
                    x =>
                    {
                        x.UseStartup<TestStartup>()
                            .UseTestServer()
                            .ConfigureTestLogging(Output, TestLoggerContextAccessor);
                    }
                );

            return builder;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());

            return base.CreateHost(builder);
        }
    }
}