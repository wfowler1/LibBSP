#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Reflection;

namespace LibBSP {
#if UNITY
	using Color = UnityEngine.Color32;
	using Vector2 = UnityEngine.Vector2;
#elif GODOT
	using Color = Godot.Color;
	using Vector2 = Godot.Vector2;
#else
	using Color = System.Drawing.Color;
	using Vector2 = System.Numerics.Vector2;
#endif

	/// <summary>
	/// Contains all the information for a single Texture Data object.
	/// </summary>
	public struct TextureData : ILumpObject {

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

		/// <summary>
		/// Gets or sets the reflectivity color of this <see cref="TextureData"/>.
		/// </summary>
		public Color Reflectivity {
			get {
				return ColorExtensions.FromArgb((int)(BitConverter.ToSingle(Data, 0) * 255), (int)(BitConverter.ToSingle(Data, 4) * 255), (int)(BitConverter.ToSingle(Data, 8) * 255), 255);
			}
			set {
				float r = value.R() / 255f;
				float g = value.G() / 255f;
				float b = value.B() / 255f;
				value.GetBytes().CopyTo(Data, 0);
			}
		}

		/// <summary>
		/// Gets the offset into <see cref="BSP.textures"/> for the texture name for this <see cref="TextureData"/>.
		/// </summary>
		public uint TextureStringOffset {
			get {
				return (uint)Parent.Bsp.texTable[TextureStringOffsetIndex];
			}
		}
		
		/// <summary>
		/// Gets or sets the index into <see cref="BSP.texTable"/>, which is an offset into <see cref="BSP.textures"/> for
		/// the texture name for this <see cref="TextureData"/>.
		/// </summary>
		public int TextureStringOffsetIndex {
			get {
				return BitConverter.ToInt32(Data, 12);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(Data, 12);
			}
		}
		
		/// <summary>
		/// Gets or sets the actual size of the <see cref="Texture"/> referenced by this <see cref="TextureData"/>.
		/// </summary>
		public Vector2 Size {
			get {
				return new Vector2(BitConverter.ToInt32(Data, 16), BitConverter.ToInt32(Data, 20));
			}
			set {
				int width = (int)value.X();
				int height = (int)value.Y();
				BitConverter.GetBytes(width).CopyTo(Data, 16);
				BitConverter.GetBytes(height).CopyTo(Data, 20);
			}
		}

		/// <summary>
		/// Gets or sets the internal size of the <see cref="Texture"/> referenced by this <see cref="TextureData"/>.
		/// </summary>
		public Vector2 ViewSize {
			get {
				return new Vector2(BitConverter.ToInt32(Data, 24), BitConverter.ToInt32(Data, 28));
			}
			set {
				int width = (int)value.X();
				int height = (int)value.Y();
				BitConverter.GetBytes(width).CopyTo(Data, 24);
				BitConverter.GetBytes(height).CopyTo(Data, 28);
			}
		}

		/// <summary>
		/// Creates a new <see cref="TextureData"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="parent">The <see cref="ILump"/> this <see cref="TextureData"/> came from.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public TextureData(byte[] data, ILump parent = null) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Parent = parent;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Lump{TextureData}"/>.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <returns>A <see cref="Lump{TextureData}"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> parameter was <c>null</c>.</exception>
		public static Lump<TextureData> LumpFactory(byte[] data, BSP bsp, LumpInfo lumpInfo) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			return new Lump<TextureData>(data, GetStructLength(bsp.version, lumpInfo.version), bsp, lumpInfo);
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
					return 32;
				}
				case MapType.Titanfall: {
					return 36;
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
