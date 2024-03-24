namespace WorkerDemo.Contracts.Repositories
{
	public interface ILogRepository
	{
		Task RemoveLogsAfterPeriodOfTime(DateTime date);
	}
}
