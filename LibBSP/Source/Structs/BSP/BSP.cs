#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#if !UNITY_5_6_OR_NEWER
#define OLDUNITY
#endif
#endif

using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace LibBSP {
#if UNITY
	using Plane = UnityEngine.Plane;
#if !OLDUNITY
	using Vertex = UnityEngine.UIVertex;
#endif
#elif GODOT
	using Plane = Godot.Plane;
#else
	using Plane = System.Numerics.Plane;
#endif

	/// <summary>
	/// Enum of the known different map formats.
	/// </summary>
	public enum MapType : int {
		/// <summary>
		/// Unknown or unsupported map type
		/// </summary>
		Undefined = 0x00000000,

		/// <summary>
		/// Quake or Quake Engine flags, including <see cref="GoldSrc"/> and <see cref="BlueShift"/>.
		/// </summary>
		Quake = 0x01000000,
		/// <summary>
		/// GoldSrc engine, especially Half-Life. As flags includes <see cref="BlueShift"/>.
		/// </summary>
		GoldSrc = 0x01010000,
		/// <summary>
		/// Half-Life Blue Shift
		/// </summary>
		BlueShift = 0x01010001,

		/// <summary>
		/// Quake 2 or Quake 2 Engine flags, including <see cref="Daikatana"/> <see cref="SoF"/>
		/// and <see cref="SiN"/>.
		/// </summary>
		Quake2 = 0x02000000,
		/// <summary>
		/// Daikatana
		/// </summary>
		Daikatana = 0x02000001,
		/// <summary>
		/// Soldier of Fortune
		/// </summary>
		SoF = 0x02000002,
		/// <summary>
		/// SiN
		/// </summary>
		SiN = 0x02000004,

		/// <summary>
		/// Quake 3 or Quake 3 Engine flags, including <see cref="ET"/> <see cref="Raven"/> <see cref="STEF2"/>
		/// <see cref="STEF2Demo"/> <see cref="MOHAA"/> <see cref="MOHAABT"/> <see cref="FAKK2"/> <see cref="Alice"/>
		/// <see cref="CoD"/> <see cref="CoD2"/> and <see cref="CoD4"/>.
		/// </summary>
		Quake3 = 0x04000000,
		/// <summary>
		/// Wolfenstein: Enemy Territory or Return to Castle Wolfenstein
		/// </summary>
		ET = 0x04000001,
		/// <summary>
		/// Raven Software (Jedi Outcast, Jedi Academy, Soldier of Fortune 2)
		/// </summary>
		Raven = 0x04010000,
		/// <summary>
		/// Call of Duty or flags including <see cref="CoDDemo"/> <see cref="CoD2"/> and <see cref="CoD4"/>.
		/// </summary>
		CoD = 0x04020000,
		/// <summary>
		/// Call of Duty demo
		/// </summary>
		CoDDemo = 0x04020001,
		/// <summary>
		/// Call of Duty 2
		/// </summary>
		CoD2 = 0x04020002,
		/// <summary>
		/// Call of Duty 4
		/// </summary>
		CoD4 = 0x04020004,
		/// <summary>
		/// Quake 3 "Ubertools" including <see cref="STEF2"/> <see cref="STEF2Demo"/> <see cref="MOHAA"/>
		/// <see cref="MOHAADemo"/> <see cref="MOHAABT"/> <see cref="FAKK2"/> and <see cref="Alice"/>.
		/// </summary>
		UberTools = 0x04040000,
		/// <summary>
		/// Star Trek Elite Force 2. As flags includes <see cref="STEF2Demo"/>.
		/// </summary>
		STEF2 = 0x04040100,
		/// <summary>
		/// Star Trek Elite Force 2 Demo version
		/// </summary>
		STEF2Demo = 0x04040101,
		/// <summary>
		/// Medal of Honor Allied Assault. As flags includes <see cref="MOHAABT"/> and <see cref="MOHAADemo"/>.
		/// </summary>
		MOHAA = 0x04040200,
		/// <summary>
		/// Medal of Honor Allied Assault free demo.
		/// </summary>
		MOHAADemo = 0x04040201,
		/// <summary>
		/// Medal of Honor Allied Assault Spearhead and BreakThrough expansion packs.
		/// </summary>
		MOHAABT = 0x04040202,
		/// <summary>
		/// Heavy Metal FAKK2. As flags includes <see cref="Alice"/>.
		/// </summary>
		FAKK2 = 0x04040400,
		/// <summary>
		/// American McGee's Alice
		/// </summary>
		Alice = 0x04040401,

		/// <summary>
		/// 007 Nightfire
		/// </summary>
		Nightfire = 0x08000000,

		/// <summary>
		/// Source Engine, including <see cref="Source17" /> <see cref="Source18"/> <see cref="Source19"/>
		/// <see cref="Source20"/> <see cref="DMoMaM"/> <see cref="Vindictus"/> <see cref="Source21"/>
		/// <see cref="L4D2"/> <see cref="TacticalInterventionEncrypted"/> <see cref="Source22"/>
		/// <see cref="Source23"/> and <see cref="Source27"/>.
		/// </summary>
		Source = 0x10000000,
		/// <summary>
		/// Source Engine v17. Vampire the Masquerade: Bloodlines
		/// </summary>
		Source17 = 0x10000100,
		/// <summary>
		/// Source Engine v18. Half-Life 2 Beta
		/// </summary>
		Source18 = 0x10000200,
		/// <summary>
		/// Source Engine v19. Half-Life 2
		/// </summary>
		Source19 = 0x10000400,
		/// <summary>
		/// Source Engine v20. As flags includes <see cref="DMoMaM"/> and <see cref="Vindictus"/>.
		/// </summary>
		Source20 = 0x10000800,
		/// <summary>
		/// Dark Messiah of Might &amp; Magic
		/// </summary>
		DMoMaM = 0x10000801,
		/// <summary>
		/// Vindictus
		/// </summary>
		Vindictus = 0x10000802,
		/// <summary>
		/// Source Engine v21. As flags includes <see cref="L4D2"/> and <see cref="TacticalInterventionEncrypted"/>.
		/// </summary>
		Source21 = 0x10001000,
		/// <summary>
		/// Left 4 Dead 2
		/// </summary>
		L4D2 = 0x10001001,
		/// <summary>
		/// Tactical Intervention, original encrypted release. Steam version is <see cref="Source22"/>.
		/// </summary>
		TacticalInterventionEncrypted = 0x10001002,
		/// <summary>
		/// Source Engine v22, Tactical Intervention
		/// </summary>
		Source22 = 0x10002000,
		/// <summary>
		/// Source Engine v23. DotA 2
		/// </summary>
		Source23 = 0x10004000,
		/// <summary>
		/// Source Engine v27. Contagion
		/// </summary>
		Source27 = 0x10008000,

		/// <summary>
		/// rBSP v29. Titanfall
		/// </summary>
		Titanfall = 0x20000000,

	}

	/// <summary>
	/// Struct containing basic information for a lump in a BSP file.
	/// </summary>
	public struct LumpInfo {
		/// <summary>
		/// ID of this lump.
		/// </summary>
		public int ident;
		/// <summary>
		/// Lump flags.
		/// </summary>
		public int flags;
		/// <summary>
		/// Lump version.
		/// </summary>
		public int version;
		/// <summary>
		/// Lump offset.
		/// </summary>
		public int offset;
		/// <summary>
		/// Lump length.
		/// </summary>
		public int length;
		/// <summary>
		/// Lump file.
		/// </summary>
		public FileInfo lumpFile;
	}

	/// <summary>
	/// Holds data for any and all supported BSP formats. Any unused lumps in a given format
	/// will be left as null.
	/// </summary>
	public class BSP : Dictionary<int, LumpInfo> {

		private MapType _mapType;

		// Map structures
		// Quake 1/GoldSrc
		private Entities _entities;
		private Lump<Plane> _planes;
		private Textures _textures;
		private Lump<Vertex> _vertices;
		private Lump<Node> _nodes;
		private Lump<TextureInfo> _textureInfo;
		private Lump<Face> _faces;
		private Lump<Leaf> _leaves;
		private NumList _leafFaces;
		private Lump<Edge> _edges;
		private NumList _faceEdges;
		private Lump<Model> _models;
		private Visibility _visibility;
		private Lightmaps _lightmaps;
		// Quake 2
		private Lump<Brush> _brushes;
		private Lump<BrushSide> _brushSides;
		private NumList _leafBrushes;
		// MoHAA
		private Lump<LODTerrain> _lodTerrains;
		private Lump<StaticModel> _staticModels;
		private NumList _leafStaticModels;
		// CoD
		private Lump<Patch> _patches;
		private Lump<Vertex> _patchVertices;
		private NumList _patchIndices;
		private NumList _leafPatches;
		// Nightfire
		private Textures _materials;
		private NumList _indices;
		private Lump<Vertex> _normals;
		// Source
		private Lump<Face> _originalFaces;
		private NumList _textureTable;
		private Lump<TextureData> _textureData;
		private Lump<Displacement> _displacements;
		private Lump<DisplacementVertex> _displacementVertices;
		private NumList _displacementTriangles;
		// public SourceOverlays overlays;
		private Lump<Cubemap> _cubemaps;
		private GameLump _gameLump;

		/// <summary>
		/// The <see cref="BSPReader"/> object in use by this <see cref="BSP"/> class.
		/// </summary>
		public BSPReader Reader { get; private set; }

		/// <summary>
		/// The version of this BSP. Do not change this unless you want to force reading a BSP as a certain format.
		/// </summary>
		public MapType MapType {
			get {
				if (_mapType == MapType.Undefined) {
					_mapType = Reader.GetVersion();
				}

				return _mapType;
			}
			set {
				_mapType = value;
			}
		}

		/// <summary>
		/// Is the BSP file in big endian format?
		/// </summary>
		public bool BigEndian { get; private set; }

		/// <summary>
		/// The <see cref="LibBSP.Entities"/> object in the BSP file, if available.
		/// </summary>
		public Entities Entities {
			get {
				if (_entities == null) {
					int index = Entity.GetIndexForLump(MapType);
					if (index >= 0) {
						_entities = Entity.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _entities;
			}
		}

		/// <summary>
		/// Has the <see cref="LibBSP.Entities"/> lump been loaded yet?
		/// </summary>
		public bool EntitiesLoaded {
			get {
				return _entities != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{Plane}"/> of <see cref="Plane"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<Plane> Planes {
			get {
				if (_planes == null) {
					int index = PlaneExtensions.GetIndexForLump(MapType);
					if (index >= 0) {
						_planes = PlaneExtensions.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _planes;
			}
		}

		/// <summary>
		/// Has the <see cref="Plane"/> lump been loaded yet?
		/// </summary>
		public bool PlanesLoaded {
			get {
				return _planes != null;
			}
		}

		/// <summary>
		/// The <see cref="LibBSP.Textures"/> object in the BSP file, if available.
		/// </summary>
		public Textures Textures {
			get {
				if (_textures == null) {
					int index = Texture.GetIndexForLump(MapType);
					if (index >= 0) {
						_textures = Texture.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _textures;
			}
		}

		/// <summary>
		/// Has the <see cref="Textures"/> lump been loaded yet?
		/// </summary>
		public bool TexturesLoaded {
			get {
				return _textures != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{Vertex}"/> of <see cref="Vertex"/> objects in the BSP file representing the vertices of the BSP, if available.
		/// </summary>
		public Lump<Vertex> Vertices {
			get {
				if (_vertices == null) {
					int index = VertexExtensions.GetIndexForLump(MapType);
					if (index >= 0) {
						_vertices = VertexExtensions.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _vertices;
			}
		}

		/// <summary>
		/// Has the <see cref="Vertex"/> lump been loaded yet?
		/// </summary>
		public bool VerticesLoaded {
			get {
				return _vertices != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{Vertex}"/> of <see cref="Vertex"/> objects in the BSP file representing the vertex normals of the BSP, if available.
		/// </summary>
		public Lump<Vertex> Normals {
			get {
				if (_normals == null) {
					int index = VertexExtensions.GetIndexForNormalsLump(MapType);
					if (index >= 0) {
						_normals = VertexExtensions.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _normals;
			}
		}

		/// <summary>
		/// Has the normals lump been loaded yet?
		/// </summary>
		public bool NormalsLoaded {
			get {
				return _normals != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.Node}"/> of <see cref="Node"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<Node> Nodes {
			get {
				if (_nodes == null) {
					int index = Node.GetIndexForLump(MapType);
					if (index >= 0) {
						_nodes = Node.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _nodes;
			}
		}

		/// <summary>
		/// Has the <see cref="Node"/> lump been loaded yet?
		/// </summary>
		public bool NodesLoaded {
			get {
				return _nodes != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.TextureInfo}"/> of <see cref="LibBSP.TextureInfo"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<TextureInfo> TextureInfo {
			get {
				if (_textureInfo == null) {
					int index = LibBSP.TextureInfo.GetIndexForLump(MapType);
					if (index >= 0) {
						_textureInfo = LibBSP.TextureInfo.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _textureInfo;
			}
		}

		/// <summary>
		/// Has the <see cref="TextureInfo"/> lump been loaded yet?
		/// </summary>
		public bool TextureInfoLoaded {
			get {
				return _textureInfo != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.Face}"/> of <see cref="Face"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<Face> Faces {
			get {
				if (_faces == null) {
					int index = Face.GetIndexForLump(MapType);
					if (index >= 0) {
						_faces = Face.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _faces;
			}
		}

		/// <summary>
		/// Has the <see cref="Face"/> lump been loaded yet?
		/// </summary>
		public bool FacesLoaded {
			get {
				return _faces != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.Leaf}"/> of <see cref="Leaf"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<Leaf> Leaves {
			get {
				if (_leaves == null) {
					int index = Leaf.GetIndexForLump(MapType);
					if (index >= 0) {
						_leaves = Leaf.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _leaves;
			}
		}

		/// <summary>
		/// Has the <see cref="Leaf"/> lump been loaded yet?
		/// </summary>
		public bool LeavesLoaded {
			get {
				return _leaves != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.Edge}"/> of <see cref="Edge"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<Edge> Edges {
			get {
				if (_edges == null) {
					int index = Edge.GetIndexForLump(MapType);
					if (index >= 0) {
						_edges = Edge.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _edges;
			}
		}

		/// <summary>
		/// Has the <see cref="Edge"/> lump been loaded yet?
		/// </summary>
		public bool EdgesLoaded {
			get {
				return _edges != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.Model}"/> of <see cref="Model"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<Model> Models {
			get {
				if (_models == null) {
					int index = Model.GetIndexForLump(MapType);
					if (index >= 0) {
						_models = Model.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _models;
			}
		}

		/// <summary>
		/// Has the <see cref="Model"/> lump been loaded yet?
		/// </summary>
		public bool ModelsLoaded {
			get {
				return _models != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.Brush}"/> of <see cref="Brush"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<Brush> Brushes {
			get {
				if (_brushes == null) {
					int index = Brush.GetIndexForLump(MapType);
					if (index >= 0) {
						_brushes = Brush.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _brushes;
			}
		}

		/// <summary>
		/// Has the <see cref="Brush"/> lump been loaded yet?
		/// </summary>
		public bool BrushesLoaded {
			get {
				return _brushes != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.BrushSide}"/> of <see cref="BrushSide"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<BrushSide> BrushSides {
			get {
				if (_brushSides == null) {
					int index = BrushSide.GetIndexForLump(MapType);
					if (index >= 0) {
						_brushSides = BrushSide.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _brushSides;
			}
		}

		/// <summary>
		/// Has the <see cref="BrushSide"/> lump been loaded yet?
		/// </summary>
		public bool BrushSidesLoaded {
			get {
				return _brushSides != null;
			}
		}

		/// <summary>
		/// A <see cref="LibBSP.Textures"/> object representing Materials (shaders), if available.
		/// </summary>
		public Textures Materials {
			get {
				if (_materials == null) {
					int index = Texture.GetIndexForMaterialLump(MapType);
					if (index >= 0) {
						_materials = Texture.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _materials;
			}
		}

		/// <summary>
		/// Has the Materials lump been loaded yet?
		/// </summary>
		public bool MaterialsLoaded {
			get {
				return _materials != null;
			}
		}

		/// <summary>
		/// A <see cref="LibBSP.Visibility"/> object holding leaf visiblity data for this <see cref="BSP"/>.
		/// </summary>
		public Visibility Visibility {
			get {
				if (_visibility == null) {
					int index = Visibility.GetIndexForLump(MapType);
					if (index >= 0) {
						_visibility = new Visibility(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _visibility;
			}
		}

		/// <summary>
		/// Has the <see cref="LibBSP.Visibility"/> lump been loaded yet?
		/// </summary>
		public bool VisibilityLoaded {
			get {
				return _visibility != null;
			}
		}

		/// <summary>
		/// A <see cref="LibBSP.Lightmaps"/> object holding lightmap data for this <see cref="BSP"/>.
		/// </summary>
		public Lightmaps Lightmaps {
			get {
				if (_lightmaps == null) {
					int index = Lightmaps.GetIndexForLump(MapType);
					if (index >= 0) {
						_lightmaps = new Lightmaps(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _lightmaps;
			}
		}

		/// <summary>
		/// Has the <see cref="LibBSP.Lightmaps"/> lump been loaded yet?
		/// </summary>
		public bool LightmapsLoaded {
			get {
				return _lightmaps != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.Face}"/> of <see cref="Face"/> objects in the BSP file representing the Original Faces, if available.
		/// </summary>
		public Lump<Face> OriginalFaces {
			get {
				if (_originalFaces == null) {
					int index = Face.GetIndexForOriginalFacesLump(MapType);
					if (index >= 0) {
						_originalFaces = Face.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _originalFaces;
			}
		}

		/// <summary>
		/// Has the Original <see cref="Face"/> lump been loaded yet?
		/// </summary>
		public bool OriginalFacesLoaded {
			get {
				return _originalFaces != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.TextureData}"/> of <see cref="LibBSP.TextureData"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<TextureData> TextureData {
			get {
				if (_textureData == null) {
					int index = LibBSP.TextureData.GetIndexForLump(MapType);
					if (index >= 0) {
						_textureData = LibBSP.TextureData.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _textureData;
			}
		}

		/// <summary>
		/// Has the <see cref="LibBSP.TextureData"/> lump been loaded yet?
		/// </summary>
		public bool TextureDataLoaded {
			get {
				return _textureData != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.Displacement}"/> of <see cref="Displacement"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<Displacement> Displacements {
			get {
				if (_displacements == null) {
					int index = Displacement.GetIndexForLump(MapType);
					if (index >= 0) {
						_displacements = Displacement.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _displacements;
			}
		}

		/// <summary>
		/// Has the <see cref="Displacement"/> lump been loaded yet?
		/// </summary>
		public bool DisplacementsLoaded {
			get {
				return _displacements != null;
			}
		}

		/// <summary>
		/// The <see cref="Lump{LibBSP.DisplacementVertex}"/> object in the BSP file, if available.
		/// </summary>
		public Lump<DisplacementVertex> DisplacementVertices {
			get {
				if (_displacementVertices == null) {
					int index = DisplacementVertex.GetIndexForLump(MapType);
					if (index >= 0) {
						_displacementVertices = DisplacementVertex.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _displacementVertices;
			}
		}

		/// <summary>
		/// Has the <see cref="DisplacementVertex"/> lump been loaded yet?
		/// </summary>
		public bool DisplacementVerticesLoaded {
			get {
				return _displacementVertices != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.Cubemap}"/> of <see cref="Cubemap"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<Cubemap> Cubemaps {
			get {
				if (_cubemaps == null) {
					int index = Cubemap.GetIndexForLump(MapType);
					if (index >= 0) {
						_cubemaps = Cubemap.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _cubemaps;
			}
		}

		/// <summary>
		/// Has the <see cref="Cubemap"/> lump been loaded yet?
		/// </summary>
		public bool CubemapsLoaded {
			get {
				return _cubemaps != null;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Leaf Faces lump, if available.
		/// </summary>
		public NumList LeafFaces {
			get {
				if (_leafFaces == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForLeafFacesLump(MapType, out type);
					if (index >= 0) {
						_leafFaces = NumList.LumpFactory(Reader.ReadLump(this[index]), type, this, this[index]);
					}
				}

				return _leafFaces;
			}
		}

		/// <summary>
		/// Has the Leaf Faces lump been loaded yet?
		/// </summary>
		public bool LeafFacesLoaded {
			get {
				return _leafFaces != null;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Face Edges lump, if available.
		/// </summary>
		public NumList FaceEdges {
			get {
				if (_faceEdges == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForFaceEdgesLump(MapType, out type);
					if (index >= 0) {
						_faceEdges = NumList.LumpFactory(Reader.ReadLump(this[index]), type, this, this[index]);
					}
				}

				return _faceEdges;
			}
		}

		/// <summary>
		/// Has the Surface Edges lump been loaded yet?
		/// </summary>
		public bool FaceEdgesLoaded {
			get {
				return _faceEdges != null;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Leaf Brushes lump, if available.
		/// </summary>
		public NumList LeafBrushes {
			get {
				if (_leafBrushes == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForLeafBrushesLump(MapType, out type);
					if (index >= 0) {
						_leafBrushes = NumList.LumpFactory(Reader.ReadLump(this[index]), type, this, this[index]);
					}
				}

				return _leafBrushes;
			}
		}

		/// <summary>
		/// Has the Leaf Brushes lump been loaded yet?
		/// </summary>
		public bool LeafBrushesLoaded {
			get {
				return _leafBrushes != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.StaticModel}"/> of <see cref="StaticModel"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<StaticModel> StaticModels {
			get {
				if (_staticModels == null) {
					int index = StaticModel.GetIndexForLump(MapType);
					if (index >= 0) {
						_staticModels = StaticModel.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _staticModels;
			}
		}

		/// <summary>
		/// Has the <see cref="StaticModel"/> lump been loaded yet?
		/// </summary>
		public bool StaticModelsLoaded {
			get {
				return _staticModels != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.LODTerrain}"/> of <see cref="LODTerrain"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<LODTerrain> LODTerrains {
			get {
				if (_lodTerrains == null) {
					int index = LODTerrain.GetIndexForLump(MapType);
					if (index >= 0) {
						_lodTerrains = LODTerrain.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _lodTerrains;
			}
		}

		/// <summary>
		/// Has the <see cref="LODTerrain"/> lump been loaded yet?
		/// </summary>
		public bool LODTerrainsLoaded {
			get {
				return _lodTerrains != null;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Leaf Static Models lump, if available.
		/// These are a list of MOHAA static model indices, referenced by leaves.
		/// </summary>
		public NumList LeafStaticModels {
			get {
				if (_leafStaticModels == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForLeafStaticModelsLump(MapType, out type);
					if (index >= 0) {
						_leafStaticModels = NumList.LumpFactory(Reader.ReadLump(this[index]), type, this, this[index]);
					}
				}

				return _leafStaticModels;
			}
		}

		/// <summary>
		/// Has the Leaf Static Models lump been loaded yet?
		/// </summary>
		public bool LeafStaticModelsLoaded {
			get {
				return _leafStaticModels != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{LibBSP.Patch}"/> of <see cref="Patch"/> objects in the BSP file, if available.
		/// </summary>
		public Lump<Patch> Patches {
			get {
				if (_patches == null) {
					int index = Patch.GetIndexForLump(MapType);
					if (index >= 0) {
						_patches = Patch.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _patches;
			}
		}

		/// <summary>
		/// Has the <see cref="Patch"/> lump been loaded yet?
		/// </summary>
		public bool PatchesLoaded {
			get {
				return _patches != null;
			}
		}

		/// <summary>
		/// A <see cref="Lump{Vertex}"/> of <see cref="Vertex"/> objects in the BSP file representing the patch vertices of the BSP, if available.
		/// </summary>
		public Lump<Vertex> PatchVertices {
			get {
				if (_patchVertices == null) {
					int index = VertexExtensions.GetIndexForPatchVertsLump(MapType);
					if (index >= 0) {
						// Hax: CoD maps will read Vertex lump with version 1 as simply Vector3s rather than vertices.
						LumpInfo lumpInfo = this[index];
						lumpInfo.version = 1;
						_patchVertices = VertexExtensions.LumpFactory(Reader.ReadLump(this[index]), this, lumpInfo);
					}
				}

				return _patchVertices;
			}
		}

		/// <summary>
		/// Has the Patch Vertices lump been loaded yet?
		/// </summary>
		public bool PatchVertsLoaded {
			get {
				return _patchVertices != null;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Patch Vertex Indices lump, if available.
		/// </summary>
		public NumList PatchIndices {
			get {
				if (_patchIndices == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForPatchIndicesLump(MapType, out type);
					if (index >= 0) {
						_patchIndices = NumList.LumpFactory(Reader.ReadLump(this[index]), type, this, this[index]);
					}
				}

				return _patchIndices;
			}
		}

		/// <summary>
		/// Has the Patch Indices lump been loaded yet?
		/// </summary>
		public bool PatchIndicesLoaded {
			get {
				return _patchIndices != null;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Leaf Patches lump, if available.
		/// </summary>
		public NumList LeafPatches {
			get {
				if (_leafPatches == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForLeafPatchesLump(MapType, out type);
					if (index >= 0) {
						_leafPatches = NumList.LumpFactory(Reader.ReadLump(this[index]), type, this, this[index]);
					}
				}

				return _leafPatches;
			}
		}

		/// <summary>
		/// Has the Leaf Patches lump been loaded yet?
		/// </summary>
		public bool LeafPatchesLoaded {
			get {
				return _leafPatches != null;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Face Vertex Indices lump, if available.
		/// </summary>
		public NumList Indices {
			get {
				if (_indices == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForIndicesLump(MapType, out type);
					if (index >= 0) {
						_indices = NumList.LumpFactory(Reader.ReadLump(this[index]), type, this, this[index]);
					}
				}

				return _indices;
			}
		}

		/// <summary>
		/// Has the Indices lump been loaded yet?
		/// </summary>
		public bool IndicesLoaded {
			get {
				return _indices != null;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Texture offsets table lump, if available.
		/// </summary>
		public NumList TextureTable {
			get {
				if (_textureTable == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForTexTableLump(MapType, out type);
					if (index >= 0) {
						_textureTable = NumList.LumpFactory(Reader.ReadLump(this[index]), type, this, this[index]);
					}
				}

				return _textureTable;
			}
		}

		/// <summary>
		/// Has the Texture Table lump been loaded yet?
		/// </summary>
		public bool TextureTableLoaded {
			get {
				return _textureTable != null;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Displacement Triangles lump, if available.
		/// </summary>
		public NumList DisplacementTriangles {
			get {
				if (_displacementTriangles == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForDisplacementTrianglesLump(MapType, out type);
					if (index >= 0) {
						_displacementTriangles = NumList.LumpFactory(Reader.ReadLump(this[index]), type, this, this[index]);
					}
				}

				return _displacementTriangles;
			}
		}

		/// <summary>
		/// Has the Displacement Triangles lump been loaded yet?
		/// </summary>
		public bool DisplacementTrianglesLoaded {
			get {
				return _displacementTriangles != null;
			}
		}

		/// <summary>
		/// The <see cref="LibBSP.GameLump"/> object in the BSP file containing internal lumps, if available.
		/// </summary>
		public GameLump GameLump {
			get {
				if (_gameLump == null) {
					int index = GameLump.GetIndexForLump(MapType);
					if (index >= 0) {
						_gameLump = GameLump.LumpFactory(Reader.ReadLump(this[index]), this, this[index]);
					}
				}

				return _gameLump;
			}
		}

		/// <summary>
		/// Has the Patch Indices lump been loaded yet?
		/// </summary>
		public bool GameLumpLoaded {
			get {
				return _patchIndices != null;
			}
		}

		/// <summary>
		/// The <see cref="StaticProps"/> object in the BSP file extracted from the <see cref="BSP.gameLump"/>, if available.
		/// </summary>
		public StaticProps StaticProps {
			get {
				if (GameLump != null) {
					return GameLump.StaticProps;
				}

				return null;
			}
		}

		/// <summary>
		/// Has the Static Props lump been loaded yet?
		/// </summary>
		public bool StaticPropsLoaded {
			get {
				return GameLumpLoaded && GameLump.StaticPropsLoaded;
			}
		}

		/// <summary>
		/// Gets the name of this map.
		/// </summary>
		public string MapName {
			get; private set;
		}

		/// <summary>
		/// Gets the <see cref="LumpInfo"/> object associated with the lump with index "<paramref name="index"/>".
		/// </summary>
		/// <param name="index">Index of the lump to get information for.</param>
		/// <returns>A <see cref="LumpInfo"/> object containing information about lump "<paramref name="index"/>".</returns>
		public new LumpInfo this[int index] {
			get {
				if (!ContainsKey(index)) {
					base[index] = Reader.GetLumpInfo(index, MapType);
				}
				return base[index];
			}
		}

		/// <summary>
		/// Creates a new <see cref="BSP"/> instance pointing to the file at <paramref name="filePath"/>. The
		/// <c>List</c>s in this class will be read and populated when accessed through their properties.
		/// </summary>
		/// <param name="filePath">The path to the .BSP file.</param>
		public BSP(string filePath) : base(16) {
			Reader = new BSPReader(new FileInfo(filePath));
			MapName = Path.GetFileName(filePath);
			MapName = MapName.Substring(0, MapName.Length - 4);
		}

		/// <summary>
		/// Creates a new <see cref="BSP"/> instance using the file referenced by <paramref name="file"/>. The
		/// <c>List</c>s in this class will be read and populated when accessed through their properties.
		/// </summary>
		/// <param name="file">A reference to the .BSP file.</param>
		public BSP(FileInfo file) : base(16) {
			Reader = new BSPReader(file);
			MapName = Path.GetFileName(file.FullName);
			MapName = MapName.Substring(0, MapName.Length - 4);
		}

		/// <summary>
		/// Gets the number of lumps in a given BSP version.
		/// </summary>
		/// <param name="version">The version to get the number of lumps for.</param>
		/// <returns>The number of lumps used by a BSP of version <paramref name="version"/>.</returns>
		public static int GetNumLumps(MapType version) {
			if (version == MapType.Titanfall) {
				return 128;
			} else if (version.IsSubtypeOf(MapType.Source)) {
				return 64;
			} else if (version.IsSubtypeOf(MapType.Quake)) {
				return 15;
			} else if (version.IsSubtypeOf(MapType.MOHAA)) {
				return 28;
			} else if (version.IsSubtypeOf(MapType.STEF2)) {
				return 30;
			} else if (version.IsSubtypeOf(MapType.FAKK2)
				|| version == MapType.SiN) {
				return 20;
			} else if (version == MapType.Raven
				|| version == MapType.Nightfire) {
				return 18;
			} else if (version == MapType.Quake2) {
				return 19;
			} else if (version == MapType.Daikatana) {
				return 21;
			} else if (version == MapType.SoF) {
				return 22;
			} else if (version == MapType.CoD || version == MapType.CoDDemo) {
				return 31;
			} else if (version == MapType.CoD2) {
				return 39;
			} else if (version == MapType.CoD4) {
				return 55;
			} else if (version == MapType.Quake3
				|| version == MapType.ET) {
				return 17;
			}

			return -1;
		}

		/// <summary>
		/// Gets all objects of type <typeparamref name="T"/> referenced through passed object <paramref name="o"/>
		/// contained in the lump <paramref name="lumpName"/> stored in this <see cref="BSP"/> class. This is done by
		/// reflecting the <c>Type</c> of <paramref name="o"/> and looping through its public properties to find
		/// a member with an <see cref="IndexAttribute"/> attribute and a member with a <see cref="CountAttribute"/> attribute
		/// both corresponding to <paramref name="lumpName"/>. The index and count are obtained and used to construct
		/// a new <c>List&lt;<typeparamref name="T"/>&gt;</c> object containing the corresponding objects.
		/// </summary>
		/// <typeparam name="T">The type of <c>object</c> stored in the lump <paramref name="lumpName"/>.</typeparam>
		/// <param name="o">The <c>object</c> which contains and index and count corresponding to <paramref name="lumpName"/>.</param>
		/// <param name="lumpName">The name of the property in this <see cref="BSP"/> object to get a <c>List</c> of objects from.</param>
		/// <returns>The <c>List&lt;<typeparamref name="T"/>&gt;</c> of objects in the lump from the index and length specified in <paramref name="o"/>.</returns>
		/// <exception cref="ArgumentException">The <see cref="BSP"/> class contains no property corresponding to <paramref name="lumpName"/>.</exception>
		/// <exception cref="ArgumentException">The <c>object</c> referenced by <paramref name="o"/> is missing one or both members with <c>IndexAttribute</c> or <c>CountAttribute</c> attributes corresponding to <paramref name="lumpName"/>.</exception>
		/// <exception cref="ArgumentNullException">One or both of <paramref name="o"/> or <paramref name="lumpName"/> is null.</exception>
		public List<T> GetReferencedObjects<T>(object o, string lumpName) {
			if (o == null) {
				throw new ArgumentNullException("Object cannot be null.");
			}
			if (lumpName == null) {
				throw new ArgumentNullException("Lump name cannot be null.");
			}
			// First, find the property in this class corresponding to lumpName, and grab its "get" method
			PropertyInfo targetLump = typeof(BSP).GetProperty(lumpName, BindingFlags.Public | BindingFlags.Instance);
			if (targetLump == null) {
				throw new ArgumentException("The lump " + lumpName + " does not exist in the BSP class.");
			}

			// Next, find the properties in the passed object corresponding to lumpName, through the Index and Length custom attributes
			Type objectType = o.GetType();
			PropertyInfo[] objectProperties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			PropertyInfo indexProperty = null;
			PropertyInfo countProperty = null;
			foreach (PropertyInfo info in objectProperties) {
				IndexAttribute indexAttribute = info.GetCustomAttribute<IndexAttribute>();
				if (indexAttribute != null) {
					if (indexAttribute.lumpName == lumpName) {
						indexProperty = info;
						if (indexProperty != null && countProperty != null) {
							break;
						}
					}
				}
				CountAttribute lengthAttribute = info.GetCustomAttribute<CountAttribute>();
				if (lengthAttribute != null) {
					if (lengthAttribute.lumpName == lumpName) {
						countProperty = info;
						if (indexProperty != null && countProperty != null) {
							break;
						}
					}
				}
			}
			if (indexProperty == null || countProperty == null) {
				throw new ArgumentException("An object of type " + objectType.Name + " does not implement both an Index and Count for lump " + lumpName + ".");
			}

			// Get the index and length from the object
			int index = (int)(indexProperty.GetGetMethod().Invoke(o, null));
			int count = (int)(countProperty.GetGetMethod().Invoke(o, null));

			// Get the lump from this class
			IList<T> theLump = targetLump.GetGetMethod().Invoke(this, null) as IList<T>;

			// Copy items from the lump into a return list.
			// IList<T> lacks AddRange and this is faster and creates less garbage than any Linq trickery I could come up with.
			// Passing references to IList<T> out of this method just eats obscene amounts of memory until the system runs out.
			List<T> ret = new List<T>(count);
			for (int i = 0; i < count; ++i) {
				ret.Add(theLump[index + i]);
			}
			return ret;
		}

	}
}
