using System;
using System.Data;
using System.Reflection;

namespace Lib.Utils.Data
{
	/// <summary>
	/// �����ṩDataRow�������ͨ����֮����໥ת�����ܣ�
	/// </summary>
	public class DataObjectConverter
	{

		/// <summary>
		/// ��DataRow����ת��Ϊ��ͨ�־û���ҵ�񣩶���
		/// </summary>
		/// <param name="dataRow">DataRow����</param>
		/// <param name="typeOfObject">Ŀ��־û����������</param>
		/// <returns>��ͨ�־û���ҵ�񣩶���</returns>
		public static object FillDataRowToObject(DataRow dataRow, Type typeOfObject)
		{
			if(dataRow == null)
			{
				return null;
			}
			object destObject = typeOfObject.GetConstructor(Type.EmptyTypes).Invoke(null);
			foreach(DataColumn column in dataRow.Table.Columns)
			{
				FillDataRowValueToOjbect(dataRow, column, destObject);
			}
			return destObject;
		}

		/// <summary>
		/// ����ͨ�־û���ҵ�񣩶���ת��ΪDataRow����
		/// </summary>
		/// <param name="srcObject">��ͨ�־û���ҵ�񣩶���</param>
		/// <param name="dataRow">DataRow����</param>
		public static void FillObjectToDataRow(object srcObject, DataRow dataRow)
		{
			if(srcObject!=null && dataRow!=null)
			{
				foreach(DataColumn column in dataRow.Table.Columns)
				{
					FillObjectValueToDataRow(srcObject, column, dataRow);
				}
			}
		}

		private static void FillDataRowValueToOjbect(DataRow dataRow, DataColumn column, object rstObject)
		{
			PropertyInfo propertyInfo = rstObject.GetType().GetProperty(column.ColumnName);
			if(propertyInfo==null)
			{
				return;
			}
			if(dataRow.IsNull(column.ColumnName))
			{
				if(propertyInfo.PropertyType.Equals(typeof(DateTime)))
				{
//					propertyInfo.SetValue(rstObject, MinDateTimeValueInDB, null);
//					propertyInfo.GetSetMethod().Invoke(rstObject, new object[]{MinDateTimeValueInDB});
				}
				return;
			}
			propertyInfo.SetValue(rstObject, dataRow[column.ColumnName], null);
		}

		private static void FillObjectValueToDataRow(object srcObject, DataColumn column, DataRow dataRow)
		{
			PropertyInfo propertyInfo = srcObject.GetType().GetProperty(column.ColumnName);
			if(propertyInfo==null)
			{
				return;
			}

			if(propertyInfo.PropertyType.Equals(typeof(DateTime)))
			{
				if(
					((DateTime)propertyInfo.GetValue(srcObject, null)).Equals(DateTime.MinValue)
					)
				{
					dataRow[column.ColumnName] = DBNull.Value;
					return;
				}
			}

			if(propertyInfo.PropertyType.Equals(typeof(string)))
			{
				string val = (string)propertyInfo.GetValue(srcObject, null);
				if(val==null || val.Length==0)
				{
					dataRow[column.ColumnName] = DBNull.Value;
					return;
				}
			}

			if(!column.ReadOnly)
			{
				dataRow[column.ColumnName] = propertyInfo.GetValue(srcObject, null);
			}
		}
	}
}
