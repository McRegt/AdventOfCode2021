// © Traxion Development Services

namespace Console
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using AdventBase.Interfaces;

	internal static class Program
	{
		private static void Main(string[] args)
		{
			Type interfaceType = typeof(IAdventDay);

			AppDomain.CurrentDomain.Load("CodeBase, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

			List<object> allInstances = AppDomain.CurrentDomain.GetAssemblies()
												 .SelectMany(x => x.GetTypes())
												 .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
												 .Select(x => Activator.CreateInstance(x))
												 .OrderBy(x => x.GetType().Name)
												 .ToList();

			if (!allInstances.Any())
			{
				throw new ArgumentNullException(nameof(allInstances), "No advent days were loaded.");
			}

			if (args.Contains("-l"))
			{
				Console.WriteLine("Running only the latest day.");

				DayRunner(allInstances.Last() as IAdventDay);
			}
			else if (args.Contains("-a"))
			{
				Console.WriteLine("Running all days.");

				allInstances.ForEach(x => DayRunner(x as IAdventDay));
			}
		}

		private static void DayRunner(IAdventDay day)
		{
			Console.WriteLine($"Running code from {day.GetType().Name}.");

			day.LoadInput(day.GetType().Name);
			RunDayPart(day.PartOne);
			RunDayPart(day.PartTwo);
		}

		private delegate long DayPart();

		private static void RunDayPart(DayPart dayPart)
		{
			var watch = Stopwatch.StartNew();

			long result = dayPart.Invoke();

			watch.Stop();

			Console.WriteLine($"Result for {dayPart.GetMethodInfo().Name} is {result} calculated in {watch.ElapsedTicks} ticks ({watch.ElapsedMilliseconds} ms).");
		}
	}
}
