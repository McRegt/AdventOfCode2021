// © Traxion Development Services

namespace CodeBase.Test
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Day02Tests
	{
		private Day02 _day;

		[TestInitialize]
		public void TestInitialize()
		{
			_day = new Day02();
		}

		[TestMethod]
		[DataRow("Day02.Test", 150)]
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
		[DataRow("Day02.Test", 900)]
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
