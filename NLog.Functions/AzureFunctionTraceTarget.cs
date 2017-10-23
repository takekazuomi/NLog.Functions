/* AzureFunctionTraceTarget.cs
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
using System.ComponentModel;
using Microsoft.Azure.WebJobs.Host;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace NLog.Functions
{
    /// <summary>
    /// Writes log messages to the Azure Function TraceWriter.(not ILogger)
    /// </summary>
    [Target("AzureFunctionTrace")]
    public sealed class AzureFunctionTraceTarget : TargetWithLayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NLog.Targets.TargetWithLayout"/> class.
        /// </summary>
        /// <remarks>
        /// The default value of the layout is: 
        /// <code>
        /// ${level:uppercase=true}|${logger}|${message}
        /// </code>
        /// </remarks>
        public AzureFunctionTraceTarget(TraceWriter traceWriter)
        {
            TraceWriter = traceWriter;
            Layout = "${level:uppercase=true}|${logger}|${message}";
        }

        [RequiredParameter]
        public TraceWriter TraceWriter { get; set; }

        /// <summary>
        /// Gets or sets the layout used to format log messages.
        /// </summary>
        /// <docgen category='Layout Options' order='1' />
        [RequiredParameter]
        [DefaultValue("${level:uppercase=true}|${logger}|${message}")]
        public override Layout Layout { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = Layout.Render(logEvent);

            if (logEvent.Level == LogLevel.Trace)
                TraceWriter.Verbose(logMessage);
            else if (logEvent.Level == LogLevel.Debug)
                TraceWriter.Verbose(logMessage);
            else if (logEvent.Level == LogLevel.Info)
                TraceWriter.Info(logMessage);
            else if (logEvent.Level == LogLevel.Warn)
                TraceWriter.Warning(logMessage);
            else if (logEvent.Level == LogLevel.Error)
                TraceWriter.Error(logMessage);
            else if (logEvent.Level == LogLevel.Fatal)
                TraceWriter.Error(logMessage);
            // off
        }
    }
}