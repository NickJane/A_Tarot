using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Lib.ClassExt
{
    public static class EntityExt
    {
        public static IEnumerable<T> GetPageList<T>(this IEnumerable<T> query, Lib.BaseClass.PageParameter para) {
            para.PRecordCount = query.Count();
            if (!string.IsNullOrEmpty(para.POrderField))
            {
                var arrs = para.POrderField.Split('-');
                int i=0;
                foreach (var item in arrs)
                {
                    query = query.ExtOrderBy(item, para.PIsAsc, i == 0 ? true : false);
                }
            }
            query = query.Skip((para.PCurrentPageIndex - 1) * para.PPageSize).Take(para.PPageSize);
            return query;
        }

        /// <summary>
        /// 自定义排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderbyField"></param>
        /// <param name="isAsc"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        public static IEnumerable<T> ExtOrderBy<T>(this IEnumerable<T> query, string orderbyField, bool isAsc, bool isFirst) {
            
            ParameterExpression x=Expression.Parameter(typeof(T),"x");
            MemberExpression fieldInfo=Expression.Property(x,orderbyField);
            var fiedlExpression=Expression.Lambda(fieldInfo,x);//(x=>x.UserID)

            string functionName = (isFirst ? "Order" : "Then") + (isAsc ? "By" : "ByDescending");

            Type[] types=new Type[2];
            types[0]=typeof(T);
            types[1]=typeof(T).GetProperty(orderbyField).PropertyType;


            var methodExpression = Expression.Call(typeof(Queryable), functionName, types, query.AsQueryable().Expression, fiedlExpression);
            
            return query.AsQueryable().Provider.CreateQuery<T>(methodExpression);
        }
        public static IEnumerable<T> Cache<T>(this IEnumerable<T> query, string key) {
            Utils.CacheHelper.Insert(key, query);
            return query;
        }
    }
}
