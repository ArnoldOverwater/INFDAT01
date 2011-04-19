// Arnold Overwater - 0821508 - INF2A

using System;
using System.Collections.Generic;

namespace Collection.Graph {

	/// <summary>
	/// This is the interface for a (read-only) graph.
	/// A graph contains edges and weighted vertices.
	/// It also implements IList, which means it's essentailly a list of edges.
	/// Since it is a list it is index-based.
	/// The methods will throw exceptions if the indeces or parameters are incorrect.
	/// </summary>
	/// <typeparam name="E">
	/// The edge type, which is also the type-param of the list.
	/// </typeparam>
	/// <typeparam name="V">
	/// The vertex type.
	/// The graph assumes all edges have one vertex to each other vertex including itself.
	/// If for example you want to test wether or not two edges are adjecent you should use bool for this type.
	/// </typeparam>
	interface IGraph<E, V> : IList<E> {

		// Get the vertex from one edge index to the other.
		V GetVertex(int from, int to);

		// Get an array containing all vertices in order from the specified edge index.
		V[] GetVerticesFrom(int index);

		// Get an array containing all vertices in order to the specified edge index.
		V[] GetVerticesTo(int index);

		// Get the vertex from one edge to the other.
		V GetVertexWithEdge(E from, E to);

		// Get an array containing all vertices in order from the specified edge.
		V[] GetVerticesFromEdge(E edge);

		// Get an array containing all vertices in order to the specified edge.
		V[] GetVerticesToEdge(E edge);

	}

}
