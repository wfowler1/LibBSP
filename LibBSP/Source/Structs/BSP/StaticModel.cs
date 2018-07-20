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
	using Vector3 = Vector3d;
#endif
	/// <summary>
	/// Handles the data needed for a static model object from MoHAA.
	/// </summary>
	public struct StaticModel {

		public string name { get; private set; }
		public Vector3 origin { get; private set; }
		public Vector3 angles { get; private set; }
		public float scale { get; private set; }
		[Index("vertices")] public int firstVertex { get; private set; }
		[Count("vertices")] public short numVertices { get; private set; }

		/// <summary>
		/// Creates a new <see cref="StaticModel"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of static prop lump this object is a member of.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public StaticModel(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			name = "";
			origin = Vector3.zero;
			angles = Vector3.zero;
			scale = 0;
			switch (type) {
				case MapType.MOHAA: {
					byte[] nameBytes = new byte[128];
					Array.Copy(data, 0, nameBytes, 0, 128);
					name = nameBytes.ToNullTerminatedString();
					origin = new Vector3(BitConverter.ToSingle(data, 128), BitConverter.ToSingle(data, 132), BitConverter.ToSingle(data, 136));
					angles = new Vector3(BitConverter.ToSingle(data, 140), BitConverter.ToSingle(data, 144), BitConverter.ToSingle(data, 148));
					scale = BitConverter.ToSingle(data, 152);
					firstVertex = BitConverter.ToInt32(data, 156);
					numVertices = BitConverter.ToInt16(data, 160);
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the StaticModel class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="StaticModel"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="StaticModel"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<StaticModel> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.MOHAA: {
					structLength = 164;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the StaticModel lump factory.");
				}
			}
			List<StaticModel> lump = new List<StaticModel>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new StaticModel(bytes, type, version));
			}
			return lump;
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.MOHAA: {
					return 25;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
