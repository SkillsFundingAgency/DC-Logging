using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Logging.Config.Extensions;
using ESFA.DC.Logging.Config.Interfaces;
using FluentAssertions;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Xunit;

namespace ESFA.DC.Logging.Tests
{
    public class LoggerConfigurationMsSqlServerSinkExtensionsTests
    {
        [Fact]
        public void ValidateMsSqlServerApplicationLoggerOutputSettings_Valid()
        {
            var outputSettings = new MsSqlServerApplicationLoggerOutputSettings()
            {
                ConnectionString = "NotNull",
                LogsTableName = "NotNull"
            };

            Action validate = () => LoggerConfigurationMsSqlServerSinkExtensions.ValidateMsSqlServerApplicationLoggerOutputSettings(outputSettings);

            validate.Should().NotThrow();
        }

        [Fact]
        public void ValidateMsSqlServerApplicationLoggerOutputSettings_ConnectionString()
        {
            var outputSettings = new MsSqlServerApplicationLoggerOutputSettings()
            {
                ConnectionString = null,
                LogsTableName = "NotNull"
            };

            Action validate = () => LoggerConfigurationMsSqlServerSinkExtensions.ValidateMsSqlServerApplicationLoggerOutputSettings(outputSettings);

            validate.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ValidateMsSqlServerApplicationLoggerOutputSettings_LogsTableName()
        {
            var outputSettings = new MsSqlServerApplicationLoggerOutputSettings()
            {
                ConnectionString = null,
                LogsTableName = "NotNull"
            };

            Action validate = () => LoggerConfigurationMsSqlServerSinkExtensions.ValidateMsSqlServerApplicationLoggerOutputSettings(outputSettings);

            validate.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void WithMsSqlServerSinks_None()
        {
            var configuration = new LoggerConfiguration();

            configuration.WithMsSqlServerSinks(new List<IApplicationLoggerOutputSettings>());
        }

        [Fact]
        public void WithMsSqlServerSinks_One()
        {
            var configuration = new LoggerConfiguration();

            configuration.WithMsSqlServerSinks(new List<IApplicationLoggerOutputSettings>()
            {
                new MsSqlServerApplicationLoggerOutputSettings()
            });
        }

        [Fact]
        public void WithMsSqlServerSinks_Multiple()
        {
            var configuration = new LoggerConfiguration();

            configuration.WithMsSqlServerSinks(new List<IApplicationLoggerOutputSettings>()
            {
                new MsSqlServerApplicationLoggerOutputSettings(),
                new MsSqlServerApplicationLoggerOutputSettings(),
                new MsSqlServerApplicationLoggerOutputSettings(),
            });
        }

        [Fact]
        public void BuildColumnOptions_TimeStamp()
        {
            var columnOptions = LoggerConfigurationMsSqlServerSinkExtensions.BuildColumnOptions();

            columnOptions.TimeStamp.ConvertToUtc.Should().Be(true);
            columnOptions.TimeStamp.ColumnName.Should().Be("TimeStampUTC");
        }

        [Fact]
        public void BuildColumnOptions_Columns()
        {
            var columnOptions = LoggerConfigurationMsSqlServerSinkExtensions.BuildColumnOptions();

            columnOptions.Store.Should().HaveCount(5);
            columnOptions.Store.Should().NotContain(StandardColumn.Properties);

            columnOptions.AdditionalDataColumns.Should().HaveCount(8);
            columnOptions.AdditionalDataColumns
                .Select(c => c.ColumnName)
                .Should()
                .BeEquivalentTo(
                    new List<string>()
                    {
                        "MachineName",
                        "ProcessName",
                        "ThreadId",
                        "CallerName",
                        "SourceFile",
                        "LineNumber",
                        "JobId",
                        "TaskKey",
                    });
        }
    }
}
