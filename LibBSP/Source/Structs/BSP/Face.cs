#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#if !UNITY_5_6_OR_NEWER
// UIVertex was introduced in Unity 4.5 but it only had color, position and one UV.
// From 4.6.0 until 5.5.6 it was missing two sets of UVs.
#define OLDUNITY
#endif
#endif

using System;
using System.Collections.Generic;
using System.Reflection;

namespace LibBSP {
#if UNITY
	using Vector2 = UnityEngine.Vector2;
	using Plane = UnityEngine.Plane;
#if !OLDUNITY
	using Vertex = UnityEngine.UIVertex;
#endif
#elif GODOT
	using Vector2 = Godot.Vector2;
	using Plane = Godot.Plane;
#else
	using Vector2 = System.Numerics.Vector2;
	using Plane = System.Numerics.Plane;
#endif

	/// <summary>
	/// Holds all the data for a face in a BSP map.
	/// </summary>
	/// <remarks>
	/// Faces is one of the more different lumps between versions. Some of these fields
	/// are only used by one format. However, there are some commonalities which make
	/// it worthwhile to unify these. All formats use a plane, a texture, vertices,
	/// and lightmaps in some way.
	/// </remarks>
	public struct Face : ILumpObject {

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
		/// Gets the Plane used by this <see cref="Face"/>.
		/// </summary>
		public Plane Plane {
			get {
				return Parent.Bsp.planes[PlaneIndex];
			}
		}

		/// <summary>
		/// Gets or sets the index of the Plane used by this <see cref="Face"/>.
		/// </summary>
		public int PlaneIndex {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
					case MapType.Source17: {
						return BitConverter.ToUInt16(Data, 32);
					}
					case MapType.Nightfire:
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 0);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
					case MapType.Source17: {
						Data[32] = bytes[0];
						Data[33] = bytes[1];
						break;
					}
					case MapType.Nightfire:
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 0);
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// Does the <see cref="Face"/>'s normal point in the same direction as the Plane's?
		/// </summary>
		public bool PlaneSide {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF: {
						return BitConverter.ToUInt16(Data, 2) > 0;
					}
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
						return Data[2] > 0;
					}
					case MapType.Vindictus: {
						return Data[4] > 0;
					}
					case MapType.Source17: {
						return Data[34] > 0;
					}
					default: {
						return true;
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
						Data[2] = (byte)(value ? 1 : 0);
						break;
					}
					case MapType.Vindictus: {
						Data[4] = (byte)(value ? 1 : 0);
						break;
					}
					case MapType.Source17: {
						Data[34] = (byte)(value ? 1 : 0);
						break;
					}
				}
			}
		}

		/// <summary>
		/// For formats which use <see cref="Edge"/>s, enumerates the <see cref="Edge"/> indices referenced
		/// by this <see cref="Face"/>. A negative index means the <see cref="Edge"/> is used backward from
		/// its second <see cref="Vertex"/> to its first.
		/// </summary>
		public IEnumerable<int> EdgeIndices {
			get {
				for (int i = 0; i < NumEdgeIndices; ++i) {
					yield return (int)Parent.Bsp.surfEdges[FirstEdgeIndexIndex + i];
				}
			}
		}

		/// <summary>
		/// For formats which use <see cref="Edge"/>s, gets or sets the index of the first <see cref="Edge"/>
		/// index in this <see cref="Face"/>. A negative index means the <see cref="Edge"/> is used backward
		/// from its second <see cref="Vertex"/> to first.
		/// </summary>
		[Index("edges")] public int FirstEdgeIndexIndex {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
						return BitConverter.ToInt32(Data, 4);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 8);
					}
					case MapType.Source17: {
						return BitConverter.ToInt32(Data, 36);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
						bytes.CopyTo(Data, 4);
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 8);
						break;
					}
					case MapType.Source17: {
						bytes.CopyTo(Data, 36);
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// For formats which use <see cref="Edge"/>s, gets or sets the count of <see cref="Edge"/> indices
		/// in this <see cref="Face"/>.
		/// </summary>
		[Count("edges")] public int NumEdgeIndices {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
						return BitConverter.ToUInt16(Data, 8);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 12);
					}
					case MapType.Source17: {
						return BitConverter.ToUInt16(Data, 40);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
						Data[8] = bytes[0];
						Data[9] = bytes[1];
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 12);
						break;
					}
					case MapType.Source17: {
						Data[40] = bytes[0];
						Data[41] = bytes[1];
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="LibBSP.Texture"/> referenced by this <see cref="Face"/>.
		/// </summary>
		public Texture Texture {
			get {
				return Parent.Bsp.textures[TextureIndex];
			}
		}
		
		/// <summary>
		/// Gets or sets the index of the <see cref="LibBSP.Texture"/> used by this <see cref="Face"/>.
		/// </summary>
		public int TextureIndex {
			get {
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						return BitConverter.ToInt16(Data, 0);
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return BitConverter.ToInt32(Data, 0);
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF: {
						return BitConverter.ToUInt16(Data, 10);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 24);
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
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						bytes.CopyTo(Data, 0);
						break;
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF: {
						Data[10] = bytes[0];
						Data[11] = bytes[1];
						break;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 24);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Enumerates the <see cref="Vertex"/> objects referenced by this <see cref="Face"/>.
		/// </summary>
		public IEnumerable<Vertex> Vertices {
			get {
				for (int i = 0; i < NumVertices; ++i) {
					yield return Parent.Bsp.vertices[FirstVertexIndex + i];
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the index of the first <see cref="Vertex"/> used by this <see cref="Face"/>.
		/// </summary>
		[Index("vertices")] public int FirstVertexIndex {
			get {
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 4);
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.CoD4: {
						return BitConverter.ToInt32(Data, 12);
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
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 4);
						break;
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.CoD4: {
						bytes.CopyTo(Data, 12);
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the number of <see cref="Vertex"/> objects used by this <see cref="Face"/>.
		/// </summary>
		[Count("vertices")] public int NumVertices {
			get {
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt16(Data, 8);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 8);
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return BitConverter.ToInt32(Data, 16);
					}
					case MapType.CoD4: {
						return BitConverter.ToInt16(Data, 16);
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
					case MapType.CoD2: {
						Data[8] = bytes[0];
						Data[9] = bytes[1];
						break;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 8);
						break;
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						bytes.CopyTo(Data, 16);
						break;
					}
					case MapType.CoD4: {
						Data[16] = bytes[0];
						Data[17] = bytes[1];
						break;
					}
				}
			}
		}

		/// <summary>
		/// For Nightfire, gets the material referenced by this <see cref="Face"/>.
		/// </summary>
		public Texture Material {
			get {
				return Parent.Bsp.materials[TextureIndex];
			}
		}
		
		/// <summary>
		/// For Nightfire, gets or sets the index of the material referenced by this <see cref="Face"/>.
		/// </summary>
		public int MaterialIndex {
			get {
				switch (MapType) {
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 28);
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
						bytes.CopyTo(Data, 28);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="LibBSP.TextureInfo"/> returned by this <see cref="Face"/>.
		/// </summary>
		public TextureInfo TextureInfo {
			get {
				return Parent.Bsp.texInfo[TextureInfoIndex];
			}
		}
		
		/// <summary>
		/// Gets or sets the index of the <see cref="LibBSP.TextureInfo"/> used by this <see cref="Face"/>.
		/// </summary>
		public int TextureInfoIndex {
			get {
				switch (MapType) {
					case MapType.Quake:
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
						return BitConverter.ToUInt16(Data, 10);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 16);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 32);
					}
					case MapType.Source17: {
						return BitConverter.ToUInt16(Data, 42);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake:
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
						Data[10] = bytes[0];
						Data[11] = bytes[1];
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 16);
						break;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 32);
						break;
					}
					case MapType.Source17: {
						Data[42] = bytes[0];
						Data[43] = bytes[1];
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the index of the <see cref="LibBSP.Displacement"/> using this <see cref="Face"/>.
		/// </summary>
		public int DisplacementIndex {
			get {
				switch (MapType) {
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
						return BitConverter.ToInt16(Data, 12);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 20);
					}
					case MapType.Source17: {
						return BitConverter.ToInt16(Data, 44);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
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
						Data[12] = bytes[0];
						Data[13] = bytes[1];
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 20);
						break;
					}
					case MapType.Source17: {
						Data[44] = bytes[0];
						Data[45] = bytes[1];
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets the original <see cref="Face"/> which was split to create this <see cref="Face"/>.
		/// </summary>
		public Face OriginalFace {
			get {
				return Parent.Bsp.originalFaces[OriginalFaceIndex];
			}
		}

		/// <summary>
		/// Gets or sets the index of the original <see cref="Face"/> which was split to create this <see cref="Face"/>.
		/// </summary>
		public int OriginalFaceIndex {
			get {
				switch (MapType) {
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
						return BitConverter.ToInt32(Data, 44);
					}
					case MapType.Vindictus: {
						if (LumpVersion == 2) {
							return BitConverter.ToInt32(Data, 60);
						}
						else {
							return BitConverter.ToInt32(Data, 56);
						}
					}
					case MapType.Source17: {
						return BitConverter.ToInt32(Data, 96);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
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
						bytes.CopyTo(Data, 44);
						break;
					}
					case MapType.Vindictus: {
						if (LumpVersion == 2) {
							bytes.CopyTo(Data, 60);
						}
						else {
							bytes.CopyTo(Data, 56);
						}
						break;
					}
					case MapType.Source17: {
						bytes.CopyTo(Data, 96);
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the type (flags) of this face.
		/// </summary>
		public int Type {
			get {
				switch (MapType) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return BitConverter.ToInt32(Data, 8);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 20);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						bytes.CopyTo(Data, 8);
						break;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 20);
						break;
					}
				}
			}
		}

		/// <summary>
		/// For formats which use triangle corner indices, enumerates the indices of <see cref="Vertex"/>
		/// objects used by this <see cref="Face"/>.
		/// </summary>
		public IEnumerable<int> Indices {
			get {
				for (int i = 0; i < NumIndices; ++i) {
					yield return (int)Parent.Bsp.indices[FirstIndexIndex + i];
				}
			}
		}
		
		/// <summary>
		/// For formats which use triangle corner indices, gets or sets the index first <see cref="Vertex"/> index
		/// used by this <see cref="Face"/>.
		/// </summary>
		[Index("indices")] public int FirstIndexIndex {
			get {
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 12);
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.CoD4: {
						return BitConverter.ToInt32(Data, 20);
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
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 12);
						break;
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.CoD4: {
						bytes.CopyTo(Data, 20);
						break;
					}
				}
			}
		}

		/// <summary>
		/// For formats which use triangle corner indices, gets or sets the count of <see cref="Vertex"/> indices
		/// used by this <see cref="Face"/>.
		/// </summary>
		[Count("indices")] public int NumIndices {
			get {
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt16(Data, 10);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 16);
					}
					case MapType.CoD4: {
						return BitConverter.ToInt16(Data, 18);
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return BitConverter.ToInt32(Data, 24);
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
					case MapType.CoD2: {
						Data[10] = bytes[0];
						Data[11] = bytes[1];
						return;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 16);
						break;
					}
					case MapType.CoD4: {
						Data[18] = bytes[0];
						Data[19] = bytes[1];
						return;
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						bytes.CopyTo(Data, 24);
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the flags for lightmap style used by this <see cref="Face"/>.
		/// </summary>
		public int LightStyle {
			get {
				switch (MapType) {
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 40);
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
						bytes.CopyTo(Data, 40);
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the offset into the lightmap data this <see cref="Face"/> uses.
		/// </summary>
		public int LightMapOffset {
			get {
				switch (MapType) {
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 44);
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
						bytes.CopyTo(Data, 44);
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// Gets the size of the <see cref="Patch"/> used by this <see cref="Face"/>.
		/// </summary>
		public Vector2 PatchSize {
			get {
				switch (MapType) {
					case MapType.Quake3:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return new Vector2(BitConverter.ToInt32(Data, 96), BitConverter.ToInt32(Data, 100));
					}
					case MapType.Raven: {
						return new Vector2(BitConverter.ToInt32(Data, 140), BitConverter.ToInt32(Data, 144));
					}
					default: {
						return new Vector2(float.NaN, float.NaN);
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.Quake3:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						byte[] bytes = BitConverter.GetBytes((int)value.X());
						bytes.CopyTo(Data, 96);
						bytes = BitConverter.GetBytes((int)value.Y());
						bytes.CopyTo(Data, 100);
						break;
					}
					case MapType.Raven: {
						byte[] bytes = BitConverter.GetBytes((int)value.X());
						bytes.CopyTo(Data, 140);
						bytes = BitConverter.GetBytes((int)value.Y());
						bytes.CopyTo(Data, 144);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Face"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="parent">The <see cref="ILump"/> this <see cref="Face"/> came from.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Face(byte[] data, ILump parent = null) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Parent = parent;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Lump{Face}"/>.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <returns>A <see cref="Lump{Face}"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> parameter was <c>null</c>.</exception>
		public static Lump<Face> LumpFactory(byte[] data, BSP bsp, LumpInfo lumpInfo) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			return new Lump<Face>(data, GetStructLength(bsp.version, lumpInfo.version), bsp, lumpInfo);
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
				case MapType.CoD:
				case MapType.CoD2: {
					return 16;
				}
				case MapType.Quake:
				case MapType.Quake2:
				case MapType.Daikatana: {
					return 20;
				}
				case MapType.CoD4: {
					return 24;
				}
				case MapType.SiN: {
					return 36;
				}
				case MapType.SoF: {
					return 40;
				}
				case MapType.Nightfire: {
					return 48;
				}
				case MapType.Source17: {
					return 104;
				}
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
					return 56;
				}
				case MapType.Vindictus: {
					if (lumpVersion == 2) {
						return 76;
					} else {
						return 72;
					}
				}
				case MapType.Quake3: {
					return 104;
				}
				case MapType.FAKK:
				case MapType.MOHAA: {
					return 108;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 132;
				}
				case MapType.Raven: {
					return 148;
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
				case MapType.FAKK:
				case MapType.MOHAA: {
					return 3;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 5;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF:
				case MapType.CoD: {
					return 6;
				}
				case MapType.CoD2:
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
				case MapType.Quake: {
					return 7;
				}
				case MapType.Nightfire:
				case MapType.CoD4: {
					return 9;
				}
				case MapType.Raven:
				case MapType.Quake3: {
					return 13;
				}
				default: {
					return -1;
				}
			}
		}

		/// <summary>
		/// Gets the index for the original faces lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForOriginalFacesLump(MapType type) {
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
					return 27;
				}
				case MapType.CoD4: {
					// Not sure where else to put this. This is the simple surfaces lump?
					return 47;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
