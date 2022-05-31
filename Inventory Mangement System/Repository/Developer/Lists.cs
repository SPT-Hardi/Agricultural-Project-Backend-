using Inventory_Mangement_System.Model.Developer;
using Inventory_Mangement_System.Repository.Developer.Contact;
using Inventory_Mangement_System.Repository.Developer.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository.Developer
{
    public class Lists
    {
        public Response.Data General(string Con, int PageSize, int PageNumber, string WhereSql, List<SqlParameter> WhereSqlParameters, string SortSql)
        {
            var selectQuery = new Developer.Schema.Properties.Property().Value(Con, "SqlCommand");
            var Ids = Current.Ids;
            using (var sqlCon = new SqlConnection(DatabaseFunctions.ConnectionString()))
            {
                var sqlCom = new System.Data.SqlClient.SqlCommand("", sqlCon);
                sqlCom.Parameters.AddWithValue("PageSize", PageSize);
                sqlCom.Parameters.AddWithValue("PageNumber", PageNumber);
                sqlCom.Parameters.AddWithValue("OId", Ids.OId);
                sqlCom.Parameters.AddWithValue("RId", Ids.RId);
                sqlCom.Parameters.AddWithValue("UId", Ids.UId);
                sqlCom.Parameters.AddWithValue("LId", Ids.LId);

                foreach (SqlParameter p in WhereSqlParameters)
                {
                    if (!sqlCom.Parameters.Contains(p.SourceColumn))
                        sqlCom.Parameters.AddWithValue(p.SourceColumn, p.Value);
                }

                var sqlComTotal = new System.Data.SqlClient.SqlCommand("", sqlCon);
                sqlComTotal.Parameters.AddWithValue("OId", Ids.OId);
                sqlComTotal.Parameters.AddWithValue("RId", Ids.RId);
                sqlComTotal.Parameters.AddWithValue("UId", Ids.UId);
                sqlComTotal.Parameters.AddWithValue("LId", Ids.LId);

                foreach (SqlParameter p in WhereSqlParameters)
                {
                    if (!sqlComTotal.Parameters.Contains(p.SourceColumn))
                        sqlComTotal.Parameters.AddWithValue(p.SourceColumn, p.Value);
                }

                string RawWhereSql = WhereSql;
                if (!string.IsNullOrEmpty(WhereSql))
                {
                    selectQuery = "select * from (" + selectQuery + ") as temp1 where " + WhereSql;
                    var TotalWhereSqlParameters = new List<SqlParameter>();
                    foreach (var x in WhereSqlParameters)
                    {
                        TotalWhereSqlParameters.Add(new SqlParameter(x.ParameterName, x.Value));
                        RawWhereSql = RawWhereSql.Replace("@" + x.ParameterName, x.Value == null ? "null" : string.Format("'{0}'", x.Value.ToString().Replace("%", "")));
                    }

                    sqlCom.Parameters.AddRange(WhereSqlParameters.ToArray());
                    sqlComTotal.Parameters.AddRange(TotalWhereSqlParameters.ToArray());
                }

                string OrderBy = "";
                if (!string.IsNullOrEmpty(SortSql))
                {
                    OrderBy = " order by " + SortSql;
                }
                else
                {
                    var DefaultOrderBy = new Developer.Schema.Properties.Property().Value(Con, "SqlCommandOrderBy");
                    if (!string.IsNullOrEmpty(DefaultOrderBy))
                    {
                        OrderBy = " order by " + DefaultOrderBy;
                    }
                    else
                    {
                        throw new ArgumentException("Sort parameter is required!");
                    }
                }

                var sq = "select * from(" + selectQuery + ") as #temp2 " + OrderBy + " OFFSET @PageSize * (@PageNumber - 1) ROWS " + "FETCH NEXT @PageSize ROWS ONLY OPTION (RECOMPILE)";
                sqlCom.CommandText = sq;
                sqlComTotal.CommandText = "select count(*) from (" + selectQuery + ") as temp";
                sqlCon.Open();
                var dt = new DataTable();
                var da = new System.Data.SqlClient.SqlDataAdapter(sqlCom);
                da.Fill(dt);     
                var records = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                        row.Add(col.ColumnName, dr[col]);
                    records.Add(row);
                }

                int Total = (int)sqlComTotal.ExecuteScalar();
                var currentTotal = dt.Rows.Count;
                var data = new Model.Developer.Response.Data()
                {
                    Total = Total,
                    CurrentTotal = currentTotal,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    Sort = SortSql,
                    Filter = RawWhereSql,
                    Records = records
                };
                return data;
            }
        }

        public Response.Structure GeneralSchema(string Con, string WhereSql, List<SqlParameter> WhereSqlParameters, string SortSql)
        {
            var selectQuery = new Developer.Schema.Properties.Property().Value(Con, "SqlCommand");
            var Ids = Current.Ids;
            using (var sqlCon = new SqlConnection(DatabaseFunctions.ConnectionString()))
            {
                var sqlCom = new System.Data.SqlClient.SqlCommand("", sqlCon);
                sqlCom.Parameters.AddWithValue("OId", Ids.OId);
                sqlCom.Parameters.AddWithValue("RId", Ids.RId);
                sqlCom.Parameters.AddWithValue("UId", Ids.UId);

                foreach (SqlParameter p in WhereSqlParameters)
                {
                    sqlCom.Parameters.AddWithValue(p.SourceColumn, p.Value);
                }

                string RawWhereSql = WhereSql;
                if (!string.IsNullOrEmpty(WhereSql))
                {
                    selectQuery = "select top 0 * from (" + selectQuery + ") as temp1 where " + WhereSql;
                    var TotalWhereSqlParameters = new List<SqlParameter>();
                    foreach (var x in WhereSqlParameters)
                    {
                        TotalWhereSqlParameters.Add(new SqlParameter(x.ParameterName, x.Value));
                        RawWhereSql = RawWhereSql.Replace("@" + x.ParameterName, "'" + x.Value.ToString().Replace("%", "") + "'");
                    }

                    sqlCom.Parameters.AddRange(WhereSqlParameters.ToArray());
                }

                string OrderBy = "";
                if (!string.IsNullOrEmpty(SortSql))
                {
                    OrderBy = " order by " + SortSql;
                }
                else
                {
                    var DefaultOrderBy = new Developer.Schema.Properties.Property().Value(Con, "SqlCommandOrderBy");
                    if (!string.IsNullOrEmpty(DefaultOrderBy))
                    {
                        OrderBy = " order by " + DefaultOrderBy;
                    }
                    else
                    {
                        throw new ArgumentException("Sort parameter is required!");
                    }
                }

                var sq = "select * from(" + selectQuery + ") as temp2 " + OrderBy;
                sqlCom.CommandText = sq;
                sqlCon.Open();
                var dt = new DataTable();
                var da = new SqlDataAdapter(sqlCom);
                da.Fill(dt);
                var columns = new List<Columns>();
                foreach (DataColumn col in dt.Columns)
                    columns.Add(new Columns() { Name = col.ColumnName, Type = col.DataType.Name });
                var result = new Response.Structure()
                {
                    Con = Con,
                    Mode = Response.ModeTypes.Simple,
                    Options = new Response.RelatedControllers()
                    {
                        //CreateCon = new Developer.Schema.Properties.Property().Value(Con, "CreateController").Trim(),
                        EditCon = new Developer.Schema.Properties.Property().Value(Con, "EditController")?.Trim(),
                        //DeleteCon = new Developer.Schema.Properties.Property().Value(Con, "DeleteController").Trim(),
                        // ViewCon = new Developer.Schema.Properties.Property().Value(Con, "ViewController").Trim(),
                        // PrintCon = new Developer.Schema.Properties.Property().Value(Con, "PrintController").Trim()
                    },

                    Schema = new Model.Developer.Schema()
                    {
                        Columns = columns,
                        Keys = new Developer.Schema.Properties.Property().Value(Con, "SqlCommandKeys")?.Split(',').ToList(),
                        OrderBy = new Developer.Schema.Properties.Property().Value(Con, "SqlCommandOrderBy")
                    }
                };
                return result;
            }
        }
    }
}
