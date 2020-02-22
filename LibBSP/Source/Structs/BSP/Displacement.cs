#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Reflection;

namespace LibBSP {
#if UNITY
	using Vector3 = UnityEngine.Vector3;
#elif GODOT
	using Vector3 = Godot.Vector3;
#else
	using Vector3 = System.Numerics.Vector3;
#endif

	/// <summary>
	/// Holds all data for a Displacement from Source engine.
	/// </summary>
	public struct Displacement : ILumpObject {

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
		/// Gets or sets the starting position of this <see cref="Displacement"/>.
		/// </summary>
		public Vector3 StartPosition {
			get {
				return new Vector3(BitConverter.ToSingle(Data, 0), BitConverter.ToSingle(Data, 4), BitConverter.ToSingle(Data, 8));
			}
			set {
				value.GetBytes().CopyTo(Data, 0);
			}
		}

		/// <summary>
		/// Enumerates the <see cref="DisplacementVertex"/>es referenced by this <see cref="Displacement"/>.
		/// </summary>
		public IEnumerable<DisplacementVertex> Vertices {
			get {
				int numVertices = NumVertices;
				for (int i = 0; i < numVertices; ++i) {
					yield return Parent.Bsp.dispVerts[FirstVertexIndex + i];
				}
			}
		}

		/// <summary>
		/// Gets or sets the index of the first <see cref="DisplacementVertex"/> used by this <see cref="Displacement"/>.
		/// </summary>
		public int FirstVertexIndex {
			get {
				return BitConverter.ToInt32(Data, 12);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(Data, 12);
			}
		}

		/// <summary>
		/// Enumerates the flags for the triangles in this <see cref="Displacement"/>.
		/// </summary>
		public IEnumerable<ushort> Triangles {
			get {
				for (int i = 0; i < NumTriangles; ++i) {
					yield return (ushort)Parent.Bsp.displacementTriangles[FirstTriangleIndex + i];
				}
			}
		}

		/// <summary>
		/// Gets or sets the index of the first Displacement Triangle used by this <see cref="Displacement"/>.
		/// </summary>
		public int FirstTriangleIndex {
			get {
				return BitConverter.ToInt32(Data, 16);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(Data, 16);
			}
		}

		/// <summary>
		/// Gets or sets the power of this <see cref="Displacement"/>.
		/// </summary>
		public int Power {
			get {
				return BitConverter.ToInt32(Data, 20);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(Data, 20);
			}
		}

		/// <summary>
		/// Gets the number of vertices this <see cref="Displacement"/> uses, based on <see cref="Power"/>.
		/// </summary>
		public int NumVertices {
			get {
				int numSideVerts = (int)Math.Pow(2, Power) + 1;
				return numSideVerts * numSideVerts;
			}
		}

		/// <summary>
		/// Gets the number of triangles this <see cref="Displacement"/> has, based on <see cref="Power"/>.
		/// </summary>
		public int NumTriangles {
			get {
				int side = Power * Power;
				return 2 * side * side;
			}
		}

		/// <summary>
		/// Gets or sets the minimum allowed tesselation for this <see cref="Displacement"/>.
		/// </summary>
		public int MinimumTesselation {
			get {
				return BitConverter.ToInt32(Data, 24);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(Data, 24);
			}
		}

		/// <summary>
		/// Gets or sets the lighting smoothing angle for this <see cref="Displacement"/>.
		/// </summary>
		public float SmoothingAngle {
			get {
				return BitConverter.ToSingle(Data, 28);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(Data, 28);
			}
		}

		/// <summary>
		/// Gets or sets the contents flags for this <see cref="Displacement"/>.
		/// </summary>
		public int Contents {
			get {
				return BitConverter.ToInt32(Data, 32);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(Data, 32);
			}
		}

		/// <summary>
		/// Gets the <see cref="LibBSP.Face"/> this <see cref="Displacement"/> was made from, for texturing and other information.
		/// </summary>
		public Face Face {
			get {
				return Parent.Bsp.faces[FaceIndex];
			}
		}

		/// <summary>
		/// Gets or sets the index of the <see cref="LibBSP.Face"/> this <see cref="Displacement"/> was made from, for texturing and other information.
		/// </summary>
		public ushort FaceIndex {
			get {
				return BitConverter.ToUInt16(Data, 36);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(Data, 36);
			}
		}

		/// <summary>
		/// Get or sets the index of the lightmap alpha for this <see cref="Displacement"/>.
		/// </summary>
		public int LightmapAlphaStart {
			get {
				return BitConverter.ToInt32(Data, 38);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(Data, 38);
			}
		}

		/// <summary>
		/// Gets or sets the index of the first lightmap sample position used by this <see cref="Displacement"/>.
		/// </summary>
		public int LightmapSamplePositionStart {
			get {
				return BitConverter.ToInt32(Data, 42);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(Data, 42);
			}
		}

		/// <summary>
		/// Gets or sets the allowed vertices for this <see cref="Displacement"/>.
		/// </summary>
		public uint[] AllowedVertices {
			get {
				uint[] allowedVertices = new uint[10];
				int offset = -1;
				switch (MapType) {
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
				}
				if (offset >= 0) {
					for (int i = 0; i < 10; ++i) {
						allowedVertices[i] = BitConverter.ToUInt32(Data, offset + (i * 4));
					}
				}
				return allowedVertices;
			}
			set {
				if (value.Length != 10) {
					throw new ArgumentException("AllowedVerts array must have 10 elements.");
				}
				int offset = -1;
				switch (MapType) {
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
				}
				if (offset >= 0) {
					for (int i = 0; i < value.Length; ++i) {
						BitConverter.GetBytes(value[i]).CopyTo(Data, offset + (i * 4));
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Displacement"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="parent">The <see cref="ILump"/> this <see cref="Displacement"/> came from.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Displacement(byte[] data, ILump parent = null) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Parent = parent;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Lump{Displacement}"/>.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <returns>A <see cref="Lump{Displacement}"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> parameter was <c>null</c>.</exception>
		public static Lump<Displacement> LumpFactory(byte[] data, BSP bsp, LumpInfo lumpInfo) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			return new Lump<Displacement>(data, GetStructLength(bsp.version, lumpInfo.version), bsp, lumpInfo);
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
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source27:
				case MapType.L4D2:
				case MapType.TacticalInterventionEncrypted:
				case MapType.DMoMaM: {
					return 176;
				}
				case MapType.Source22: {
					return 180;
				}
				case MapType.Source23: {
					return 184;
				}
				case MapType.Vindictus: {
					return 232;
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
