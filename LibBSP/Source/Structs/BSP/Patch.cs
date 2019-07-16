#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Reflection;

namespace LibBSP {
#if UNITY
	using Vector2d = UnityEngine.Vector2;
#elif GODOT
	using Vector2d = Godot.Vector2;
#endif

	/// <summary>
	/// Holds the data for a patch in a CoD BSP.
	/// </summary>
	public struct Patch : ILumpObject {

		/// <summary>
		/// The <see cref="ILump"/> this <see cref="ILumpObject"/> came from.
		/// </summary>
		public ILump Parent { get; private set; }

		/// <summary>
		/// Array of <c>byte</c>s used as the data source for this <see cref="ILumpObject"/>.
		/// </summary>
		public byte[] Data { get; private set; }

		/// <summary>
		/// The <see cref="LibBSP.MapType"/> to use to interpret <see cref="Data"/>.
		/// </summary>
		public MapType MapType {
			get {
				if (Parent == null || Parent.Bsp == null) {
					return MapType.Undefined;
				}
				return Parent.Bsp.version;
			}
		}

		/// <summary>
		/// The version number of the <see cref="ILump"/> this <see cref="ILumpObject"/> came from.
		/// </summary>
		public int LumpVersion {
			get {
				if (Parent == null) {
					return 0;
				}
				return Parent.LumpInfo.version;
			}
		}

		public short shader {
			get {
				switch (MapType) {
					case MapType.CoD: {
						return BitConverter.ToInt16(Data, 0);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD: {
						bytes.CopyTo(Data, 0);
						break;
					}
				}
			}
		}
		
		public short patchType {
			get {
				switch (MapType) {
					case MapType.CoD: {
						return BitConverter.ToInt16(Data, 2);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD: {
						bytes.CopyTo(Data, 2);
						break;
					}
				}
			}
		}
		
		public Vector2d dimensions {
			get {
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 0) {
							return new Vector2d(BitConverter.ToInt16(Data, 4), BitConverter.ToInt16(Data, 6));
						} else {
							return new Vector2d(float.NaN, float.NaN);
						}
					}
					default: {
						return new Vector2d(float.NaN, float.NaN);
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 0) {
							BitConverter.GetBytes((short)value.x).CopyTo(Data, 4);
							BitConverter.GetBytes((short)value.y).CopyTo(Data, 6);
						}
						break;
					}
				}
			}
		}
		
		public int flags {
			get {
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 0) {
							return BitConverter.ToInt32(Data, 8);
						} else {
							return -1;
						}
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 0) {
							bytes.CopyTo(Data, 8);
						}
						break;
					}
				}
			}
		}
		
		[Index("patchVerts")] public int firstVertex {
			get {
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 0) {
							return BitConverter.ToInt32(Data, 12);
						} else if (patchType == 1) {
							return BitConverter.ToInt32(Data, 8);
						} else {
							return -1;
						}
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 0) {
							bytes.CopyTo(Data, 12);
						} else if (patchType == 1) {
							bytes.CopyTo(Data, 8);
						}
						break;
					}
				}
			}
		}
		
		[Count("patchVerts")] public int numVertices {
			get {
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 0) {
							return BitConverter.ToInt16(Data, 4) * BitConverter.ToInt16(Data, 6);
						} else if (patchType == 1) {
							return BitConverter.ToInt16(Data, 4);
						} else {
							return -1;
						}
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 1) {
							Data[4] = bytes[0];
							Data[5] = bytes[1];
						}
						break;
					}
				}
			}
		}
		
		[Count("patchIndices")] public int numIndices {
			get {
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 1) {
							return BitConverter.ToInt16(Data, 6);
						} else {
							return -1;
						}
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 1) {
							Data[6] = bytes[0];
							Data[7] = bytes[1];
						}
						break;
					}
				}
			}
		}
		
		[Index("patchIndices")] public int firstIndex {
			get {
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 1) {
							return BitConverter.ToInt32(Data, 12);
						} else {
							return -1;
						}
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD: {
						if (patchType == 1) {
							bytes.CopyTo(Data, 12);
						}
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Patch"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="parent">The <see cref="ILump"/> this <see cref="Patch"/> came from.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Patch(byte[] data, ILump parent = null) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Parent = parent;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Lump{Patch}"/>.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <returns>A <see cref="Lump{Patch}"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> parameter was <c>null</c>.</exception>
		public static Lump<Patch> LumpFactory(byte[] data, BSP bsp, LumpInfo lumpInfo) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			return new Lump<Patch>(data, GetStructLength(bsp.version, lumpInfo.version), bsp, lumpInfo);
		}

		/// <summary>
		/// Gets the length of this struct's data for the given <paramref name="mapType"/> and <paramref name="lumpVersion"/>.
		/// </summary>
		/// <param name="mapType">The <see cref="LibBSP.MapType"/> of the BSP.</param>
		/// <param name="lumpVersion">The version number for the lump.</param>
		/// <returns>The length, in <c>byte</c>s, of this struct.</returns>
		/// <exception cref="ArgumentException">This struct is not valid or is not implemented for the given <paramref name="mapType"/> and <paramref name="lumpVersion"/>.</exception>
		public static int GetStructLength(MapType mapType, int lumpVersion = 0) {
			switch (mapType) {
				case MapType.CoD: {
					return 16;
				}
				default: {
					throw new ArgumentException("Lump object " + MethodBase.GetCurrentMethod().DeclaringType.Name + " does not exist in map type " + mapType + " or has not been implemented.");
				}
			}
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
