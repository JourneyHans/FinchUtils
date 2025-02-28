using System;
using FinchUtils.Common.Singleton;

namespace FinchUtils.Debugger;

public interface ILoggerHandler {
    void Log(string message, params object[] args);
    void LogT(string tag, string message, params object[] args);
    void Warning(string message, params object[] args);
    void Error(string message, params object[] args);
    void Assert(bool condition, string message);
}

public class Logger : NullableSingleton<Logger> {
    private static ILoggerHandler _loggerHandler;

    public void RegisterLogger(ILoggerHandler loggerHandler) {
        _loggerHandler = loggerHandler;
    }

    protected override void OnDestroy() {
        _loggerHandler = null;
    }

    #region ILogger

    public void Log(string message, params object[] args) {
        _loggerHandler.Log(message, args);
    }

    public void Log<T>(string message, params object[] args) {
        _loggerHandler.LogT(typeof(T).Name, message, args);
    }

    public void LogT(string tag, string message, params object[] args) {
        _loggerHandler.LogT(tag, message, args);
    }

    public void Warning(string log, params object[] args) {
        _loggerHandler.Warning(log, args);
    }

    public void Error(string log, params object[] args) {
        _loggerHandler.Error(log, args);
    }

    public void Assert(bool condition, string message) {
        _loggerHandler.Assert(condition, message);
    }

    #endregion
}