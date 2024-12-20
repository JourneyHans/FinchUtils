using FinchUtils.Common.Singleton;

namespace FinchUtils.Debugger;

public interface ILogger {
    void Log(string message, params object[] args);
    void LogT(string tag, string message, params object[] args);
    void Warning(string message, params object[] args);
    void Error(string message, params object[] args);
    void Assert(bool condition, string message);
}

public class Log : ManagedSingleton<Log> {
    private static ILogger _logger;

    public void RegisterLogger(ILogger logger) {
        _logger = logger;
    }

    protected override void OnDestroy() {
        _logger = null;
    }

    #region ILogger

    public void Debug(string message, params object[] args) {
        _logger.Log(message, args);
    }

    public void LogT(string tag, string message, params object[] args) {
        _logger.LogT(tag, message, args);
    }

    public void Warning(string log, params object[] args) {
        _logger.Warning(log, args);
    }

    public void Error(string log, params object[] args) {
        _logger.Error(log, args);
    }

    public void Assert(bool condition, string message) {
        _logger.Assert(condition, message);
    }

    #endregion
}