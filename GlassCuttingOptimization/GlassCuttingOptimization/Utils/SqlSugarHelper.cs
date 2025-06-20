using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlassCuttingOptimization.Utils
{
  
    public class SqlSugarHelper
    {
        public SqlSugarClient _db;
     
        public SqlSugarHelper()
        {
            string path = Path.Combine(Application.StartupPath,"Database", "data.db");
            _db = new SqlSugarClient(new ConnectionConfig()
            {

                ConnectionString = $"Data Source = {path}; ",

                DbType = SqlSugar.DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
            });


        }

        public SqlSugarClient Db => _db;

        // 增加
        public int Insert<T>(T entity) where T : class, new()
        {
            return _db.Insertable(entity).ExecuteReturnIdentity();
        }

        // 删除
        public int Delete<T>(object primaryKey) where T : class, new()
        {
            return _db.Deleteable<T>().In(primaryKey).ExecuteCommand();
        }

        // 更新
        public int Update<T>(T entity) where T : class, new()
        {
            return _db.Updateable(entity).ExecuteCommand();
        }

        // 查询
        public List<T> Query<T>() where T : class, new()
        {
            return _db.Queryable<T>().ToList();
        }

        public T QueryById<T>(object primaryKey) where T : class, new()
        {
            return _db.Queryable<T>().InSingle(primaryKey);
        }

        // 新增的方法：批量插入
        public int InsertBatch<T>(List<T> entities) where T : class, new()
        {
            return _db.Insertable(entities).ExecuteCommand();
        }

        // 新增的方法：条件查询
        public List<T> QueryByCondition<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return _db.Queryable<T>().Where(expression).ToList();
        }

        // 新增的方法：分页查询
        public List<T> QueryByPage<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereExpression = null) where T : class, new()
        {
            var query = _db.Queryable<T>();
            if (whereExpression != null)
            {
                query = query.Where(whereExpression);
            }
            return query.ToPageList(pageIndex, pageSize);
        }

        // 新增的方法：事务处理
        public bool ExecuteWithTransaction(Action action)
        {
            try
            {
                _db.Ado.BeginTran();
                action();
                _db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                _db.Ado.RollbackTran();
                Console.WriteLine($"Transaction failed: {ex.Message}");
                return false;
            }
        }
    }
}
