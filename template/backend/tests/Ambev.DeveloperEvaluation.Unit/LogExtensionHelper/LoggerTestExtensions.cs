using Microsoft.Extensions.Logging;
using Moq;

namespace Ambev.DeveloperEvaluation.Unit.LogExtensionHelper;

public static class LoggerTestExtensions
{
    public static void VerifyLog<T>(
        this Mock<ILogger<T>> loggerMock,
        LogLevel expectedLogLevel,
        string expectedMessage,
        Times? times = null)
    {
        times ??= Times.Once();

        loggerMock.Verify(
            x => x.Log(
                expectedLogLevel,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) =>
                    string.Equals(expectedMessage, o.ToString(), StringComparison.OrdinalIgnoreCase)),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            times.Value);
    }

    public static void VerifyLog<T>(
        this Mock<ILogger<T>> loggerMock,
        LogLevel expectedLogLevel,
        Exception expectedException,
        Times? times = null)
    {
        times ??= Times.Once();

        loggerMock.Verify(
            x => x.Log(
                expectedLogLevel,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                expectedException,
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            times.Value);
    }
}
