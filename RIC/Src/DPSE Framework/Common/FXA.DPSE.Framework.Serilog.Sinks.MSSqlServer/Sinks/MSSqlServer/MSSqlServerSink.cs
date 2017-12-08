// Copyright 2013 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace FXA.DPSE.Framework.Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer
{
    /// <summary>
    ///     Writes log events as rows in a table of MSSqlServer database.
    /// </summary>
    public class MsSqlServerSink : PeriodicBatchingSink
    {
        /// <summary>
        ///     A reasonable default for the number of events posted in
        ///     each batch.
        /// </summary>
        public const int DefaultBatchPostingLimit = 50;

        /// <summary>
        ///     A reasonable default time to wait between checking for event batches.
        /// </summary>
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(5);

        readonly string _connectionString;

        DataTable _eventsTable;
        readonly IFormatProvider _formatProvider;
        readonly bool _includeProperties;
        readonly string _tableName;
        readonly CancellationTokenSource _token = new CancellationTokenSource();
        readonly bool _storeTimestampInUtc;

        /// <summary>
        ///     Construct a sink posting to the specified database.
        /// </summary>
        /// <param name="connectionString">Connection string to access the database.</param>
        /// <param name="tableName">Name of the table to store the data in.</param>
        /// <param name="includeProperties">Specifies if the properties need to be saved as well.</param>
        /// <param name="batchPostingLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="storeTimestampInUtc">Store Timestamp In UTC</param>
        public MsSqlServerSink(string connectionString, string tableName, bool includeProperties, int batchPostingLimit,
            TimeSpan period, IFormatProvider formatProvider, bool storeTimestampInUtc)
            : base(batchPostingLimit, period)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException("tableName");

            _connectionString = connectionString;
            _tableName = tableName;
            _includeProperties = includeProperties;
            _formatProvider = formatProvider;
            _storeTimestampInUtc = storeTimestampInUtc;

            // Prepare the data table
            //_eventsTable = CreateDataTable();
        }

        /// <summary>
        ///     Emit a batch of log events, running asynchronously.
        /// </summary>
        /// <param name="events">The events to emit.</param>
        /// <remarks>
        ///     Override either <see cref="PeriodicBatchingSink.EmitBatch" /> or <see cref="PeriodicBatchingSink.EmitBatchAsync" />
        ///     ,
        ///     not both.
        /// </remarks>
        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            try
            {
                var logEvents = events as IList<LogEvent> ?? events.ToList();
                var logEvent = logEvents.ToList().FirstOrDefault();

                if (logEvent != null)
                {
                    // Prepare the data table
                    if (_eventsTable == null)
                    {
                        _eventsTable = CreateDataTable(logEvent);
                    }

                    foreach (var eventToLog in logEvents)
                    {
                        // Copy the events to the data table
                        FillDataTable(eventToLog);                        
                    }

                    using (var cn = new SqlConnection(_connectionString))
                    {
                        await cn.OpenAsync(_token.Token);
                        using (var copy = new SqlBulkCopy(cn))
                        {
                            copy.DestinationTableName = _tableName;
                            await copy.WriteToServerAsync(_eventsTable, _token.Token);

                            // Processed the items, clear for the next run
                            _eventsTable.Clear();
                        }
                    }    
                }
            }
            catch (Exception e)
            {
                var message = e.Message;
                throw;
            }
        }

        private DataTable CreateDataTable(LogEvent logEvent)
        {
            var eventsTable = new DataTable(_tableName);

            var id = new DataColumn
            {
                DataType = Type.GetType("System.Int32"),
                ColumnName = "Id",
                AutoIncrement = true
            };

            eventsTable.Columns.Add(id);

            foreach (var property in logEvent.Properties)
            {
                var col = new DataColumn
                {
                    DataType = Type.GetType("System.String"),
                    ColumnName = property.Key
                };

                eventsTable.Columns.Add(col);
            }

            // Create an array for DataColumn objects.
            var keys = new DataColumn[1];
            keys[0] = id;
            eventsTable.PrimaryKey = keys;

            return eventsTable;
        }

        void FillDataTable(LogEvent logEvent)
        {
            // Add the new rows to the collection. 
            var row = _eventsTable.NewRow();

            if (_includeProperties)
            {
                foreach (var property in logEvent.Properties)
                {
                    var key = property.Key;
                    var propValue = property.Value.ToString();

                    if (!string.IsNullOrEmpty(propValue) && propValue.StartsWith("\"") && propValue.EndsWith("\""))
                    {
                        propValue = propValue.Substring(1, propValue.Length - 2);
                        
                        if (string.IsNullOrEmpty(propValue)) 
                            propValue = null;
                    }

                    row[key] = propValue;                    
                }
            }

            _eventsTable.Rows.Add(row);
            _eventsTable.AcceptChanges();
        }

        protected override void Dispose(bool disposing)
        {
            _token.Cancel();

            if (_eventsTable != null)
                _eventsTable.Dispose();

            base.Dispose(disposing);
        }
    }
}