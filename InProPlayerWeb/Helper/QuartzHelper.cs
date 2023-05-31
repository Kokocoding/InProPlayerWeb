using InProPlayerWeb.Helper;
using Quartz.Impl;
using Quartz;
using System.Reflection;

namespace InProPlayerWeb.Helper
{
    public class QuartzHelper
    {
        //調度器工廠
        private static ISchedulerFactory? schedulerFactory;
        //調度器
        private static IScheduler? scheduler;

        public static string title = "";
        public static string groups = "on";
        public static string functionName = "TurnOn";
        public static string type = "Cron";
        public static string time = "5";
        public static Dictionary<string, object> JobData = new Dictionary<string, object>(){
            { "Music", "Accusefive-Your Guilt" },
            { "LoopTimes", "1" },
        };

        public QuartzHelper()
        {

        }

        #region Job

        //2、創建一個任務 Create<Class(Task Name)> <= QuartzHelper.cs 
        public class MyJob
        {
            public IJobDetail PlayMusic()
            {
                IJobDetail job = JobBuilder.Create<PlayMusic>()
                                           .WithIdentity(title, groups)
                                           .UsingJobData("LoopTimes", JobData["LoopTimes"].ToString())
                                           .UsingJobData("Music", JobData["Music"].ToString())
                                           .UsingJobData("LoopType", JobData["LoopType"].ToString())
                                           .UsingJobData("KeepTimes", JobData["KeepTimes"].ToString())
                                           .Build();
                return job;
            }

            public IJobDetail OpenMachine()
            {
                IJobDetail job = JobBuilder.Create<OpenMachine>()
                                           .WithIdentity(title, groups)
                                           .UsingJobData("Terminal", JobData["Terminal"].ToString())
                                           .Build();
                return job;
            }
        }

        #endregion

        #region Task
        //打上DisallowConcurrentExecution標簽讓Job進行單線程跑，避免沒跑完時的重覆執行。
        [DisallowConcurrentExecution]
        public class PlayMusic : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {

            }
        }

        [DisallowConcurrentExecution]
        public class OpenMachine : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {

            }
        }
        #endregion

        #region Trigger
        public static ITrigger trigger()
        {
            //3、創建一個觸發器(有4種觸發器供選擇)
            //重複執行：WithRepeatCount(count) / RepeatForever(count)
            //間格時間：WithInterval(time)
            //定時執行：StartAt() / StartNow()
            //設定優先：WithPriority()，default 5

            ITrigger trigger;
            switch (type)
            {
                case "Simple":
                    #region 觸發器1：WithSimpleSchedule 
                    trigger = TriggerBuilder.Create()
                        .WithIdentity(title, groups)
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(int.Parse(time)).RepeatForever())
                        .Build();
                    #endregion
                    break;
                case "Daily":
                    #region 觸發器2：WithDailyTimeIntervalSchedule
                    trigger = TriggerBuilder.Create()
                        .WithIdentity(title, groups)
                        .WithDailyTimeIntervalSchedule(x => x.OnEveryDay().WithIntervalInSeconds(int.Parse(time)))
                        .Build();
                    #endregion
                    break;
                case "Calendar":
                    #region 觸發器3：WithCalendarIntervalSchedule
                    trigger = TriggerBuilder.Create()
                        .WithIdentity(title, groups)
                        .WithCalendarIntervalSchedule(x => x.WithIntervalInSeconds(int.Parse(time)))
                        .Build();
                    #endregion
                    break;
                case "Cron":
                default:
                    #region 觸發器4：WithCronSchedule()
                    trigger = TriggerBuilder.Create()
                        .WithIdentity(title, groups)
                        .WithCronSchedule(time)
                        .Build();
                    #endregion
                    break;
            }
            return trigger;
        }
        #endregion

        #region sys_function
        public static async void Run()
        {
            //1、創建一個調度器
            schedulerFactory = new StdSchedulerFactory();
            scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            ITrigger iTrigger = QuartzHelper.trigger();
            // 取得 ExampleClass 的 Type 物件
            MyJob myJob = new MyJob();
            // 從 Type 物件中查找指定名稱的方法
            MethodInfo methodInfo = typeof(MyJob).GetMethod(functionName);

            IJobDetail iJobDetail;
            // 如果找到了該方法，調用它
            if (methodInfo != null)
            {
                object[] parameters = new object[] { };
                iJobDetail = (IJobDetail)methodInfo.Invoke(myJob, parameters);

                JobKey jobKey = new JobKey(iJobDetail.Key.Name, iJobDetail.Key.Group);
                TriggerKey triggerKey = new TriggerKey(iTrigger.Key.Name, iTrigger.Key.Group);
                bool existsJob = await scheduler.CheckExists(jobKey);
                bool existsTrigger = await scheduler.CheckExists(triggerKey);

                if (!existsJob && !existsTrigger)
                {
                    // 該工作還沒有執行
                    //4、將任務與觸發器添加到調度器中
                    await scheduler.ScheduleJob(iJobDetail, iTrigger);
                }
                //5、開始執行
                await scheduler.Start();
            }
            else
            {
                Shutdown();
            }
        }
        public static void Shutdown()
        {
            if (scheduler != null)
            {
                scheduler.Shutdown(true);
            }
        }
        public static async void removeJob()
        {
            schedulerFactory = new StdSchedulerFactory();
            scheduler = await schedulerFactory.GetScheduler();

            JobKey jobKey = new JobKey(title, groups);
            TriggerKey triggerKey = new TriggerKey(title, groups);

            try
            {
                //停止觸發器
                await scheduler.PauseTrigger(triggerKey);
                //移除觸發器
                await scheduler.UnscheduleJob(triggerKey);
                //刪除任務
                await scheduler.DeleteJob(jobKey);
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}
