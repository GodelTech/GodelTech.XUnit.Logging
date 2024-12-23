# GodelTech.XUnit.Logging
GodelTech.XUnit.Logging is a helper library for capturing logs in xUnit tests. It enables seamless integration of logging within xUnit test frameworks, allowing developers to access log entries and utilize them in their tests. 
## Overview

`GodelTech.XUnit.Logging` contains helper for getting logs in xunit testing.

```c#
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
```

## Exmaple of usage

```c#
    public sealed class SomeTests : IDisposable
    {
        private readonly AppTestFixture _fixture;

        public SomeTests(ITestOutputHelper output)
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

        // test methods
    }
```

To access logs entries use:
```c#
_fixture
    .TestLoggerContextAccessor
    .TestLoggerContext
    .Entries;
```
You can check example of usage in Integration Tests for this library here: https://github.com/GodelTech/GodelTech.XUnit.Logging/tree/main/test/GodelTech.XUnit.Logging.IntegrationTests.

# License
This project is licensed under the MIT License. See the LICENSE file for more details.