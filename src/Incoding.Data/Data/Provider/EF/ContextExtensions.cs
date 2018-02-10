using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Incoding.Data
{
    using System.Text.RegularExpressions;

    public static class ContextExtensions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            var relationalEntityTypeAnnotations = context.Model.FindEntityType(typeof(T)).Relational();
            var schema = relationalEntityTypeAnnotations.Schema;
            return (!string.IsNullOrWhiteSpace(schema) ? schema + "." : "") + relationalEntityTypeAnnotations.TableName;
        }
    }
}