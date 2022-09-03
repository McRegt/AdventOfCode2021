// © Traxion Development Services

namespace AdventBase.Helpers
{
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;

	public static class InputLoader<T>
	{
		private static string InputFilePath(string fileName)
		{
			return Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + @"\Inputs\" + fileName + ".txt";
		}

		public static List<int> LoadIntList(string fileName)
		{
			var resultList = new List<int>();

			string fullPath = InputFilePath(fileName);
			var reader = new StreamReader(fullPath);

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();

				if (line != null)
				{
					resultList.Add(int.Parse(line));
				}
			}

			reader.Close();

			return resultList;
		}

		public static List<string> LoadStringList(string fileName)
		{
			var resultList = new List<string>();

			string fullPath = InputFilePath(fileName);
			var reader = new StreamReader(fullPath);

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();

				if (line != null)
				{
					resultList.Add(line);
				}
			}

			reader.Close();

			return resultList;
		}

		public delegate T DelegateAction(string line);

		public static List<T> LoadLines(string fileName, DelegateAction objectToCreate)
		{
			var list = new List<T>();

			string fullPath = InputFilePath(fileName);
			var reader = new StreamReader(fullPath);

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();

				if (line != null)
				{
					T instance = objectToCreate(line);
					list.Add(instance);
				}
			}

			reader.Close();

			return list;
		}
	}
}
