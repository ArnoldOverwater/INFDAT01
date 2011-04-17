using Counterstrike;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Counterstrike_Test {

	/// <summary>
	///This is a test class for GameTest and is intended
	///to contain all GameTest Unit Tests
	///</summary>
	[TestClass()]
	public class GameTest {

		private static Game game;

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext) {
			game = new Game();
		}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		[ClassCleanup()]
		public static void MyClassCleanup() {
			game = null;
		}
		//
		//Use TestInitialize to run code before running each test
		[TestInitialize()]
		public void MyTestInitialize() {
			game.AddPlayer(new Player("P1"));
			game.AddPlayer(new Player("P2"));
		}
		//
		//Use TestCleanup to run code after each test has run
		[TestCleanup()]
		public void MyTestCleanup() {
			game.Clear();
		}
		//
		#endregion

		[TestMethod()]
		public void TestMethod1() {
			game.AddPlayer(new Player());
			Assert.AreEqual(game.Count, 3);
		}

		[TestMethod()]
		public void TestMethod2() {
			game.KillPlayerIndex(killerIndex: 0, victimIndex: 1);
			Assert.AreEqual(game.GetVertex(from: 0, to: 1), 1);
			Assert.AreEqual(game.GetVertex(from: 1, to: 0), 0);
			Assert.AreEqual(game[0].MatchKills, 1);
			Assert.AreEqual(game[1].MatchKills, 0);
			Assert.AreEqual(game[0].MatchKilled, 0);
			Assert.AreEqual(game[1].MatchKilled, 1);
		}

		[TestMethod()]
		public void TestMethod3() {
			try {
				game.AddPlayer(null);
				Assert.Fail();
			} catch (NullReferenceException) {
				Assert.AreEqual(game.Count, 2);
			}
		}

		[TestMethod()]
		public void TestMethod4() {
			game.KillPlayerIndex(killerIndex: 0, victimIndex: 1);
			game.KillPlayerIndex(killerIndex: 1, victimIndex: 0);
			game.KillPlayerIndex(suicider: 0);
			Player[] array = new Player[2];
			game.CopyTo(array);
			game.EndGame();
			Assert.AreEqual(array[0].TotalKills, 2ul);
			Assert.AreEqual(array[1].TotalKills, 1ul);
			Assert.AreEqual(array[0].TotalKilled, 2ul);
			Assert.AreEqual(array[1].TotalKilled, 1ul);
		}

	}

}
