using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lib.Utils
{
    /// <summary>
    /// using ((new ReadWriterLockSlim()).CreateDisposable(LockType.Write))            {///.........///}
    /// </summary>
    
    public struct Disposable : IDisposable{
        /// <summary>       
        /// 创建销毁帮手实例       
        /// </summary>      
        /// <param name="onCreate">创建时要做的操作</param>       /// 
        /// <param name="onDispose">销毁是要做的操作</param>       
        public Disposable(Action onCreate, Action onDispose)    
        {        
            OnDispose = onDispose;          
            onCreate();     
        }       /// <summary>       /// 销毁时要做的操作 

        private Action OnDispose    {             get ;set;          }
        void IDisposable.Dispose() { OnDispose(); OnDispose = null; }
        
    }  

    public static class ReaderWriterLockSlimHelper {    
        /// <summary>    
        /// 为读写锁创建支持using的IDisposable帮手   
        /// </summary>   
        /// <param name="instance">读写锁实例</param>    
        /// <param name="lockType">加锁类型 读/写</param>    
        /// <returns>帮手实例</returns>    
        public static IDisposable CreateDisposable(this ReaderWriterLockSlim instance, LockType lockType)    
        {        
            var kvp = LockDisposeDic[lockType];        
            return new Disposable(() => kvp.Key(instance), () => kvp.Value(instance));    
        }     
        /// <summary>    
        /// 读写的不同操作字典    
        /// </summary>    
        static Dictionary<LockType, KeyValuePair<Action<ReaderWriterLockSlim>, Action<ReaderWriterLockSlim>>> LockDisposeDic =
            new Dictionary<LockType, KeyValuePair<Action<ReaderWriterLockSlim>, Action<ReaderWriterLockSlim>>>()    
            {        
            {            
                LockType.Read,             
                new KeyValuePair<Action<ReaderWriterLockSlim>,Action<ReaderWriterLockSlim>>(                    
                    ins=>ins.EnterReadLock(),                    
                    ins=>ins.ExitReadLock()                
                    )                     
                    },        
                    {            
                        LockType.Write,             
                        new KeyValuePair<Action<ReaderWriterLockSlim>,Action<ReaderWriterLockSlim>>                 
                            (                    
                            ins=>ins.EnterWriteLock(),                    ins=>ins.ExitWriteLock()                )                     }    
            };
    }
    public enum LockType { Read, Write }
}
