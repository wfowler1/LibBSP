#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Text;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if UNITY
	using Vector3d = Vector3;
#endif

	/// <summary>
	/// Handles the data needed for a static model object from MoHAA.
	/// </summary>
	public struct StaticModel {

		public byte[] data;
		public MapType type;
		public int version;

		public string name {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return data.ToNullTerminatedString(0, 128);
					}
					default: {
						return null;
					}
				}
			}
			set {
				switch (type) {
					case MapType.MOHAA: {
						for (int i = 0; i < 128; ++i) {
							data[i] = 0;
						}
						byte[] strBytes = Encoding.ASCII.GetBytes(value);
						Array.Copy(strBytes, 0, data, 0, Math.Min(strBytes.Length, 127));
						break;
					}
				}
			}
		}
		
		public Vector3d origin {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return new Vector3d(BitConverter.ToSingle(data, 128), BitConverter.ToSingle(data, 132), BitConverter.ToSingle(data, 136));
					}
					default: {
						return new Vector3d(float.NaN, float.NaN, float.NaN);
					}
				}
			}
			set {
				switch (type) {
					case MapType.MOHAA: {
						value.GetBytes().CopyTo(data, 128);
						break;
					}
				}
			}
		}
		
		public Vector3d angles {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return new Vector3d(BitConverter.ToSingle(data, 140), BitConverter.ToSingle(data, 144), BitConverter.ToSingle(data, 148));
					}
					default: {
						return new Vector3d(float.NaN, float.NaN, float.NaN);
					}
				}
			}
			set {
				switch (type) {
					case MapType.MOHAA: {
						value.GetBytes().CopyTo(data, 140);
						break;
					}
				}
			}
		}
		
		public float scale {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return BitConverter.ToSingle(data, 152);
					}
					default: {
						return float.NaN;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.MOHAA: {
						bytes.CopyTo(data, 152);
						break;
					}
				}
			}
		}
		
		[Index("vertices")] public int firstVertex {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return BitConverter.ToInt32(data, 156);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.MOHAA: {
						bytes.CopyTo(data, 156);
						break;
					}
				}
			}
		}
		
		[Count("vertices")] public short numVertices {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return BitConverter.ToInt16(data, 160);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.MOHAA: {
						bytes.CopyTo(data, 160);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="StaticModel"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of static prop lump this object is a member of.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public StaticModel(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
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
			int numObjects = data.Length / structLength;
			List<StaticModel> lump = new List<StaticModel>(numObjects);
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
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
