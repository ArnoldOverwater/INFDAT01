// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;

namespace Collection.Graph {

	public abstract class Graph<E, V> : IGraph<E, V> {

		public V GetVertex(int from, int to) {
			throw new NotImplementedException();
		}

		public V[] GetVerticesFrom(int index) {
			throw new NotImplementedException();
		}

		public V[] GetVerticesTo(int index) {
			throw new NotImplementedException();
		}

		public int IndexOf(E item) {
			throw new NotImplementedException();
		}

		public void Insert(int index, E item) {
			throw new NotImplementedException();
		}

		public void RemoveAt(int index) {
			throw new NotImplementedException();
		}

		public E this[int index] {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}

		public void Add(E item) {
			throw new NotImplementedException();
		}

		public void Clear() {
			throw new NotImplementedException();
		}

		public bool Contains(E item) {
			throw new NotImplementedException();
		}

		public void CopyTo(E[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		public int Count {
			get { throw new NotImplementedException(); }
		}

		public bool IsReadOnly {
			get { throw new NotImplementedException(); }
		}

		public bool Remove(E item) {
			throw new NotImplementedException();
		}

		public IEnumerator<E> GetEnumerator() {
			throw new NotImplementedException();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			throw new NotImplementedException();
		}
	}

}
