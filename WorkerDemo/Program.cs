using WorkerDemo;
using Microsoft.EntityFrameworkCore;
using WorkerDemo.Persistence;
using WorkerDemo.Contracts.Repositories;
using WorkerDemo.Persistence.Repositories;
using WorkerDemo.Options;
using Quartz;
using WorkerDemo.Jobs;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<WorkerDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString"));
});

builder.Services.AddQuartz(opt =>
{
	opt.UseMicrosoftDependencyInjectionJobFactory();
	var jobKey = new JobKey("DeleteLogsJob");
	opt.AddJob<DeleteLogsJob>(options => options.WithIdentity(jobKey));
	opt.AddTrigger(options =>
	{
		options.ForJob(jobKey)
			.WithIdentity("DeleteLogsJob-Trigger")
			.WithCronSchedule(builder.Configuration.GetSection("DeleteLogsJob:CronSchedule").Value ?? "0/5 * * * * ?");
	});
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddScoped<ILogRepository, LogRepository>();

builder.Services.Configure<DeleteLogsJobOptions>(builder.Configuration.GetSection(DeleteLogsJobOptions.DeleteLogJobOptionsKey));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

using var scope = host.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<WorkerDbContext>();

if(dbContext != null)
{
	Seeder.Seed(dbContext);
}

host.Run();
