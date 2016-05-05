using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Utils.Data.Paging
{
    /// <summary>
    /// ��ҳ��صĹ��÷���
    /// </summary>
    public class CommonPagingMethods
    {
        #region �����ҳҳ���ķ���

        /// <summary>
        ///  ȡ�÷�ҳҳ����
        /// </summary>
        /// <param name="dataCount">�����ܼ�¼��</param>
        /// <param name="PageSize">ÿҳ��ʾ��¼��</param>
        /// <returns>ҳ������</returns>
        public static int ComputePageCount(long dataCount, int PageSize)
        {
            int count = (int)dataCount / PageSize;
            if (dataCount % PageSize > 0)
            {
                count++;
            }
            return count;
        }

        #endregion
    }
}
