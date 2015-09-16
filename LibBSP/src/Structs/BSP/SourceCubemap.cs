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
	/// Holds all data for a Cubemap from Source engine.
	/// </summary>
	public struct SourceCubemap {

		public Vector3 origin { get; private set; }
		public int size { get; private set; }

		/// <summary>
		/// Creates a new <c>SourceCubemap</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public SourceCubemap(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			switch (type) {
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.TacticalIntervention:
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					origin = new Vector3(BitConverter.ToInt32(data, 0), BitConverter.ToInt32(data, 4), BitConverter.ToInt32(data, 8));
					size = BitConverter.ToInt32(data, 12);
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the SourceCubemap class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <c>SourceCubemap</c> objects.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>List</c> of <c>SourceCubemap</c> objects</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public static List<SourceCubemap> LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.TacticalIntervention:
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					structLength = 16;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the SourceCubemap lump factory.");
				}
			}
			int offset = 0;
			List<SourceCubemap> lump = new List<SourceCubemap>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new SourceCubemap(bytes, type));
				offset += structLength;
			}
			return lump;
		}

	}
}
