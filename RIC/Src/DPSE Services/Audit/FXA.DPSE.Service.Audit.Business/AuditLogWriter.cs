using FXA.DPSE.Framework.Serilog.Sinks.MSSqlServer;
using FXA.DPSE.Service.Audit.Common.Configuration;
using Serilog;

namespace FXA.DPSE.Service.Audit.Business
{
    public class AuditLogWriter : IAuditLogWriter
    {
        private readonly ILogger _logger;
        private readonly IAuditCustomConfig _config;

        public AuditLogWriter(IAuditCustomConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public void Log(string message, params object[] propertyValues)
        {
            var connectionString = _config.AuditConnectionStringElement.Value;
            var tableName = _config.TableNameElement.Value;

            var logger = new LoggerConfiguration()
                .WriteTo
                .MSSqlServer(connectionString, tableName)
                .CreateLogger();

            logger.Information(message, propertyValues);
        }
    }
}