#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP.Doom {
#if !UNITY
	using Vector3 = Vector3d;
#endif
	/// <summary>
	/// Holds all the data for a node in a Doom map.
	/// </summary>
	/// <remarks>
	/// This is the one lump that has a structure similar to future BSPs.
	/// </remarks>
	public struct Node {
		// This format uses a vector head and tail for partitioning, rather
		// than the 3D plane conventionally used by more modern engines.
		// The "tail" is actually a change in X and Y, rather than an explicitly defined point.
		public Vector3 vecHead { get; private set; }
		public Vector3 vecTail { get; private set; }
		// These rectangles are defined by
		// Top, Bottom, Left, Right. That's YMax, YMin, XMin, XMax, in that order
		public Rect RRectangle  { get; private set; }
		public Rect LRectangle { get; private set; }
		public short RChild { get; private set; }
		public short LChild { get; private set; }

		/// <summary>
		/// Creates a new <c>Node</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public Node(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			vecHead = new Vector3(BitConverter.ToInt16(data, 0), BitConverter.ToInt16(data, 2));
			vecTail = new Vector3(BitConverter.ToInt16(data, 4), BitConverter.ToInt16(data, 6));
			RRectangle = Rect.MinMaxRect(BitConverter.ToInt16(data, 12), BitConverter.ToInt16(data, 8), BitConverter.ToInt16(data, 14), BitConverter.ToInt16(data, 10));
			LRectangle = Rect.MinMaxRect(BitConverter.ToInt16(data, 20), BitConverter.ToInt16(data, 16), BitConverter.ToInt16(data, 22), BitConverter.ToInt16(data, 18));
			this.RChild = BitConverter.ToInt16(data, 24);
			this.LChild = BitConverter.ToInt16(data, 26);
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <c>Node</c> objects.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>List</c> of <c>Node</c> objects</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public static List<Node> LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 28;
			List<Node> lump = new List<Node>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, i * structLength, bytes, 0, structLength);
				lump.Add(new Node(bytes, type));
			}
			return lump;
		}
	}
}
