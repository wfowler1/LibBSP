using System;
using System.Collections.Generic;
using System.Reflection;

namespace LibBSP {

	/// <summary>
	/// Holds the data used by the brush structures of all formats of BSP.
	/// </summary>
	public struct Brush : ILumpObject {

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
		/// Enumerates the <see cref="BrushSide"/>s referenced by this <see cref="Brush"/>.
		/// </summary>
		public IEnumerable<BrushSide> Sides {
			get {
				for (int i = 0; i < NumSides; ++i) {
					yield return Parent.Bsp.brushSides[FirstSideIndex + i];
				}
			}
		}

		/// <summary>
		/// Gets or sets the index of the first side of this <see cref="Brush"/>.
		/// </summary>
		[Index("brushSides")] public int FirstSideIndex {
			get {
				switch (MapType) {
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.Vindictus:
					case MapType.TacticalInterventionEncrypted:
					case MapType.DMoMaM:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK: {
						return BitConverter.ToInt32(Data, 0);
					}
					case MapType.Nightfire:
					case MapType.STEF2: {
						return BitConverter.ToInt32(Data, 4);
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
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.Vindictus:
					case MapType.TacticalInterventionEncrypted:
					case MapType.DMoMaM:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK: {
						bytes.CopyTo(Data, 0);
						break;
					}
					case MapType.Nightfire:
					case MapType.STEF2: {
						bytes.CopyTo(Data, 4);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the count of sides in this <see cref="Brush"/>.
		/// </summary>
		[Count("brushSides")] public int NumSides {
			get {
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						return BitConverter.ToInt16(Data, 0);
					}
					case MapType.STEF2: {
						return BitConverter.ToInt32(Data, 0);
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.Vindictus:
					case MapType.TacticalInterventionEncrypted:
					case MapType.DMoMaM:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK: {
						return BitConverter.ToInt32(Data, 4);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 8);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						Data[0] = bytes[0];
						Data[1] = bytes[1];
						break;
					}
					case MapType.STEF2: {
						bytes.CopyTo(Data, 0);
						break;
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.Vindictus:
					case MapType.TacticalInterventionEncrypted:
					case MapType.DMoMaM:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK: {
						bytes.CopyTo(Data, 4);
						break;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 8);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="LibBSP.Texture"/> referenced by this <see cref="Brush"/>. Quake 3 engines only use this for contents.
		/// </summary>
		public Texture Texture {
			get {
				return Parent.Bsp.textures[TextureIndex];
			}
		}

		/// <summary>
		/// Gets or sets the index of the <see cref="LibBSP.Texture"/> used by this <see cref="Brush"/>. Quake 3 engines only use this for contents.
		/// </summary>
		public int TextureIndex {
			get {
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						return BitConverter.ToInt16(Data, 2);
					}
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2: {
						return BitConverter.ToInt32(Data, 8);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						bytes.CopyTo(Data, 2);
						break;
					}
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2: {
						bytes.CopyTo(Data, 8);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the Contents mask for this <see cref="Brush"/>.
		/// </summary>
		public int Contents {
			get {
				switch (MapType) {
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 0);
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.Vindictus:
					case MapType.TacticalInterventionEncrypted:
					case MapType.DMoMaM: {
						return BitConverter.ToInt32(Data, 8);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 0);
						break;
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.Vindictus:
					case MapType.TacticalInterventionEncrypted:
					case MapType.DMoMaM: {
						bytes.CopyTo(Data, 8);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Brush"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="parent">The <see cref="ILump"/> this <see cref="Brush"/> came from.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Brush(byte[] data, ILump parent = null) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Parent = parent;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Lump{Brush}"/>.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <returns>A <see cref="Lump{Brush}"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> parameter was <c>null</c>.</exception>
		public static Lump<Brush> LumpFactory(byte[] data, BSP bsp, LumpInfo lumpInfo) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			return new Lump<Brush>(data, GetStructLength(bsp.version, lumpInfo.version), bsp, lumpInfo);
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
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.SiN:
				case MapType.SoF:
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
				case MapType.Nightfire:
				case MapType.STEF2:
				case MapType.MOHAA:
				case MapType.STEF2Demo:
				case MapType.Raven:
				case MapType.Quake3:
				case MapType.FAKK: {
					return 12;
				}
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4: {
					return 4;
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
					return 4;
				}
				case MapType.CoD2: {
					return 6;
				}
				case MapType.CoD4:
				case MapType.Raven:
				case MapType.Quake3: {
					return 8;
				}
				case MapType.FAKK: {
					return 11;
				}
				case MapType.MOHAA: {
					return 12;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 13;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 14;
				}
				case MapType.Nightfire: {
					return 15;
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
					return 18;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
