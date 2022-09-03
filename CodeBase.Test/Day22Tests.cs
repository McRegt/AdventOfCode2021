// © Traxion Development Services

namespace CodeBase.Test
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Day22Tests
	{
		private Day22 _day;

		[TestInitialize]
		public void TestInitialize()
		{
			_day = new Day22();
		}

		[TestMethod]
		[DataRow("Day22.Test.01", 7)]
		[DataRow("Day22.Test.02", 18)]
		[DataRow("Day22.Test.03", 39)]
		[DataRow("Day22.Test.04", 590784)]
		[DataRow("Day22.Test.05", 27)]
		[DataRow("Day22.Test.06", 24)]
		[DataRow("Day22.Test.07", 22)]
		[DataRow("Day22.Test.08", 20)]
		[DataRow("Day22.Test.09", 24)]
		[DataRow("Day22.Test.10", 474140)]
		[DataRow("Day22", 615869)]
		public void PartOne(string fileName, long expectedResult)
		{
			// Arrange.
			_day.LoadInput(fileName);

			// Act.
			long result = _day.PartOne();

			// Assert.
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		[DataRow("Day22.Test.10", 2758514936282235)]
		public void PartTwo(string fileName, long expectedResult)
		{
			// Arrange.
			_day.LoadInput(fileName);

			// Act.
			long result = _day.PartTwo();

			// Assert.
			Assert.AreEqual(expectedResult, result);
		}
	}
}
