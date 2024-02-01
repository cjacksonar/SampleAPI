using Classes;
using System.Data;
using System.Data.SqlClient;

namespace Data.Repository
{
    public class RepositoryBase : IDisposable
    {
        private readonly APIDbContext _ctx;

        #region Public Variables
        private Exception _objException;
        private Boolean _blnHasException;
        private string _connectionString;
        #endregion

        #region Constructors
        public RepositoryBase()
        {
            if (_ctx == null) { _ctx = new APIDbContext(); }
        }
        #endregion

        #region Public Properties      

        public APIDbContext Context
        {
            get { return _ctx; }
        }
        public Exception ClassException
        {
            get { return _objException; }
            set { _objException = value; }
        }
        public Boolean HasException
        {
            get { return _blnHasException; }
            set { _blnHasException = value; }
        }
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        #endregion

        #region Public Methods
        public void ClearException()
        {
            _objException = null;
            _blnHasException = false;
        }
        public void SaveChanges()
        {
            _ctx.SaveChanges();
        }
        public void RunSQLCommand(string commandText)
        {
            this.ClearException();
            if (commandText.Length <= 0)
            {
                ClassException = new Exception("RunSQLCommand Error: SQL statement was not provided.");
                HasException = true;
                return;
            }
            try
            {
                if (_connectionString == null) { _connectionString = Globals.ApplicationDatabaseConnectionString; }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    objConn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = objConn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = commandText;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                string exceptionMessage = string.Format("RunSQLCommand Error: {0} {1} Connection string {2}, {3} SQL = {4}", ex.Message, Environment.NewLine, _connectionString, Environment.NewLine, commandText);
                ClassException = new Exception(exceptionMessage);
                HasException = true;
                return;
            }
        }
        public DataTable GetDataTable(string SQL)
        {
            this.ClearException();
            DataTable returnDataTable = new DataTable();
            if (SQL.Length <= 0)
            {
                _objException = new Exception("GetDataTable Error: SQL statement was not provided.");
                _blnHasException = true;
                return returnDataTable;
            }
            if (_connectionString == null) { _connectionString = Globals.ApplicationDatabaseConnectionString; }
            try
            {
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    using (SqlDataAdapter ad = new SqlDataAdapter(SQL, objConn))
                    {
                        DataSet ds = new DataSet();
                        ad.Fill(ds); objConn.Close();
                        return ds.Tables[0];
                    }
                }
            }
            catch (SqlException ex)
            {
                string exceptionMessage = string.Format("GetDataTable Error: {0} {1} Connection string {2}, {3} SQL = {4}", ex.Message, Environment.NewLine, _connectionString, Environment.NewLine, SQL);
                _objException = new Exception(exceptionMessage);
                _blnHasException = true;
                return returnDataTable;
            }
        }
        public DataTable FillDataTableUsingStoredProcedure(string storedProcedureName)
        {
            this.ClearException();
            DataTable returnDataTable = new DataTable();
            if (storedProcedureName.Length <= 0)
            {
                _objException = new Exception("FillDataTable Error: Stored procedure name was not provided.");
                _blnHasException = true;
                return returnDataTable;
            }
            if (_connectionString == null) { _connectionString = Globals.ApplicationDatabaseConnectionString; }
            try
            {
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand(storedProcedureName, objConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(returnDataTable);
                            return returnDataTable;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string exceptionMessage = string.Format("UsingStoredProcedure Error: {0} {1} Connection string {2}, {3} Stored Procedure = {4}", ex.Message, Environment.NewLine, _connectionString, Environment.NewLine, storedProcedureName);
                _objException = new Exception(exceptionMessage);
                _blnHasException = true;
                return returnDataTable;
            }
        }
        public DataTable FillDataTableUsingStoredProcedure(string storedProcedureName, List<SqlParameter> parametersList)
        {
            this.ClearException();
            DataTable returnDataTable = new DataTable();
            if (storedProcedureName.Length <= 0)
            {
                _objException = new Exception("FillDataTable Error: Stored procedure name was not provided.");
                _blnHasException = true;
                return returnDataTable;
            }
            if (parametersList.Count == 0)
            {
                _objException = new Exception("FillDataTable Error: No parameters were provided.");
                _blnHasException = true;
                return returnDataTable;
            }
            if (_connectionString == null) { _connectionString = Globals.ApplicationDatabaseConnectionString; }
            try
            {
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand(storedProcedureName, objConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        foreach (var parm in parametersList)
                        {
                            cmd.Parameters.Add(parm);
                        }
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(returnDataTable);
                            return returnDataTable;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string exceptionMessage = string.Format("FillDataTableUsingStoredProcedure Error: {0} {1} Connection string {2}, {3} Stored Procedure = {4}", ex.Message, Environment.NewLine, _connectionString, Environment.NewLine, storedProcedureName);
                _objException = new Exception(exceptionMessage);
                _blnHasException = true;
                return returnDataTable;
            }
        }

        public void Dispose()
        {
            //if (DataContext != null) DataContext.Dispose();
        }
        #endregion

    }
}
