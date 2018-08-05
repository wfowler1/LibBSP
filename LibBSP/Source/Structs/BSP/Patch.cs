#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if !UNITY
	using Vector2 = Vector2d;
#endif

	/// <summary>
	/// Holds the data for a patch in a CoD BSP.
	/// </summary>
	public struct Patch {

		public short shader { get; private set; }
		public short type { get; private set; }
		public Vector2 dimensions { get; private set; }
		public int flags { get; private set; }
		[Index("patchVerts")] public int firstVertex { get; private set; }
		[Count("patchVerts")] public int numVertices { get; private set; }
		[Count("patchIndices")] public int numIndices { get; private set; }
		[Index("patchIndices")] public int firstIndex { get; private set; }

		/// <summary>
		/// Creates a new <see cref="Patch"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public Patch(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			switch (type) {
				case MapType.CoD: {
					shader = BitConverter.ToInt16(data, 0);
					this.type = BitConverter.ToInt16(data, 2);
					if (this.type == 0) { // Patch
						short x = BitConverter.ToInt16(data, 4);
						short y = BitConverter.ToInt16(data, 6);
						dimensions = new Vector2(x, y);
						flags = BitConverter.ToInt32(data, 8);
						firstVertex = BitConverter.ToInt32(data, 12);
						numVertices = x * y;
					} else if (this.type == 1) { // Terrain
						numVertices = BitConverter.ToInt16(data, 4);
						numIndices = BitConverter.ToInt16(data, 6);
						firstVertex = BitConverter.ToInt32(data, 8);
						firstIndex = BitConverter.ToInt32(data, 12);
					}
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Patch class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="Patch"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="Patch"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<Patch> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.CoD: {
					structLength = 16;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Patch lump factory.");
				}
			}
			List<Patch> lump = new List<Patch>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Patch(bytes, type, version));
			}
			return lump;
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.CoD: {
					return 24;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
