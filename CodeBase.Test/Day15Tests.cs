// © Traxion Development Services

namespace CodeBase.Test
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Day15Tests
	{
		private Day15 _day;

		[TestInitialize]
		public void TestInitialize()
		{
			_day = new Day15();
		}

		[TestMethod]
		[DataRow("Day15.Test", 748)]
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
		[DataRow("Day15.Test", 3045)]
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
