// © Traxion Development Services

namespace CodeBase.Test
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Day14Tests
	{
		private Day14 _day;

		[TestInitialize]
		public void TestInitialize()
		{
			_day = new Day14();
		}

		[TestMethod]
		[DataRow("Day14.Test", 1588)]
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
		[DataRow("Day14.Test", 2188189693529)]
		public void PartTwo(string fileName, long expectedResult)
		{
			// Arrange.
			_day.LoadInput(fileName);

			// Act.
			_ = _day.PartOne();
			long result = _day.PartTwo();

			// Assert.
			Assert.AreEqual(expectedResult, result);
		}
	}
}
