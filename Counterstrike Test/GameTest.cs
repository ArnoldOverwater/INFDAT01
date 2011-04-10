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
			
		}

	}

}
