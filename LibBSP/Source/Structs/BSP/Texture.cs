#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Text;

namespace LibBSP {
#if UNITY
	using Vector2d = UnityEngine.Vector2;
	using Vector3d = UnityEngine.Vector3;
#elif GODOT
	using Vector2d = Godot.Vector2;
	using Vector3d = Godot.Vector3;
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
	public struct Texture : ILumpObject {

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

		public string name {
			get {
				switch (MapType) {
					case MapType.Quake: {
						return Data.ToNullTerminatedString(0, 16);
					}
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.FAKK:
					case MapType.MOHAA:
					case MapType.Nightfire: {
						return Data.ToNullTerminatedString(0, 64);
					}
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana: {
						return Data.ToNullTerminatedString(40, 32);
					}
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.TacticalInterventionEncrypted:
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						return Data.ToRawString();
					}
					case MapType.SiN: {
						return Data.ToNullTerminatedString(36, 64);
					}
					default: {
						return null;
					}
				}
			}
			set {
				byte[] bytes = Encoding.ASCII.GetBytes(value);
				switch (MapType) {
					case MapType.Quake: {
						for (int i = 0; i < 16; ++i) {
							Data[i] = 0;
						}
						Array.Copy(bytes, 0, Data, 0, Math.Min(bytes.Length, 15));
						break;
					}
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.FAKK:
					case MapType.MOHAA:
					case MapType.Nightfire: {
						for (int i = 0; i < 64; ++i) {
							Data[i] = 0;
						}
						Array.Copy(bytes, 0, Data, 0, Math.Min(bytes.Length, 63));
						break;
					}
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana: {
						for (int i = 0; i < 32; ++i) {
							Data[i + 40] = 0;
						}
						Array.Copy(bytes, 0, Data, 40, Math.Min(bytes.Length, 31));
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
					case MapType.L4D2:
					case MapType.TacticalInterventionEncrypted:
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						Data = bytes;
						break;
					}
					case MapType.SiN: {
						for (int i = 0; i < 64; ++i) {
							Data[i + 36] = 0;
						}
						Array.Copy(bytes, 0, Data, 36, Math.Min(bytes.Length, 63));
						break;
					}
				}
			}
		}

		public string mask {
			get {
				switch (MapType) {
					case MapType.MOHAA: {
						return Data.ToNullTerminatedString(76, 64);
					}
					default: {
						return null;
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.MOHAA: {
						for (int i = 0; i < 64; ++i) {
							Data[i + 76] = 0;
						}
						byte[] strBytes = Encoding.ASCII.GetBytes(value);
						Array.Copy(strBytes, 0, Data, 76, Math.Min(strBytes.Length, 63));
						break;
					}
				}
			}
		}

		public int flags {
			get {
				switch (MapType) {
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana:
					case MapType.SiN: {
						return BitConverter.ToInt32(Data, 32);
					}
					case MapType.MOHAA:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.FAKK: {
						return BitConverter.ToInt32(Data, 64);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana:
					case MapType.SiN: {
						bytes.CopyTo(Data, 32);
						break;
					}
					case MapType.MOHAA:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.FAKK: {
						bytes.CopyTo(Data, 64);
						break;
					}
				}
			}
		}
		
		public int contents {
			get {
				switch (MapType) {
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.FAKK:
					case MapType.MOHAA: {
						return BitConverter.ToInt32(Data, 68);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.FAKK:
					case MapType.MOHAA: {
						bytes.CopyTo(Data, 68);
						break;
					}
				}
			}
		}
		
		public TextureInfo texAxes {
			get {
				switch (MapType) {
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana:
					case MapType.SiN: {
						return new TextureInfo(new Vector3d(BitConverter.ToSingle(Data, 0), BitConverter.ToSingle(Data, 4), BitConverter.ToSingle(Data, 8)),
						                       new Vector3d(BitConverter.ToSingle(Data, 16), BitConverter.ToSingle(Data, 20), BitConverter.ToSingle(Data, 24)),
						                       new Vector2d(BitConverter.ToSingle(Data, 12), BitConverter.ToSingle(Data, 28)),
						                       new Vector2d(1, 1),
						                       -1, -1, 0);
					}
					default: {
						return new TextureInfo();
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana:
					case MapType.SiN: {
						byte[] bytes = value.uAxis.GetBytes();
						bytes.CopyTo(Data, 0);
						bytes = value.vAxis.GetBytes();
						bytes.CopyTo(Data, 16);
						bytes = BitConverter.GetBytes(value.translation.x);
						bytes.CopyTo(Data, 12);
						bytes = BitConverter.GetBytes(value.translation.y);
						bytes.CopyTo(Data, 28);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Texture"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="parent">The <see cref="ILump"/> this <see cref="Texture"/> came from.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Texture(byte[] data, ILump parent) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Parent = parent;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Textures"/> object.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <returns>A <see cref="Textures"/> object.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> parameter was <c>null</c>.</exception>
		public static Textures LumpFactory(byte[] data, BSP bsp, LumpInfo lumpInfo) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			return new Textures(data, GetStructLength(bsp.version, lumpInfo.version), bsp, lumpInfo);
		}

		/// <summary>
		/// Depending on format, this is a variable length structure. Return -1. The <see cref="Textures"/> class will handle object creation.
		/// </summary>
		/// <param name="mapType">The <see cref="LibBSP.MapType"/> of the BSP.</param>
		/// <param name="lumpVersion">The version number for the lump.</param>
		/// <returns>-1</returns>
		public static int GetStructLength(MapType type, int version) {
			return -1;
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
				case MapType.L4D2:
				case MapType.DMoMaM:
				case MapType.Titanfall: {
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
