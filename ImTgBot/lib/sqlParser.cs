using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class sqlParser
    {
   public     static void MainTEst()
        {
            string sql = "SELECT * FROM Users WHERE Age > 25 AND Name = 'John Doe'";
            string whereClause = ExtractWhereClause(sql);
            if (!string.IsNullOrEmpty(whereClause))
            {
               Print("WHERE Clause: " + whereClause);
                List<Tuple<string, string, string>> conditions = ParseWhereClause(whereClause);
                foreach (var condition in conditions)
                {
                   Print($"Field: {condition.Item1}, Operator: {condition.Item2}, Value: {condition.Item3}");
                }
            }
            else
            {
               Print("No WHERE clause found.");
            }
        }

        static string ExtractWhereClause(string sql)
        {
            int whereIndex = sql.IndexOf("WHERE", StringComparison.OrdinalIgnoreCase);
            if (whereIndex != -1)
            {
                return sql.Substring(whereIndex + 5).Trim();
            }
            return null;
        }

        static List<Tuple<string, string, string>> ParseWhereClause(string whereClause)
        {
            List<Tuple<string, string, string>> conditions = new List<Tuple<string, string, string>>();
            string[] parts = whereClause.Split(new[] { " AND ", " OR " }, StringSplitOptions.None);
            foreach (string part in parts)
            {
                string[] tokens = part.Split(new[] { ' ', '=', '>', '<', '!' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 3)
                {
                    conditions.Add(new Tuple<string, string, string>(tokens[0], tokens[1], tokens[2]));
                }
            }
            return conditions;
        }
    }
}
