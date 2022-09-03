// © Traxion Development Services

namespace CodeBase.Test
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Day23Tests
	{
		private Day23 _day;

		[TestInitialize]
		public void TestInitialize()
		{
			_day = new Day23();
		}

		[TestMethod]

		//[DataRow("Day23.Test.01", 46)]
		//[DataRow("Day23.Test.02", 448)]
		[DataRow("Day23.Test.03", 12521)]
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
		[DataRow("Day23.Test.04", 44169)]
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
