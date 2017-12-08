using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.Audit.DataAccess
{
    public static class ContextExtensions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }

        public static IEnumerable<string> GetTableNames(this DbContext context)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            var tables = metadata.GetItemCollection(DataSpace.SSpace)
                .GetItems<EntityContainer>()
                .Single()
                .BaseEntitySets
                .OfType<EntitySet>()
                .Where(s => !s.MetadataProperties.Contains("Type")
                            || s.MetadataProperties["Type"].ToString() == "Tables");

            var list = new List<string>();
            foreach (var table in tables)
            {
                var tableName = table.MetadataProperties.Contains("Table")
                                && table.MetadataProperties["Table"].Value != null
                    ? table.MetadataProperties["Table"].Value.ToString()
                    : table.Name;
                list.Add(tableName);
            }
            return list;
        }
    }
}