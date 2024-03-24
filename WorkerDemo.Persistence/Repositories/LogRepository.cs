using WorkerDemo.Contracts.Repositories;

namespace WorkerDemo.Persistence.Repositories
{
	public class LogRepository : ILogRepository
	{
        private readonly WorkerDbContext _dbContext;

        public LogRepository(WorkerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task RemoveLogsAfterPeriodOfTime(DateTime date)
        {
            var logsToDelete = _dbContext.Logs.Where(x => x.Created < date);
            _dbContext.RemoveRange(logsToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}
