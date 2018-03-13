using System;
using Autofac;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.Logging.Console
{
    public class LogStuff : IDisposable
    {
        //static ILogger _logger = null;
        private readonly ContainerBuilder _containerBuilder;
        private IContainer _container;
        private ILifetimeScope _scope;

        public LogStuff(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        public void   DoStuff()
        {
            ConfigureIoCContainerFromBuilder();

            var logger = _scope.Resolve<ILogger>();

            logger.LogDebug("some debug");
            logger.LogInfo("some info");
            logger.LogWarning("some warn");
            logger.LogError("some error", new Exception("there was an exception"));
        }

        public void ConfigureIoCContainerFromBuilder()
        {
            _container = _containerBuilder.Build();
            _scope = _container.BeginLifetimeScope();
        }

        public void Dispose()
        {
            _container?.Dispose();
            _scope?.Dispose();
        }
    }
}
