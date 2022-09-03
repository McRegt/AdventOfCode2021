// © Traxion Development Services

namespace CodeBase.Test
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Day07Tests
	{
		private Day07 _day;

		[TestInitialize]
		public void TestInitialize()
		{
			_day = new Day07();
		}

		[TestMethod]
		[DataRow("Day07.Test", 37)]
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
		[DataRow("Day07.Test", 168)]
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
