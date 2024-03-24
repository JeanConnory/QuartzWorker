using Microsoft.Extensions.Options;
using Quartz;
using WorkerDemo.Contracts.Repositories;
using WorkerDemo.Options;

namespace WorkerDemo.Jobs
{
	[DisallowConcurrentExecution]
	public class DeleteLogsJob : IJob
	{
		private readonly ILogRepository _logRepository;
		private readonly int _amountOfDays;

        public DeleteLogsJob(ILogRepository logRepository, IOptions<DeleteLogsJobOptions> options)
        {
			_logRepository = logRepository;
			_amountOfDays = options.Value.AmountOfDays ?? throw new ArgumentException("Amount of days cannot be null");
        }

        public async Task Execute(IJobExecutionContext context)
		{
			var date = DateTime.Now.AddDays(-_amountOfDays);
			await _logRepository.RemoveLogsAfterPeriodOfTime(date);
		}
	}
}
