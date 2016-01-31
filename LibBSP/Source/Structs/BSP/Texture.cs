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
	/// An all-encompassing class to handle the texture information of any given BSP format.
	/// </summary>
	/// <remarks>
	/// The way texture information is stored varies wildly between versions. As a general
	/// rule, this class only handles the lump containing the string of a texture's name,
	/// and data from within the lump associated with it.
	/// For example, Nightfire's texture lump only contains 64-byte null-padded strings, but
	/// Quake 2's has texture scaling included.
	/// </remarks>
	public struct Texture {

		public string name { get; private set; }
		public string mask { get; private set; } // Only used by MoHAA, "ignore" means it's unused
		public int flags { get; private set; }
		public int contents { get; private set; }
		public TextureInfo texAxes { get; private set; }

		/// <summary>
		/// Creates a new <see cref="Texture"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public Texture(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			name = "";
			mask = "ignore";
			flags = 0;
			contents = 0;
			switch (type) {
				case MapType.Quake:
				case MapType.Nightfire: {
					name = data.ToNullTerminatedString();
					break;
				}
				case MapType.Quake2:
				case MapType.SoF:
				case MapType.Daikatana: {
					texAxes = new TextureInfo(new Vector3(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8)), BitConverter.ToSingle(data, 12), new Vector3(BitConverter.ToSingle(data, 16), BitConverter.ToSingle(data, 20), BitConverter.ToSingle(data, 24)), BitConverter.ToSingle(data, 28), -1, -1);
					flags = BitConverter.ToInt32(data, 32);
					name = data.ToNullTerminatedString(40, 32);
					break;
				}
				case MapType.MOHAA: {
					mask = data.ToNullTerminatedString(76, 64);
					goto case MapType.STEF2;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.Raven:
				case MapType.Quake3:
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4:
				case MapType.FAKK: {
					name = data.ToNullTerminatedString(0, 64);
					flags = BitConverter.ToInt32(data, 64);
					contents = BitConverter.ToInt32(data, 68);
					break;
				}
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.TacticalInterventionEncrypted:
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					name = data.ToRawString();
					break;
				}
				case MapType.SiN: {
					texAxes = new TextureInfo(new Vector3(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8)), BitConverter.ToSingle(data, 12), new Vector3(BitConverter.ToSingle(data, 16), BitConverter.ToSingle(data, 20), BitConverter.ToSingle(data, 24)), BitConverter.ToSingle(data, 28), -1, -1);
					flags = BitConverter.ToInt32(data, 32);
					name = data.ToNullTerminatedString(36, 64);
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Texture class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Textures"/> object.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <returns>A <see cref="Textures"/> object.</returns>
		public static Textures LumpFactory(byte[] data, MapType type) {
			return new Textures(data, type);
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4:
				case MapType.FAKK:
				case MapType.MOHAA:
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 0;
				}
				case MapType.Raven:
				case MapType.Quake3: {
					return 1;
				}
				case MapType.Quake:
				case MapType.Nightfire: {
					return 2;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 5;
				}
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
					return 43;
				}
				default: {
					return -1;
				}
			}
		}

		/// <summary>
		/// Gets the index for the materials lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForMaterialLump(MapType type) {
			switch (type) {
				case MapType.Nightfire: {
					return 3;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
