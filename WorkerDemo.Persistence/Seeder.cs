using Bogus;
using WorkerDemo.Contracts.Entities;

namespace WorkerDemo.Persistence
{
	public static class Seeder
	{
		private static List<string> _messages = new() { "Message1", "Message2", "Message3", "Message4" };
		private static List<string> _stackTraces = new() { "StackTrace1", "StackTrace2", "StackTrace3", "StackTrace4" };

		public static void Seed(WorkerDbContext dbContext)
		{
			if (dbContext.Logs.Count() < 50)
			{
				var faker = new Faker<Log>()
					.RuleFor(x => x.Created, x => x.Date.Past())
					.RuleFor(x => x.Message, x => x.PickRandom(_messages))
					.RuleFor(x => x.StackTrace, x => x.PickRandom(_stackTraces))
					.RuleFor(x => x.LogLevel, x => x.PickRandom<LogLevel>());

				var fakeLogs = faker.GenerateBetween(200, 200);

				dbContext.Logs.AddRange(fakeLogs);
				dbContext.SaveChanges();
			}
		}
	}
}
