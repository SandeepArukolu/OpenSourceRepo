using Microsoft.EntityFrameworkCore;
using OpenSourceProj.DbContextInfo;
using System.Xml;
using Microsoft.Extensions.Hosting;
using OpenSourceProj.DbTables;
using System.Collections.Generic;

namespace OpenSourceProj.HostedBackGroundService
{
    public class ScheduledTaskService: IHostedService, IDisposable
    {
        private readonly ILogger<ScheduledTaskService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;  

        public ScheduledTaskService(ILogger<ScheduledTaskService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //// Schedule the task to run daily at a specific time (e.g., 10:00 AM)
            //var now = DateTime.Now;
            //var scheduledTime = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0); // Set to 10:00 AM
            //var initialDelay = (scheduledTime > now) ? scheduledTime - now : scheduledTime.AddDays(1) - now;
            // Set up a timer to trigger the task
            //_timer = new Timer(PerformDbInsert, null, initialDelay, TimeSpan.FromDays(1));
            _timer = new Timer(PerformDbInsert, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void PerformDbInsert(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DbContextFile>();

                // Example: Insert a record into the database
               var getUsers = dbContext.Users.ToList();
                var result = getUsers.Select(x => new {
                  x.FullName,
                    x.Email, 
                    x.Password,
                    x.RePassword,
                    x.MobileNo,
                }).ToList();

              
                List<UserInfo> list = getUsers.Select(x=> new UserInfo
                {
                    FullName = x.FullName,
                    Email = x.Email,
                    Password = x.Password,
                    RePassword = x.RePassword,
                    MobileNo = x.MobileNo
                }).ToList();

                // dbContext.Users.AddRangeAsync(list);
                // dbContext.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
