#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Text;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if UNITY
	using Vector2d = Vector2;
	using Vector3d = Vector3;
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

		public byte[] data;
		public MapType type;
		public int version;

		public string name {
			get {
				switch (type) {
					case MapType.Quake: {
						return data.ToNullTerminatedString(0, 16);
					}
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.FAKK:
					case MapType.Nightfire: {
						return data.ToNullTerminatedString(0, 64);
					}
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana: {
						return data.ToNullTerminatedString(40, 32);
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
					case MapType.DMoMaM: {
						return data.ToRawString();
					}
					case MapType.SiN: {
						return data.ToNullTerminatedString(36, 64);
					}
					default: {
						return null;
					}
				}
			}
			set {
				byte[] bytes = Encoding.ASCII.GetBytes(value);
				switch (type) {
					case MapType.Quake: {
						for (int i = 0; i < 16; ++i) {
							data[i] = 0;
						}
						Array.Copy(bytes, 0, data, 0, Math.Min(bytes.Length, 15));
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
					case MapType.Nightfire: {
						for (int i = 0; i < 64; ++i) {
							data[i] = 0;
						}
						Array.Copy(bytes, 0, data, 0, Math.Min(bytes.Length, 63));
						break;
					}
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana: {
						for (int i = 0; i < 32; ++i) {
							data[i + 40] = 0;
						}
						Array.Copy(bytes, 0, data, 40, Math.Min(bytes.Length, 31));
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
					case MapType.DMoMaM: {
						data = bytes;
						break;
					}
					case MapType.SiN: {
						for (int i = 0; i < 64; ++i) {
							data[i + 36] = 0;
						}
						Array.Copy(bytes, 0, data, 36, Math.Min(bytes.Length, 63));
						break;
					}
				}
			}
		}

		public string mask {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return data.ToNullTerminatedString(76, 64);
					}
					default: {
						return null;
					}
				}
			}
			set {
				switch (type) {
					case MapType.MOHAA: {
						for (int i = 0; i < 64; ++i) {
							data[i + 76] = 0;
						}
						byte[] strBytes = Encoding.ASCII.GetBytes(value);
						Array.Copy(strBytes, 0, data, 76, Math.Min(strBytes.Length, 63));
						break;
					}
				}
			}
		}

		public int flags {
			get {
				switch (type) {
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana:
					case MapType.SiN: {
						return BitConverter.ToInt32(data, 32);
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
						return BitConverter.ToInt32(data, 64);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana:
					case MapType.SiN: {
						bytes.CopyTo(data, 32);
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
						bytes.CopyTo(data, 64);
						break;
					}
				}
			}
		}
		
		public int contents {
			get {
				switch (type) {
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.FAKK:
					case MapType.MOHAA: {
						return BitConverter.ToInt32(data, 68);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.FAKK:
					case MapType.MOHAA: {
						bytes.CopyTo(data, 68);
						break;
					}
				}
			}
		}
		
		public TextureInfo texAxes {
			get {
				switch (type) {
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana:
					case MapType.SiN: {
						return new TextureInfo(new Vector3d(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8)),
						                       new Vector3d(BitConverter.ToSingle(data, 16), BitConverter.ToSingle(data, 20), BitConverter.ToSingle(data, 24)),
						                       new Vector2d(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 28)),
						                       Vector2d.one,
						                       -1, -1, 0);
					}
					default: {
						return null;
					}
				}
			}
			set {
				switch (type) {
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.Daikatana:
					case MapType.SiN: {
						byte[] bytes = value.uAxis.GetBytes();
						bytes.CopyTo(data, 0);
						bytes = value.vAxis.GetBytes();
						bytes.CopyTo(data, 16);
						bytes = BitConverter.GetBytes(value.translation.x);
						bytes.CopyTo(data, 12);
						bytes = BitConverter.GetBytes(value.translation.y);
						bytes.CopyTo(data, 28);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Texture"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Texture(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Textures"/> object.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <see cref="Textures"/> object.</returns>
		public static Textures LumpFactory(byte[] data, MapType type, int version = 0) {
			return new Textures(data, type, version);
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
