using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using DataManagement.Models;

namespace DataManagement
{
    public class InitTasks
    {
        public static Timer timer;
		
        public async static void start_Timer()
        {
            await Task.Run(() => InitTasks.schedule_Timer());
        }
        static void  schedule_Timer()
        {
            Console.WriteLine("### Timer Started ###");

            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 21, 00, 0, 0); //Specify your scheduled time HH,MM,SS [9pm and 00 minutes]
            if (nowTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            double tickTime = (double)(scheduledTime - DateTime.Now).TotalMilliseconds;
            timer = new Timer(tickTime);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Timer Stopped
            timer.Stop();
            //### Scheduled Task Started ###
            Index();
            // Task Finished ### 
            schedule_Timer();
        }
        
        public static void Index()
        {
            RSS_DB_EF db = new RSS_DB_EF();
            List<UserPlanModel> users = db.UserPlans.Where(p => p.expiryTime.Month == DateTime.Now.Month && p.expiryTime.Day == DateTime.Now.Day).ToList();
            if (users != null)
            {
                db.UserPlans.RemoveRange(users);
                db.SaveChanges();
            }

            List<FileModel> files = db.Files.Where(f => (f.sharingDuration.Month == DateTime.Now.Month && f.sharingDuration.Day == DateTime.Now.Day) || (f.fileDuration.Month == DateTime.Now.Month && f.fileDuration.Day == DateTime.Now.Day)).ToList();
            if (files != null)
            {
                db.Files.RemoveRange(files);
                db.SaveChanges();
            }

        }
        
    }
}