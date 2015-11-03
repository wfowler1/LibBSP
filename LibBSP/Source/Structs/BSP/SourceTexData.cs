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
	/// Contains all the information for a single SourceTexData object
	/// </summary>
	public struct SourceTexData {

		public Vector3 reflectivity { get; private set; }
		public int stringTableIndex { get; private set; }
		public int width { get; private set; }
		public int height { get; private set; }
		public int view_width { get; private set; }
		public int view_height { get; private set; }

		/// <summary>
		/// Creates a new <c>SourceTexData</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public SourceTexData(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			reflectivity = new Vector3(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
			stringTableIndex = BitConverter.ToInt32(data, 12);
			width = BitConverter.ToInt32(data, 16);
			height = BitConverter.ToInt32(data, 20);
			view_width = BitConverter.ToInt32(data, 24);
			view_height = BitConverter.ToInt32(data, 28);
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <c>SourceTexData</c> objects.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>List</c> of <c>SourceTexData</c> objects</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public static List<SourceTexData> LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 32;
			List<SourceTexData> lump = new List<SourceTexData>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; i++) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new SourceTexData(bytes, type));
			}
			return lump;
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.Vindictus:
				case MapType.TacticalInterventionEncrypted:
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.DMoMaM: {
					return 2;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
