#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Reflection;

namespace LibBSP {
#if UNITY
	using Plane = UnityEngine.Plane;
#elif GODOT
	using Plane = Godot.Plane;
#else
	using Plane = System.Numerics.Plane;
#endif

	/// <summary>
	/// Holds the data used by the brush side structures of all formats of BSP.
	/// </summary>
	public struct BrushSide : ILumpObject {

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
		/// Gets the Plane referenced by this <see cref="BrushSide"/>.
		/// </summary>
		public Plane Plane {
			get {
				return Parent.Bsp.planes[PlaneIndex];
			}
		}

		/// <summary>
		/// Gets or sets the index of the Plane used by this <see cref="BrushSide"/>.
		/// </summary>
		public int PlaneIndex {
			get {
				switch (MapType) {
					case MapType.SiN:
					case MapType.Quake2:
					case MapType.Daikatana:
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
					case MapType.DMoMaM: {
						return BitConverter.ToUInt16(Data, 0);
					}
					case MapType.Raven:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 0);
					}
					case MapType.STEF2:
					case MapType.Nightfire: {
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
					case MapType.SiN:
					case MapType.Quake2:
					case MapType.Daikatana:
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
					case MapType.DMoMaM: {
						Data[0] = bytes[0];
						Data[1] = bytes[1];
						break;
					}
					case MapType.Raven:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 0);
						break;
					}
					case MapType.STEF2:
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 4);
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// In Call of Duty based maps, gets or sets the distance of this <see cref="BrushSide"/> from its axis.
		/// </summary>
		public float Distance {
			get {
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						return BitConverter.ToSingle(Data, 0);
					}
					default: {
						return float.NaN;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						bytes.CopyTo(Data, 0);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="LibBSP.Texture"/> referenced by this <see cref="BrushSide"/>.
		/// </summary>
		public Texture Texture {
			get {
				return Parent.Bsp.textures[TextureIndex];
			}
		}

		/// <summary>
		/// Gets or sets the index of the <see cref="LibBSP.Texture"/> used by this <see cref="BrushSide"/>.
		/// </summary>
		public int TextureIndex {
			get {
				switch (MapType) {
					case MapType.STEF2: {
						return BitConverter.ToInt32(Data, 0);
					}
					case MapType.SiN:
					case MapType.Quake2:
					case MapType.Daikatana:
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
					case MapType.DMoMaM: {
						return BitConverter.ToInt16(Data, 2);
					}
					case MapType.Vindictus:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.Raven: {
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
					case MapType.STEF2: {
						bytes.CopyTo(Data, 0);
						break;
					}
					case MapType.SiN:
					case MapType.Quake2:
					case MapType.Daikatana:
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
					case MapType.DMoMaM: {
						Data[2] = bytes[0];
						Data[3] = bytes[1];
						break;
					}
					case MapType.Vindictus:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.Raven: {
						bytes.CopyTo(Data, 4);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="LibBSP.Face"/> referenced by this <see cref="BrushSide"/>.
		/// </summary>
		public Face Face {
			get {
				return Parent.Bsp.faces[FaceIndex];
			}
		}

		/// <summary>
		/// Gets or sets the index of the <see cref="LibBSP.Face"/> used by this <see cref="BrushSide"/>.
		/// </summary>
		public int FaceIndex {
			get {
				switch (MapType) {
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 0);
					}
					case MapType.Raven: {
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
					case MapType.Raven: {
						bytes.CopyTo(Data, 8);
						break;
					}
				}
			}
		}

		/// <summary>
		/// In Source engine, gets the <see cref="LibBSP.Displacement"/> referenced by this <see cref="BrushSide"/>.
		/// This is never used since the brushes used to create Displacements are optimized out.
		/// </summary>
		public Displacement Displacement {
			get {
				return Parent.Bsp.dispInfos[DisplacementIndex];
			}
		}

		/// <summary>
		/// In Source engine, gets or sets the index of the <see cref="LibBSP.Displacement"/> used by this <see cref="BrushSide"/>.
		/// This is never used since the brushes used to create Displacements are optimized out.
		/// </summary>
		public int DisplacementIndex {
			get {
				switch (MapType) {
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
					case MapType.DMoMaM: {
						return BitConverter.ToInt16(Data, 4);
					}
					case MapType.Vindictus: {
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
					case MapType.DMoMaM: {
						Data[4] = bytes[0];
						Data[5] = bytes[1];
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 8);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Is this <see cref="BrushSide"/> a bevel?
		/// </summary>
		public bool IsBevel {
			get {
				switch (MapType) {
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
					case MapType.DMoMaM: {
						return Data[6] > 0;
					}
					case MapType.Vindictus: {
						return Data[12] > 0;
					}
					default: {
						return false;
					}
				}
			}
			set {
				switch (MapType) {
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
					case MapType.DMoMaM: {
						Data[6] = (byte)(value ? 1 : 0);
						break;
					}
					case MapType.Vindictus: {
						Data[12] = (byte)(value ? 1 : 0);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Is this <see cref="BrushSide"/> thin?
		/// </summary>
		public bool IsThin {
			get {
				switch (MapType) {
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
					case MapType.DMoMaM: {
						return Data[7] > 0;
					}
					default: {
						return false;
					}
				}
			}
			set {
				switch (MapType) {
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
					case MapType.DMoMaM: {
						Data[7] = (byte)(value ? 1 : 0);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="BrushSide"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="parent">The <see cref="ILump"/> this <see cref="BrushSide"/> came from.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public BrushSide(byte[] data, ILump parent) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Parent = parent;
		}
		
		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Lump{BrushSide}"/>.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <returns>A <see cref="Lump{BrushSide}"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> parameter was <c>null</c>.</exception>
		public static Lump<BrushSide> LumpFactory(byte[] data, BSP bsp, LumpInfo lumpInfo) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			return new Lump<BrushSide>(data, GetStructLength(bsp.version, lumpInfo.version), bsp, lumpInfo);
		}

		/// <summary>
		/// Gets the length of this struct's data for the given <paramref name="mapType"/> and <paramref name="lumpVersion"/>.
		/// </summary>
		/// <param name="mapType">The <see cref="LibBSP.MapType"/> of the BSP.</param>
		/// <param name="lumpVersion">The version number for the lump.</param>
		/// <returns>The length, in <c>byte</c>s, of this struct.</returns>
		/// <exception cref="ArgumentException">This struct is not valid or is not implemented for the given <paramref name="mapType"/> and <paramref name="lumpVersion"/>.</exception>
		public static int GetStructLength(MapType type, int version = 0) {
			switch (type) {
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 4;
				}
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4:
				case MapType.SiN:
				case MapType.Nightfire:
				case MapType.Quake3:
				case MapType.STEF2:
				case MapType.STEF2Demo:
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
				case MapType.DMoMaM:
				case MapType.FAKK: {
					return 8;
				}
				case MapType.MOHAA:
				case MapType.Raven: {
					return 12;
				}
				case MapType.Vindictus: {
					return 16;
				}
				default: {
					throw new ArgumentException("Lump object " + MethodBase.GetCurrentMethod().DeclaringType.Name + " does not exist in map type " + type + " or has not been implemented.");
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
				case MapType.CoD: {
					return 3;
				}
				case MapType.CoD2:
				case MapType.CoD4: {
					return 5;
				}
				case MapType.Raven:
				case MapType.Quake3: {
					return 9;
				}
				case MapType.FAKK: {
					return 10;
				}
				case MapType.MOHAA: {
					return 11;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 12;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 15;
				}
				case MapType.Nightfire: {
					return 16;
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
					return 19;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
