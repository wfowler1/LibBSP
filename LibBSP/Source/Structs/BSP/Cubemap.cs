#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if UNITY
	using Vector3d = Vector3;
#endif

	/// <summary>
	/// Holds all data for a Cubemap from Source engine.
	/// </summary>
	public struct Cubemap {

		public byte[] data;
		public MapType type;
		public int version;

		public Vector3d origin {
			get {
				switch (type) {
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.TacticalInterventionEncrypted:
					case MapType.L4D2:
					case MapType.Vindictus:
					case MapType.DMoMaM: {
						return new Vector3d(BitConverter.ToInt32(data, 0), BitConverter.ToInt32(data, 4), BitConverter.ToInt32(data, 8));
					}
					default: {
						return new Vector3d(float.NaN, float.NaN, float.NaN);
					}
				}
			}
			set {
				switch (type) {
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.TacticalInterventionEncrypted:
					case MapType.L4D2:
					case MapType.Vindictus:
					case MapType.DMoMaM: {
						value.GetBytes().CopyTo(data, 0);
						break;
					}
				}
			}
		}

		public int size {
			get {
				switch (type) {
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.TacticalInterventionEncrypted:
					case MapType.L4D2:
					case MapType.Vindictus:
					case MapType.DMoMaM: {
						return BitConverter.ToInt32(data, 12);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.TacticalInterventionEncrypted:
					case MapType.L4D2:
					case MapType.Vindictus:
					case MapType.DMoMaM: {
						bytes.CopyTo(data, 12);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Cubemap"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public Cubemap(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="Cubemap"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="Cubemap"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<Cubemap> LumpFactory(byte[] data, MapType type, int version = 0) {
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
				case MapType.TacticalInterventionEncrypted:
				case MapType.L4D2:
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					structLength = 16;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the SourceCubemap lump factory.");
				}
			}
			int numObjects = data.Length / structLength;
			List<Cubemap> lump = new List<Cubemap>(numObjects);
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Cubemap(bytes, type, version));
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
				case MapType.L4D2:
				case MapType.DMoMaM: {
					return 42;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
