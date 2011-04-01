// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;

namespace Collection.Graph {

	interface IGraph<E, V> : IList<E> {

		V GetVertex(int from, int to);

		V[] GetVerticesFrom(int index);

		V[] GetVerticesTo(int index);

	}

}
