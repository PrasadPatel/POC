using Serilog.Configuration;
using Serilog.Sinks.Elasticsearch;

namespace Consume_API.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class SinkExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerCnfg"></param>
        /// <param name="cnfg"></param>
        /// <returns></returns>
        public static LoggerConfiguration ConfigureElasticSink(this LoggerConfiguration loggerCnfg, IConfigurationRoot cnfg)
        {
            return loggerCnfg.WriteTo.Elasticsearch(ConfigureElasticSinkOptions(cnfg));
        }
        private static ElasticsearchSinkOptions ConfigureElasticSinkOptions(IConfigurationRoot cnfg)
        {
            return new ElasticsearchSinkOptions(new Uri(cnfg["ElasticConfiguration:Uri"]))
            {
                TypeName = null,
                AutoRegisterTemplate = Convert.ToBoolean(cnfg["ElasticConfiguration:AutoRegisterTemplate"]),
                IndexFormat = cnfg["ElasticConfiguration:IndexFormat"],
                ModifyConnectionSettings = (c) => c.BasicAuthentication(cnfg["ElasticConfiguration:User"], cnfg["ElasticConfiguration:Pwd"]),
                NumberOfShards = Convert.ToInt16(cnfg["ElasticConfiguration:NumberOfShards"]),
                NumberOfReplicas = Convert.ToInt16(cnfg["ElasticConfiguration:NumberOfReplicas"]),
                EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
                MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information
            };
        }
    }
}
