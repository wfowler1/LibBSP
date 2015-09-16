#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if !UNITY
	using Vector3 = Vector3d;
#endif
	/// <summary>
	/// Holds all the data for a displacement in a Source map.
	/// </summary>
	public struct SourceDispVertex {

		public Vector3 normal { get; private set; } // The normalized vector direction this vertex points from "flat"
		public float dist { get; private set; } // Magnitude of normal, before normalization
		public float alpha { get; private set; } // Alpha value of texture at this vertex

		/// <summary>
		/// Creates a new <c>SourceDispVertex</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public SourceDispVertex(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.normal = new Vector3(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
			this.dist = BitConverter.ToSingle(data, 12);
			this.alpha = BitConverter.ToSingle(data, 16);
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>SourceDispVertices</c> object.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>SourceDispVertices</c> object</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public static SourceDispVertices LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			return new SourceDispVertices(data, type);
		}
	}
}
