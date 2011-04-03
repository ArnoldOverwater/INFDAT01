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
			graph = new UndirectionalGraph<UInt16, Single>(1.0f);
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

		[TestMethod]
		public void TestMethod2() {
			graph.Remove(2);
			Assert.AreEqual(graph.Count, 1);
		}

		[TestMethod]
		public void TestMethod3() {
			graph.Add(16);
			Assert.AreEqual(graph.Count, 3);
			Assert.AreEqual(graph.IndexOf(2), 1);
			Assert.AreEqual(graph[2], 16);
		}

		[TestMethod]
		public void TestMethod4() {
			Assert.IsTrue(graph.IndexOf(3) < 0);
			Assert.IsFalse(graph.Contains(3));
			try {
				graph[5].ToString();
				Assert.Fail();
			} catch (Exception e) {
				Assert.IsNotInstanceOfType(e, typeof(AssertFailedException));
			}
		}

		[TestMethod]
		public void TestMethod5() {
			graph.Add(4);
			graph.Add(8);
			graph.Add(16);
			graph.Add(32);
			graph.Add(64);
			graph.Add(128);
			graph.Add(256);
			Assert.AreEqual(graph.GetVertex(2, 5), 1.0f);
			graph.SetVertex(2, 5, 0.5f);
			Assert.AreEqual(graph.GetVertex(2, 5), 0.5f);
			Assert.AreEqual(graph.GetVertex(5, 2), 0.5f);
			graph.RemoveAt(2);
			Assert.AreEqual(graph.GetVertex(2, 5), 1.0f);
		}

	}
}
