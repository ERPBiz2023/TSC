using GTCore.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTCore.Connection
{
    public abstract class BaseConnection : IDisposable
    {
        protected string ConnectionString = string.Empty;
        protected IDbConnection Connection = null;
        protected IDbTransaction Transaction = null;
        protected bool IsConnected => Connection != null && Connection.State == ConnectionState.Open;
       
        public abstract void OpenConnection();
        public abstract void CloseConnection();
        public abstract IDbCommand SetCommand(string query, IDataParameter[] parameters = null);
        public abstract void FillDataSet(string query, IDataParameter[] parameters, DataSet dataSet);
        public void Dispose()
        {
            if (IsConnected)
            {
                CloseConnection();
            }

            GC.SuppressFinalize(this);
            GC.Collect();
        }
        public void BeginTransaction()
        {
            if (!IsConnected)
            {
                OpenConnection();
            }

            Transaction = Connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
            }
        } 
        public Hashtable ExecQueryToHashtable(string query, out string errorMessage, IDataParameter[] parameters = null)
        {
            Hashtable result = null;
            errorMessage = string.Empty;
            try
            {
                OpenConnection();
                DataSet dataSet = new DataSet();
                FillDataSet(query, parameters, dataSet);
                DataView defaultView = dataSet.Tables[0].DefaultView;
                result = ((defaultView.Count > 0) ? defaultView[0].ToHashtable() : null);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                Dispose();
            }

            return result;
        }

        public Hashtable[] ExecQueryToArrayHashtable(string query, out string errorMessage, IDataParameter[] parameters = null)
        {
            Hashtable[] result = new Hashtable[0];
            errorMessage = string.Empty;
            try
            {
                OpenConnection();
                DataSet dataSet = new DataSet();
                FillDataSet(query, parameters, dataSet);

                result = dataSet.Tables[0].DefaultView.ToHashtableArray();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                Dispose();
            }

            return result;
        }

      
        public int ExecuteWithOpenClose(string query)
        {
            string errorMessage;
            int result = ExecuteWithOpenCloseBase(query, out errorMessage);
            if (string.IsNullOrEmpty(errorMessage))
            {
                return result;
            }
            return -1;
        }
        public int ExecuteWithOpenCloseBase(string query, out string errorMessage)
        {
            int result = -1;
            errorMessage = string.Empty;
            try
            {
                OpenConnection();
                result = ExecuteBase(query);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                Dispose();
            }

            return result;
        }

        public int ExecuteBase(string query)
        {
            int result = -1;
            using (var command = SetCommand(query))
            {
                result = command.ExecuteNonQuery();
            }

            return result;
        }
    }
}
