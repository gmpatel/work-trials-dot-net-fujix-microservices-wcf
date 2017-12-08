﻿using System;
using FXA.Framework.Scheduler.Host.Core;
using Quartz;
using Quartz.Spi;

namespace FXA.Framework.Scheduler.Host
{
    public class SchedulerService : ISchedulerService
    {
        private bool IsPaused { get; set; }

        public IScheduler Scheduler { get; private set; }
        //public ILogger Logger { get; private set; }

        public SchedulerService(IScheduler scheduler, IJobFactory jobFactory/*, ILogger logger*/)
        {
            if (scheduler == null) 
                throw new ArgumentNullException("scheduler");

            if (jobFactory == null)
                throw new ArgumentNullException("jobFactory");
            
            //if (logger == null)
            //    throw new ArgumentNullException("logger");

            IsPaused = false;
            Scheduler = scheduler;
            //Logger = logger;

            scheduler.JobFactory = jobFactory;
        }

        public void Start()
        {
            //Logger.Info("StandardSchedulerService : Starting Service...");

            if (!Scheduler.IsStarted)
            {
                //Logger.Info("StandardSchedulerService : Starting Scheduler...");

                IsPaused = false;
                Scheduler.Start();
            }

            //Logger.Info("StandardSchedulerService : Service Started...");
        }

        public void Stop()
        {
            //Logger.Info("StandardSchedulerService : Stopping Service...");

            if (!Scheduler.IsShutdown)
            {
                //Logger.Info("StandardSchedulerService : Shutting-down Scheduler...");

                IsPaused = false;
                Scheduler.Shutdown(true);
                Scheduler.Clear();    
            }

            //Logger.Info("StandardSchedulerService : Service Stopped...");
        }

        public void Pause()
        {
            //Logger.Info("StandardSchedulerService : Pausing Service...");

            if (!IsPaused)
            {
                //Logger.Info("StandardSchedulerService : Pausing All Triggers...");

                IsPaused = true;
                Scheduler.PauseAll();
            }

            //Logger.Info("StandardSchedulerService : Service Paused...");
        }

        public void Continue()
        {
            //Logger.Info("StandardSchedulerService : Continueing Service...");

            if (IsPaused)
            {
                //Logger.Info("StandardSchedulerService : Continueing All Triggers...");

                IsPaused = false;
                Scheduler.ResumeAll();
            }

            //Logger.Info("StandardSchedulerService : Service Continued...");
        }
    }
}   