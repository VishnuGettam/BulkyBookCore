using BulkyBook.DataAccess.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class StoredProcedureCallRepo : IStoredProcedureCall
    {
        private readonly ApplicationDbContext _db;

        public string ConnectionString { get; set; }

        public StoredProcedureCallRepo(ApplicationDbContext db)
        {
            _db = db;
            ConnectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Execute(string procudureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                sqlConn.Execute(procudureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> List<T>(string procudureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                return sqlConn.Query<T>(procudureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procudureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                var result = SqlMapper.QueryMultiple(sqlConn, procudureName, param, commandType: System.Data.CommandType.StoredProcedure);
                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();

                if (item1 != null && item2 != null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
                }
                else
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(null, null);
                }
            }
        }

        public T OneRecord<T>(string procudureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                var result = sqlConn.Query<T>(procudureName, param, commandType: System.Data.CommandType.StoredProcedure);

                return (T)Convert.ChangeType(result.FirstOrDefault(), typeof(T));
            }
        }

        public T Single<T>(string procudureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                var result = sqlConn.ExecuteScalar<T>(procudureName, param, commandType: System.Data.CommandType.StoredProcedure);

                return (T)Convert.ChangeType(result, typeof(T));
            }
        }
    }
}