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
	/// Contains all the information for a single Texture Data object.
	/// </summary>
	public struct TextureData {

		public byte[] data;
		public MapType type;
		public int version;

		public Vector3d reflectivity {
			get {
				return new Vector3d(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
			}
			set {
				value.GetBytes().CopyTo(data, 0);
			}
		}
		
		public int stringTableIndex {
			get {
				return BitConverter.ToInt32(data, 12);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 12);
			}
		}
		
		public int width {
			get {
				return BitConverter.ToInt32(data, 16);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 16);
			}
		}
		
		public int height {
			get {
				return BitConverter.ToInt32(data, 20);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 20);
			}
		}
		
		public int view_width {
			get {
				return BitConverter.ToInt32(data, 24);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 24);
			}
		}
		
		public int view_height {
			get {
				return BitConverter.ToInt32(data, 28);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 28);
			}
		}

		/// <summary>
		/// Creates a new <see cref="TextureData"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public TextureData(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="TextureData"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="TextureData"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public static List<TextureData> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 32;
			if (type == MapType.Titanfall) {
				structLength = 36;
			}
			int numObjects = data.Length / structLength;
			List<TextureData> lump = new List<TextureData>(numObjects);
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new TextureData(bytes, type, version));
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
				case MapType.DMoMaM:
				case MapType.Titanfall: {
					return 2;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
