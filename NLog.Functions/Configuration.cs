/* Configuration.cs
 * 
 * Copyright 2017 Takekazu Omi
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using Microsoft.Azure.WebJobs.Host;
using NLog.Config;

// 
// https://github.com/NLog/NLog.Web/blob/master/NLog.Web.AspNetCore/Targets/AspNetTrace.cs
namespace NLog.Functions
{
    // https://github.com/yorek/AzureFunctionNLog
    // https://github.com/NLog/NLog/wiki/Configuration-API
    public static class Configuration
    {
        private static readonly Object Locker = new Object();

        public static void Initialize(TraceWriter log)
        {
            if (LogManager.Configuration != null)
                return;

            lock (Locker)
            {
                if (LogManager.Configuration != null)
                    return;

                // Configure NLog programmatically
                var config = new LoggingConfiguration();

                // Add the AzureFuctionLogTarget Target
                var azureTarget = new AzureFunctionTraceTarget(log);
                config.AddTarget("azure", azureTarget);

                var loglevel = Environment.GetEnvironmentVariable("NLOG_LOGLEVEL");
                if (string.IsNullOrEmpty(loglevel))
                    loglevel = "Error";

                var rule1 = new LoggingRule("*", LogLevel.FromString(loglevel), azureTarget);
                config.LoggingRules.Add(rule1);

                // Assign the newly created configuration to the active LogManager NLog instance
                LogManager.Configuration = config;
            }
        }
    }
}
