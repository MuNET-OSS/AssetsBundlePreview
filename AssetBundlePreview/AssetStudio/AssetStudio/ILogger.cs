namespace AssetStudio;

public interface ILogger
{
	void Log(LoggerEvent loggerEvent, string message);
}
