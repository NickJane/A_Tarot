using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TarotWinServicesFramework;
using TarotWinServicesFramework.Enum;


namespace ConsoleApplication1
{
    class Program
    {
        public static void OtputString(string str)
        {
            Console.WriteLine(str);
        }

        public static int Add(int a, int b)
        {
            return a + b;
        }
        
        static void Main(string[] args)
        {
            //NewMethod1();
            //TarotWinServicesFramework.Implement.CompressImage c = new TarotWinServicesFramework.Implement.CompressImage();
            //c.Run();
            //Console.Read();

            var action = new Action<string>(OtputString);
            
            action("OutputString Invoked!");
            var func = new Func<int, int, int>(Add);
            var sum = func(3, 5);
            Console.WriteLine(sum);
        }
        class MyTime : System.Timers.Timer
        {
            public int TaskID { get; set; }
        }
        private static void NewMethod1()
        {
            TarotEntities enty = new TarotEntities();
            //获得所有的有效任务.
            var tasks = enty.MyTask.Where(x => x.enable == true);
            
            foreach (var item in tasks)
            {
                
                MyTime mytime = new MyTime();
                mytime.TaskID = item.ID;//将任务ID付给当前的定时类ID


                //达到时间间隔执行. 可以理解为每一个定时控件都是一个新的线程. 
                mytime.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
                {
                    Lib.Utils.LogHelper.CustomLog(item.Name, "d:\\customer\\log2.txt");
                    #region
                    try
                    {
                        //记录逻辑操作耗时多少, 用以计算下一次运行此方法的时间间隔.
                        System.Diagnostics.Stopwatch stop = new System.Diagnostics.Stopwatch();
                        stop.Start();

                        TarotEntities enty2 = new TarotEntities();
                        var currentTimer = sender as MyTime;
                        //必须通过当前的时间控件重新获得数据库对象. 如果直接写item(见上一级的foreach), 那么得到的item始终是tasks集合的最后一个.
                        var currentitem = enty2.MyTask.Where(x => x.ID == currentTimer.TaskID).FirstOrDefault();
                        //如果当前任务满足执行条件
                        if (currentitem.LastRunTime <= DateTime.Now && (currentitem.NextRunTime == null || currentitem.NextRunTime <= DateTime.Now))
                        {
                            currentTimer.Stop();//暂停当前任务
                            //填充当前任务的所有参数
                            #region
                            var itemParameter = enty2.MyTaskParameter.Where(x => x.TaskID == currentitem.ID).ToList();
                            Func<IList<MyTaskParameter>, object[]> toObjects = x =>
                            {
                                object[] objs = new object[x.Count];
                                for (int i = 0; i < x.Count; i++)
                                {
                                    objs[i] = x[i].ParmValue;
                                }
                                return objs;
                            };
                            #endregion
                            //获得并执行
                            //使用工厂反射出需要执行的任务类,并转化为接口对象/
                            IWindowTask itask = TaskFactory.GetTask(currentitem.ClassPath);
                            //执行
                            itask.Run(toObjects(itemParameter));

                            System.Threading.Thread.Sleep(2000);

                            //更新下一次执行的时间
                            #region
                            Func<int, InterValType, double> plusSecond = (intervalue, intervalueType) =>
                            {
                                var temp = 0;
                                switch ((int)intervalueType)
                                {
                                    case 1: temp = currentitem.Interval.Value; break;
                                    case 2: temp = currentitem.Interval.Value * 60; break;
                                    case 3: temp = currentitem.Interval.Value * 60 * 60; break;
                                    case 4: temp = currentitem.Interval.Value * 60 * 60 * 24; break;
                                }
                                return temp;
                            };
                            #endregion

                            //读取配置文件来获得时间间隔
                            #region
                            var tempsecond = plusSecond(currentitem.Interval.Value, (InterValType)currentitem.IntervalType.Value);

                            DateTime d1 = currentitem.NextRunTime.HasValue ? currentitem.NextRunTime.Value : DateTime.Now;
                            currentitem.NextRunTime = d1.AddSeconds(tempsecond);
                            currentitem.LastRunTime = DateTime.Now;
                            #endregion

                            enty2.SaveChanges();//更新任务的状态信息

                            stop.Stop();//监视停止
                            ////时间控件下一次执行的时间间隔为配置间隔减去这次运行时长.
                            //currentTimer.Interval = tempsecond * 1000 - stop.ElapsedMilliseconds;
                            //开始时间控件
                            currentTimer.Start();

                            Console.WriteLine(); Console.WriteLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        string[] ErrorEmails = Lib.Utils.ConfigHelper.GetAppSetting("ErrorEmails").Split(';');
                        foreach (var mail in ErrorEmails)
                        {
                            if (string.IsNullOrEmpty(mail)) continue;
                            Lib.Utils.MailHelper.SendEmail("smtp.exmail.qq.com", "TaluoSystem@taluolaile.com", "TaluoSystem76568091",
                            mail,
                            "异常邮件", ex.Message+"<br>"+ex.StackTrace, false, null);
                        }
                    }
                    #endregion
                };

                mytime.Interval = 60 * 1000 * 2;
                mytime.Start(); //开始, 默认间隔是100毫秒
            }
        }

    }
}
