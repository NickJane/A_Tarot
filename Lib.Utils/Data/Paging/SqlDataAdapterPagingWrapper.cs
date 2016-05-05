using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Lib.Utils.Data.Paging
{
    /// <summary>
    /// ��װ��SqlDataAdapter�е�Fill��ҳ�㷨������С�������ķ�ҳ�ȽϷ��㡣
    /// ���Ǵ��������ķ�ҳ���Ƽ�ʹ������ࡣ
    /// </summary>    
    public class SqlDataAdapterPagingWrapper
    {
       
        #region ��÷�ҳ���ݵķ��� 
        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="sql">ȡ��sql���</param>
        /// <param name="pageIndex">��ʼ��ҳ����</param>
        /// <param name="pageSize">ҳ��ļ�¼��</param>
        /// <param name="connectionString">
        /// ���ݿ����Ӷ���������Ӷ���δ�򿪣�����������򿪣�
        /// ������Σ�����������ر����ӣ����Ե��÷�Ӧ����ʾ�ر����ӡ�
        /// </param>
        /// <returns>ָ�������ķ�ҳ����</returns> 
        [System.Obsolete("��������ʱ�벻Ҫ�ô˷���")]    
        public static DataSet GetDataSetByPage(string sql,int pageIndex,int pageSize,string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                return GetDataSetByPage(sql, pageIndex, pageSize, conn);  
            }
        }
        /// <summary>
        ///     ��ȡ��ҳ����
        /// </summary>
        /// <param name="sql">ȡ��sql���</param>
        /// <param name="pageIndex">0 ��ʼ��ҳ����</param>
        /// <param name="pageSize">ҳ��ļ�¼��</param>
        /// <param name="conn">
        ///     ���ݿ����Ӷ���������Ӷ���δ�򿪣�����������򿪣�
        ///     ������Σ�����������ر����ӣ����Ե��÷�Ӧ����ʾ�ر����ӡ�
        /// </param>
        /// <returns>ָ�������ķ�ҳ����</returns>
        [System.Obsolete("��������ʱ�벻Ҫ�ô˷���")] 
        public static DataSet GetDataSetByPage(string sql,int pageIndex,int pageSize,SqlConnection conn)
        {
            int startRecord = pageIndex * pageSize;
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            adapter.SelectCommand.CommandTimeout = 45;

            try
            {
                adapter.Fill(ds, startRecord, pageSize, "Table0");
                return ds;
            }
            finally
            {
                conn.Close();
            } 
        }

        #endregion

        #region ��ȡ��ҳ���ݵļ�¼�����ķ���

        /// <summary>
        ///     ��ȡ��ҳ���ݵļ�¼����
        /// </summary>
        /// <param name="tableName">�������������Ӳ�ѯ</param>
        /// <param name="whereCondition">
        ///     ��ѯ�������������Ӳ�ѯ�����Բ���"where"ǰ׺�ؼ��֡�
        /// </param>
        /// <param name="groupBy">�������������Բ���"group by"�ؼ���</param>
        /// <param name="connectionString">
        ///     ���ݿ������ַ���
        /// </param>
        /// <returns>��ҳ���ݵļ�¼����</returns>
        public static long GetDataCount(string tableName,string whereCondition,string groupBy,string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                return GetDataCount(tableName, whereCondition, groupBy, conn);
            }
        }

        /// <summary>
        ///     ��ȡ��ҳ���ݵļ�¼����
        /// </summary>
        /// <param name="tableName">�������������Ӳ�ѯ</param>
        /// <param name="whereCondition">
        ///     ��ѯ�������������Ӳ�ѯ������"where"ǰ׺�ؼ���Ҳ���ԡ�
        /// </param>
        /// <param name="groupBy">�������������Բ���"group by"�ؼ���</param>
        /// <param name="conn">
        ///     ���ݿ����Ӷ���������Ӷ���δ�򿪣�����������򿪣�
        ///     ������Σ�����������ر����ӣ����Ե��÷�Ӧ����ʾ�ر����ӡ�
        /// </param>
        /// <returns>��ҳ���ݵļ�¼����</returns>
        public static long GetDataCount(string tableName,string whereCondition,string groupBy,SqlConnection conn)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            StringBuilder sql = new StringBuilder(" SELECT COUNT (*) FROM ");
            sql.Append(tableName);

            if (!string.IsNullOrEmpty(whereCondition))
            {
                if (!whereCondition.Trim().ToLower().StartsWith("where"))
                {
                    sql.Append(" WHERE");
                }
                sql.Append(" ");
                sql.Append(whereCondition);
            }

            if (!string.IsNullOrEmpty(groupBy))
            {
                if (!groupBy.Trim().ToLower().StartsWith("group by"))
                {
                    sql.Append(" GROUP BY");
                }
                sql.Append(" ");
                sql.Append(groupBy);
            }

            return GetDataCount(sql.ToString(), conn);
        }

        /// <summary>
        ///     ��ȡ��¼����
        /// </summary>
        /// <param name="sql">
        ///     ����"select count(*) from tablename" ��Sql���
        /// </param>
        /// <param name="connectionString">
        ///     ���ݿ������ַ���
        /// </param>
        /// <returns>��¼����</returns>
        public static long GetDataCount(string sql, string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                return GetDataCount(sql, conn);
            }
        }

        /// <summary>
        ///     ��ȡ��¼����
        /// </summary>
        /// <param name="sql">
        ///     ����"select count(*) from tablename" ��Sql���
        /// </param>
        /// <param name="conn">
        ///     ���ݿ����Ӷ���������Ӷ���δ�򿪣�����������򿪣�
        ///     ������Σ�����������ر����ӣ����Ե��÷�Ӧ����ʾ�ر����ӡ�
        /// </param>
        /// <returns>��¼����</returns>
        public static long GetDataCount(string sql, SqlConnection conn)
        {

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand sqlCommand = new SqlCommand(sql, conn);
                object rst = sqlCommand.ExecuteScalar();

                if (rst == null)
                {
                    return 0;
                }

                return Convert.ToInt64(rst);
            } 
            finally
            {
                conn.Close();
            } 
        }

        #endregion

    }
}
