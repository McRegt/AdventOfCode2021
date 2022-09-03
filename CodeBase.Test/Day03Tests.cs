// © Traxion Development Services

namespace CodeBase.Test
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Day03Tests
	{
		private Day03 _day;

		[TestInitialize]
		public void TestInitialize()
		{
			_day = new Day03();
		}

		[TestMethod]
		[DataRow("Day03.Test", 198)]
		public void PartOne(string fileName, long expectedResult)
		{
			// Arrange.
			_day.LoadInput(fileName);

			// Act.
			long result = _day.PartOne();

			// Assert.
			Assert.AreEqual(198, result);
		}

		[TestMethod]
		[DataRow("Day03.Test", 230)]
		public void PartTwo(string fileName, long expectedResult)
		{
			// Arrange.
			_day.LoadInput(fileName);

			// Act.
			long result = _day.PartTwo();

			// Assert.
			Assert.AreEqual(230, result);
		}
	}
}
