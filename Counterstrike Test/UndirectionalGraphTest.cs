using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Collection.Graph;

namespace Counterstrike_Test {
	/// <summary>
	/// Summary description for UndirectionalGraphTest
	/// </summary>
	[TestClass]
	public class UndirectionalGraphTest {

		private static Graph<UInt16, Single> graph;

		public UndirectionalGraphTest() {
			
		}

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
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext) {
			graph = new UndirectionalGraph<UInt16, Single>(1.0);
		}
		//
		// Use ClassCleanup to run code after all tests in a class have run
		[ClassCleanup()]
		public static void MyClassCleanup() {
			graph = null;
		}
		//
		// Use TestInitialize to run code before running each test 
		[TestInitialize()]
		public void MyTestInitialize() {
			graph.Add(1);
			graph.Add(2);
		}
		//
		// Use TestCleanup to run code after each test has run
		[TestCleanup()]
		public void MyTestCleanup() {
			graph.Clear();
		}
		//
		#endregion

		[TestMethod]
		public void TestMethod1() {
			Assert.AreEqual(graph.Count, 2);
		}
	}
}
