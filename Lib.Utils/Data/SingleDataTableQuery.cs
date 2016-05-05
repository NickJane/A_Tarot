using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Lib.Utils.Data;

namespace Lib.Utils.Data
{
	/// <summary>
	/// ������һ�������࣬�����Ǹ���DataRow��DataTable��������Sql���
	/// ע�⣺ֻ֧��DataRow�������������£�
	/// Boolean��Byte��Char��DateTime��Double��Int16��
	/// Int32��Int64��SByte��Single��String��TimeSpan��UInt16��UInt32��UInt64��
	/// ֻ����Щ���͵�DataRow��������Ϊ��������Where�Ӿ䣬
	/// ��DataRow������������Ϊ0ʱ��DateTime���͵�ֵΪ��Сʱ��ʱ��������ΪWhere�Ӿ��е�����
	/// </summary>
	public class SelectSqlBuilder
	{
		/// <summary>
		/// ����DataRow��������Select Sql���
		/// </summary>
		/// <param name="row">DataRow����</param>
		/// <returns>Select Sql���</returns>
		public static string BuildSelectSql(DataRow row)
		{
			return " SELECT " + BuildSelectList(row.Table) 
				+ " FROM " + BuildFromList(row)
				+ " WHERE " + BuildWhere(row);
		}

		/// <summary>
		/// ����Sql����Select List
		/// </summary>
		/// <param name="table">DataTable����</param>
		/// <returns>Sql����Select List �ִ�</returns>
		public static string BuildSelectList(DataTable table)
		{
			return BuildSelectList(table, false);
		}

		/// <summary>
		/// ����Sql����Select List�����isChangeColumnNameΪtrue,
		/// ��ѱ�����ΪSelect List���ֶα�����ǰ׺��
		/// ��table1.column1�ֶ���Select List�еı���Ϊ��table1_column1
		/// </summary>
		/// <param name="table">DataTable����</param>
		/// <param name="isChangeColumnName">�Ƿ�ѱ�����ΪSelect List���ֶα�����ǰ׺</param>
		/// <returns>Sql����Select List �ִ�</returns>
		public static string BuildSelectList(DataTable table, bool isChangeColumnName)
		{
			StringBuilder rst = new StringBuilder("");
			foreach ( DataColumn column in table.Columns )
			{
				if(rst.Length > 0 )
				{
					rst.Append(", ");
				}
				if(table.TableName.Length>0)
				{
					rst.Append(table.TableName + ".");
				}
				rst.Append(column.ColumnName);
				if(isChangeColumnName)
				{
					rst.Append(" " + table.TableName + "_" + column.ColumnName);
				}
			}
			return rst.ToString();
		}

		private static string BuildFromList(DataRow row)
		{
			return row.Table.TableName;
		}

		/// <summary>
		/// ������������sql����where����
		/// </summary>
		/// <param name="row">DataRow����</param>
		/// <returns></returns>
		private static string BuildWhere(DataRow row)
		{
			StringBuilder rst = new StringBuilder("");
			foreach ( DataColumn column in row.Table.Columns )
			{
				if(!IsNeedAddToSql(row, column))
				{
					continue;
				}
				if(rst.Length > 0 )
				{
					rst.Append(" AND ");
				}
				if(row.Table.TableName.Length>0)
				{
					rst.Append(row.Table.TableName + ".");
				}
				rst.Append(column.ColumnName);
				rst.Append(" = @");
				rst.Append(row.Table.TableName + "_" + column.ColumnName);
			}
			return rst.ToString();
		}

		/// <summary>
		///		����Where�����õ���sql������
		/// </summary>
		/// <param name="row">���������ľ���ֵ�Ӵ�DataRow��ȡ</param>
		/// <returns></returns>
		public static SqlParameter[] BuildParameter(DataRow row)
		{
			ArrayList rst = new ArrayList();
			foreach ( DataColumn column in row.Table.Columns )
			{
				if(!IsNeedAddToSql(row, column))
				{
					continue;
				}
				string paramName = "@" + row.Table.TableName + "_" + column.ColumnName;
				rst.Add(new SqlParameter(paramName, row[column.ColumnName]));
			}
			return (SqlParameter[])rst.ToArray(typeof(SqlParameter));
		}

		private static bool IsNeedAddToSql(DataRow row, DataColumn column)
		{
			if(!IsSupportedType(column.DataType))
			{
				return false;
			}
			if(row[column.ColumnName]==DBNull.Value)
			{
				return false;
			}
			if(IsNumberType(column.DataType) && row[column.ColumnName].Equals(0))
			{
				return false;
			}
			if(column.DataType.Equals(typeof(DateTime))
				&&DateTime.MinValue.Equals(row[column.ColumnName])
				)
			{
				return false;
			}
			return true;
		}

		private static bool IsSupportedType(Type type)
		{
			switch ( type.Name )
			{
				case "Boolean":
					return true;
				case "Byte":
					return true;
				case "Char":
					return true;
				case "DateTime":
					return true;
				case "Double":
					return true;
				case "Int16":
					return true;
				case "Int32":
					return true;
				case "Int64":
					return true;
				case "SByte":
					return true;
				case "Single":
					return true;
				case "String":
					return true;
				case "TimeSpan":
					return true;
				case "UInt16":
					return true;
				case "UInt32":
					return true;
				case "UInt64":
					return true;
				default:
					return false;
			}
		}

		private static bool IsNumberType(Type type)
		{
			switch ( type.Name)
			{
				case "Byte":
					return true;
				case "Double":
					return true;
				case "Int16":
					return true;
				case "Int32":
					return true;
				case "Int64":
					return true;
				case "SByte":
					return true;
				case "Single":
					return true;
				case "UInt16":
					return true;
				case "UInt32":
					return true;
				case "UInt64":
					return true;
				default:
					return false;
			}
		}
	}

	/// <summary>
	/// ������������SelectSqlBuilder��ʵ�ֵ���������ݶ�ȡ��
	/// ���÷�ֻ��ָ��һ��DataRow��������ݿ������ַ����Ϳ��ԡ�
	/// ע�⣺ֻ֧��DataRow�������������£�
	/// Boolean��Byte��Char��DateTime��Double��Int16��
	/// Int32��Int64��SByte��Single��String��TimeSpan��UInt16��UInt32��UInt64��
	/// ֻ����Щ���͵�DataRow��������Ϊ��������Where�Ӿ䣬
	/// ��DataRow������������Ϊ0ʱ��DateTime���͵�ֵΪ��Сʱ��ʱ��������ΪWhere�Ӿ��е�������
	/// �����DataRow����ֻ��һ������������ֵΪ��ʱ�������ܲ�ѯ�������ԣ��ֶΣ�����0�ļ�¼��
	/// ���ǲ�ѯ�����еļ�¼��
	/// </summary>
	public class SingleDataTableQuery
	{
		#region GetDataSet
		/// <summary>
		/// �����ݿ���ȡ���������paramRow�и�ֵ"��/and"������DataSet
		/// </summary>
		/// <param name="connectionString">���ݿ������ַ���</param>
		/// <param name="paramRow">DataRow����ֵ</param>
		/// <returns>����paramRow������DataSet</returns>
		public static DataSet GetDataSet(string connectionString, DataRow paramRow)
		{
			using(SqlConnection connection = new SqlConnection(connectionString))
			{
				return GetDataSet(connection, paramRow);
			}
		}

		/// <summary>
		/// �����ݿ���ȡ���������paramRow�и�ֵ"��/and"������DataSet
		/// </summary>
		/// <param name="connection">���ݿ����Ӷ���</param>
		/// <param name="paramRow">DataRow����ֵ</param>
		/// <returns>����paramRow������DataSet</returns>
		public static DataSet GetDataSet(SqlConnection connection, DataRow paramRow)
		{
			DataSet dataSet = (DataSet)paramRow.Table.DataSet.GetType().GetConstructor(new Type[0]).Invoke(null);
			FillDataSet(connection, paramRow, dataSet);
			return dataSet;
		}

		/// <summary>
		/// �����ݿ���ȡ���������paramRow�и�ֵ"��/and"������DataSet,
		/// ������orderSql��startRecord��maxRecords��Ϊ��ҳ����
		/// </summary>
		/// <param name="connectionString">���ݿ������ַ���</param>
		/// <param name="paramRow">DataRow����ֵ</param>
		/// <param name="orderSql">����Sql�Ӿ�</param>
		/// <param name="startRecord">��ʼ��¼�ţ���СֵΪ0</param>
		/// <param name="maxRecords">һ�η��ص�����¼��</param>
		/// <returns>����paramRow������DataSet</returns>
		public static DataSet GetDataSet(
			string connectionString, 
			DataRow paramRow, 
			string orderSql, 
			int startRecord, 
			int maxRecords)
		{
			using(SqlConnection connection = new SqlConnection(connectionString))
			{
				return GetDataSet(connection, paramRow, orderSql, startRecord, maxRecords);
			}
		}

		/// <summary>
		/// �����ݿ���ȡ���������paramRow�и�ֵ"��/and"������DataSet,
		/// ������orderSql��startRecord��maxRecords��Ϊ��ҳ����
		/// </summary>
		/// <param name="connection">���ݿ����Ӷ���</param>
		/// <param name="paramRow">DataRow����ֵ</param>
		/// <param name="orderSql">����Sql�Ӿ�</param>
		/// <param name="startRecord">��ʼ��¼�ţ���СֵΪ0</param>
		/// <param name="maxRecords">һ�η��ص�����¼��</param>
		/// <returns>����paramRow������DataSet</returns>
		public static DataSet GetDataSet(
			SqlConnection connection, 
			DataRow paramRow, 
			string orderSql, 
			int startRecord, 
			int maxRecords)
		{
			DataSet dataSet = (DataSet)paramRow.Table.DataSet.GetType()
				.GetConstructor(new Type[0]).Invoke(null);
			FillDataSet(connection,paramRow,orderSql, startRecord, maxRecords,dataSet);
			return dataSet;
		}
		#endregion

		#region FillDataSet ������GetDataSet()������ͬ��ֻ���ɵ��÷�����DataSet���������½�DataSet
		/// <summary>
		/// ����FillDataSet��GetDataSet����������ͬ��ֻ���ɵ��÷�����DataSet���������½�DataSet
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="paramRow"></param>
		/// <param name="dataSet"></param>
		public static void FillDataSet(string connectionString, DataRow paramRow, DataSet dataSet)
		{
			using(SqlConnection connection = new SqlConnection(connectionString))
			{
				FillDataSet(connection, paramRow, dataSet);
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramRow"></param>
        /// <param name="dataSet"></param>
		public static void FillDataSet(SqlConnection connection, DataRow paramRow, DataSet dataSet)
		{
			CreateDataAdapter(connection, paramRow).Fill(dataSet, paramRow.Table.TableName);
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="paramRow"></param>
        /// <param name="orderSql"></param>
        /// <param name="startRecord"></param>
        /// <param name="maxRecords"></param>
        /// <param name="dataSet"></param>
		public static void FillDataSet(
			string connectionString, 
			DataRow paramRow, 
			string orderSql, 
			int startRecord, 
			int maxRecords,
			DataSet dataSet)
		{
			using(SqlConnection connection = new SqlConnection(connectionString))
			{
				FillDataSet(connection, paramRow, orderSql, startRecord, maxRecords, dataSet);
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramRow"></param>
        /// <param name="orderSql"></param>
        /// <param name="startRecord"></param>
        /// <param name="maxRecords"></param>
        /// <param name="dataSet"></param>
		public static void FillDataSet(
			SqlConnection connection, 			
			DataRow paramRow, 
			string orderSql, 
			int startRecord, 
			int maxRecords,
			DataSet dataSet)

		{
			CreateDataAdapter(connection, paramRow, orderSql)
				.Fill(dataSet, startRecord, maxRecords, paramRow.Table.TableName);
		}
		#endregion

		#region FillDataTable
		/// <summary>
		/// ����FillDataSet��GetDataSet����������ͬ��
		/// ֻ���ɵ��÷�����DataTable�������������DataTable
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="paramRow"></param>
		/// <param name="dataTable"></param>
		public static void FillDataTable(string connectionString, DataRow paramRow, DataTable dataTable)
		{
			using(SqlConnection connection = new SqlConnection(connectionString))
			{
				FillDataTable(connection, paramRow, dataTable);
			}
		}		

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramRow"></param>
        /// <param name="dataTable"></param>
		public static void FillDataTable(SqlConnection connection, DataRow paramRow, DataTable dataTable)
		{
			CreateDataAdapter(connection, paramRow).Fill(dataTable);
		}
		#endregion

		#region CreateDataAdapter
		private static SqlDataAdapter CreateDataAdapter(
			SqlConnection connection, 
			DataRow paramRow)
		{
			return CreateDataAdapter(connection, paramRow, "");
		}

		private static SqlDataAdapter CreateDataAdapter(
			SqlConnection connection, 
			DataRow paramRow, 
			string orderSql)
		{
			string sql = SelectSqlBuilder.BuildSelectSql(paramRow) + " " +orderSql;
			SqlParameter[] sqlParams = SelectSqlBuilder.BuildParameter(paramRow);
			SqlDataAdapter adapter = new SqlDataAdapter(sql,connection);
			foreach(SqlParameter sqlParam in sqlParams)
			{
				adapter.SelectCommand.Parameters.Add(sqlParam);
			}
			return adapter;
		}
		#endregion
	}
}