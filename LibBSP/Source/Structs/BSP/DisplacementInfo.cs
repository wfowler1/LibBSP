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
	/// Holds all data for a Displacement from Source engine.
	/// </summary>
	public struct DisplacementInfo {

		public Vector3 startPosition { get; private set; }
		public int dispVertStart { get; private set; }
		//public int dispTriStart { get; private set; }
		public int power { get; private set; }
		public uint[] allowedVerts { get; private set; }

		/// <summary>
		/// Creates a new <see cref="DisplacementInfo"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public DisplacementInfo(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			startPosition = new Vector3(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
			dispVertStart = BitConverter.ToInt32(data, 12);
			//dispTriStart = BitConverter.ToInt32(in, 16);
			power = BitConverter.ToInt32(data, 20);
			allowedVerts = new uint[10];
			int offset = 0;
			switch (type) {
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source27:
				case MapType.L4D2:
				case MapType.TacticalInterventionEncrypted:
				case MapType.DMoMaM: {
					offset = 136;
					break;
				}
				case MapType.Source22: {
					offset = 140;
					break;
				}
				case MapType.Source23: {
					offset = 144;
					break;
				}
				case MapType.Vindictus: {
					offset = 192;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the SourceDispInfo class.");
				}
			}
			for (int i = 0; i < 10; ++i) {
				allowedVerts[i] = BitConverter.ToUInt32(data, offset + (i * 4));
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="DisplacementInfo"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="DisplacementInfo"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<DisplacementInfo> LumpFactory(byte[] data, MapType type, int version = 0) {
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
				case MapType.Source27:
				case MapType.L4D2:
				case MapType.TacticalInterventionEncrypted:
				case MapType.DMoMaM: {
					structLength = 176;
					break;
				}
				case MapType.Source22: {
					structLength = 180;
					break;
				}
				case MapType.Source23: {
					structLength = 184;
					break;
				}
				case MapType.Vindictus: {
					structLength = 232;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the SourceDispInfo lump factory.");
				}
			}
			int offset = 0;
			List<DisplacementInfo> lump = new List<DisplacementInfo>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new DisplacementInfo(bytes, type, version));
				offset += structLength;
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
					return 26;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
