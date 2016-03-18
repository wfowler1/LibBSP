#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
#define UNITY
#endif

using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if !UNITY
	using Vector3 = Vector3d;
#endif
	/// <summary>
	/// Enum of the known different map formats.
	/// </summary>
	public enum MapType : int {
		Undefined = 0,
		Quake = 29,
		// TYPE_GOLDSRC = 30, // Uses mostly the same structures as Quake
		Nightfire = 42,
		Vindictus = 346131372,
		STEF2 = 556942937,
		MOHAA = 892416069,
		// TYPE_MOHBT = 1095516506, // Similar enough to MOHAA to use the same structures
		STEF2Demo = 1263223129,
		FAKK = 1263223152,
		TacticalInterventionEncrypted = 1268885814,
		CoD2 = 1347633741,
		SiN = 1347633747, // The headers for SiN and Jedi Outcast are exactly the same
		Raven = 1347633748,
		CoD4 = 1347633759,
		Source17 = 1347633767,
		Source18 = 1347633768,
		Source19 = 1347633769,
		Source20 = 1347633770,
		Source21 = 1347633771,
		Source22 = 1347633772,
		Source23 = 1347633773,
		L4D2 = 1347633774,
		Quake2 = 1347633775,
		Source27 = 1347633777,
		Daikatana = 1347633778,
		SoF = 1347633782, // Uses the same header as Q3.
		Quake3 = 1347633783,
		// TYPE_RTCW = 1347633784, // Uses same structures as Quake 3
		CoD = 1347633796,
		DMoMaM = 1347895914,
	}

	/// <summary>
	/// Struct containing basic information for a lump in a BSP file.
	/// </summary>
	public class LumpInfo {
		public int ident;
		public int flags;
		public int version;
		public int offset;
		public int length;
		public FileInfo lumpFile;
	}

	/// <summary>
	/// Holds data for any and all BSP formats. Any unused lumps in a given format
	/// will be left as null.
	/// </summary>
	public class BSP : Dictionary<int, LumpInfo> {

		private MapType _version;

		// Map structures
		// Quake 1/GoldSrc
		private Entities _entities;
		private List<Plane> _planes;
		private Textures _textures;
		private List<UIVertex> _vertices;
		private List<Node> _nodes;
		private List<TextureInfo> _texInfo;
		private List<Face> _faces;
		private List<Leaf> _leaves;
		private NumList _markSurfaces;
		private List<Edge> _edges;
		private NumList _surfEdges;
		private List<Model> _models;
		// public byte[] pvs;
		// Quake 2
		private List<Brush> _brushes;
		private List<BrushSide> _brushSides;
		private NumList _markBrushes;
		// Nightfire
		private Textures _materials;
		private NumList _indices;
		// Source
		private List<Face> _originalFaces;
		private NumList _texTable;
		private List<TextureData> _texDatas;
		private List<DisplacementInfo> _dispInfos;
		private DisplacementVertices _dispVerts;
		private NumList _displacementTriangles;
		// public SourceOverlays overlays;
		private List<Cubemap> _cubemaps;
		private GameLump _gameLump;
		private StaticProps _staticProps;

		/// <summary>
		/// The <see cref="BSPReader"/> object in use by this <see cref="BSP"/> class.
		/// </summary>
		public BSPReader reader { get; private set; }

		/// <summary>
		/// The version of this BSP. Do not change this unless you want to force reading a BSP as a certain format.
		/// </summary>
		public MapType version {
			get {
				if (_version == MapType.Undefined) {
					_version = reader.GetVersion();
				}
				return _version;
			}
			set {
				_version = value;
			}
		}

		/// <summary>
		/// Is the BSP file in big endian format?
		/// </summary>
		public bool bigEndian { get { return reader.bigEndian; } }

		/// <summary>
		/// The <see cref="Entities"/> object in the BSP file, if available.
		/// </summary>
		public Entities entities {
			get {
				if (_entities == null) {
					int index = Entity.GetIndexForLump(version);
					if (index >= 0) {
						_entities = Entity.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _entities;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="Plane"/> objects in the BSP file, if available.
		/// </summary>
		public List<Plane> planes {
			get {
				if (_planes == null) {
					int index = PlaneExtensions.GetIndexForLump(version);
					if (index >= 0) {
						_planes = PlaneExtensions.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _planes;
			}
		}

		/// <summary>
		/// The <see cref="Textures"/> object in the BSP file, if available.
		/// </summary>
		public Textures textures {
			get {
				if (_textures == null) {
					int index = Texture.GetIndexForLump(version);
					if (index >= 0) {
						_textures = Texture.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _textures;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="UIVertex"/> objects in the BSP file representing the vertices of the BSP, if available.
		/// </summary>
		public List<UIVertex> vertices {
			get {
				if (_vertices == null) {
					int index = UIVertexExtensions.GetIndexForLump(version);
					if (index >= 0) {
						_vertices = UIVertexExtensions.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _vertices;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="Node"/> objects in the BSP file, if available.
		/// </summary>
		public List<Node> nodes {
			get {
				if (_nodes == null) {
					int index = Node.GetIndexForLump(version);
					if (index >= 0) {
						_nodes = Node.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _nodes;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="TextureInfo"/> objects in the BSP file, if available.
		/// </summary>
		public List<TextureInfo> texInfo {
			get {
				if (_texInfo == null) {
					int index = TextureInfo.GetIndexForLump(version);
					if (index >= 0) {
						_texInfo = TextureInfo.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _texInfo;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="Face"/> objects in the BSP file, if available.
		/// </summary>
		public List<Face> faces {
			get {
				if (_faces == null) {
					int index = Face.GetIndexForLump(version);
					if (index >= 0) {
						_faces = Face.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _faces;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="Leaf"/> objects in the BSP file, if available.
		/// </summary>
		public List<Leaf> leaves {
			get {
				if (_leaves == null) {
					int index = Leaf.GetIndexForLump(version);
					if (index >= 0) {
						_leaves = Leaf.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _leaves;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="Edge"/> objects in the BSP file, if available.
		/// </summary>
		public List<Edge> edges {
			get {
				if (_edges == null) {
					int index = Edge.GetIndexForLump(version);
					if (index >= 0) {
						_edges = Edge.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _edges;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="Model"/> objects in the BSP file, if available.
		/// </summary>
		public List<Model> models {
			get {
				if (_models == null) {
					int index = Model.GetIndexForLump(version);
					if (index >= 0) {
						_models = Model.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _models;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="Brush"/> objects in the BSP file, if available.
		/// </summary>
		public List<Brush> brushes {
			get {
				if (_brushes == null) {
					int index = Brush.GetIndexForLump(version);
					if (index >= 0) {
						_brushes = Brush.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _brushes;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="BrushSide"/> objects in the BSP file, if available.
		/// </summary>
		public List<BrushSide> brushSides {
			get {
				if (_brushSides == null) {
					int index = BrushSide.GetIndexForLump(version);
					if (index >= 0) {
						_brushSides = BrushSide.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _brushSides;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="Texture"/> objects in the BSP file representing Materials (shaders), if available.
		/// </summary>
		public Textures materials {
			get {
				if (_materials == null) {
					int index = Texture.GetIndexForMaterialLump(version);
					if (index >= 0) {
						_materials = Texture.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _materials;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="Face"/> objects in the BSP file representing the Original Faces, if available.
		/// </summary>
		public List<Face> originalFaces {
			get {
				if (_originalFaces == null) {
					int index = Face.GetIndexForOriginalFacesLump(version);
					if (index >= 0) {
						_originalFaces = Face.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _originalFaces;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="TextureData"/> objects in the BSP file, if available.
		/// </summary>
		public List<TextureData> texDatas {
			get {
				if (_texDatas == null) {
					int index = TextureData.GetIndexForLump(version);
					if (index >= 0) {
						_texDatas = TextureData.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _texDatas;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="DisplacementInfo"/> objects in the BSP file, if available.
		/// </summary>
		public List<DisplacementInfo> dispInfos {
			get {
				if (_dispInfos == null) {
					int index = DisplacementInfo.GetIndexForLump(version);
					if (index >= 0) {
						_dispInfos = DisplacementInfo.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _dispInfos;
			}
		}

		/// <summary>
		/// The <see cref="DisplacementVertices"/> object in the BSP file, if available.
		/// </summary>
		public DisplacementVertices dispVerts {
			get {
				if (_dispVerts == null) {
					int index = DisplacementVertex.GetIndexForLump(version);
					if (index >= 0) {
						_dispVerts = DisplacementVertex.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _dispVerts;
			}
		}

		/// <summary>
		/// A <c>List</c> of <see cref="Cubemap"/> objects in the BSP file, if available.
		/// </summary>
		public List<Cubemap> cubemaps {
			get {
				if (_cubemaps == null) {
					int index = Cubemap.GetIndexForLump(version);
					if (index >= 0) {
						_cubemaps = Cubemap.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _cubemaps;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Mark Surfaces (Leaf Surfaces) lump, if available.
		/// </summary>
		public NumList markSurfaces {
			get {
				if (_markSurfaces == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForMarkSurfacesLump(version, out type);
					if (index >= 0) {
						_markSurfaces = NumList.LumpFactory(reader.ReadLump(this[index]), type);
					}
				}
				return _markSurfaces;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Surface Edges lump, if available.
		/// </summary>
		public NumList surfEdges {
			get {
				if (_surfEdges == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForSurfEdgesLump(version, out type);
					if (index >= 0) {
						_surfEdges = NumList.LumpFactory(reader.ReadLump(this[index]), type);
					}
				}
				return _surfEdges;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Mark Brushes (Leaf Brushes) lump, if available.
		/// </summary>
		public NumList markBrushes {
			get {
				if (_markBrushes == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForMarkBrushesLump(version, out type);
					if (index >= 0) {
						_markBrushes = NumList.LumpFactory(reader.ReadLump(this[index]), type);
					}
				}
				return _markBrushes;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Face Vertex Indices lump, if available.
		/// </summary>
		public NumList indices {
			get {
				if (_indices == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForIndicesLump(version, out type);
					if (index >= 0) {
						_indices = NumList.LumpFactory(reader.ReadLump(this[index]), type);
					}
				}
				return _indices;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Texture offsets table lump, if available.
		/// </summary>
		public NumList texTable {
			get {
				if (_texTable == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForTexTableLump(version, out type);
					if (index >= 0) {
						_texTable = NumList.LumpFactory(reader.ReadLump(this[index]), type);
					}
				}
				return _texTable;
			}
		}

		/// <summary>
		/// A <see cref="NumList"/> object containing the Displacement Triangles lump, if available.
		/// </summary>
		public NumList displacementTriangles {
			get {
				if (_displacementTriangles == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForDisplacementTrianglesLump(version, out type);
					if (index >= 0) {
						_displacementTriangles = NumList.LumpFactory(reader.ReadLump(this[index]), type);
					}
				}
				return _displacementTriangles;
			}
		}

		/// <summary>
		/// The <see cref="GameLump"/> object in the BSP file containing internal lumps, if available.
		/// </summary>
		public GameLump gameLump {
			get {
				if (_gameLump == null) {
					int index = GameLump.GetIndexForLump(version);
					if (index >= 0) {
						_gameLump = GameLump.LumpFactory(reader.ReadLump(this[index]), version, this[index].version);
					}
				}
				return _gameLump;
			}
		}

		/// <summary>
		/// The <see cref="StaticProps"/> object in the BSP file extracted from the <see cref="BSP.gameLump"/>, if available.
		/// </summary>
		public StaticProps staticProps {
			get {
				if (_staticProps == null) {
					if (gameLump != null && gameLump.ContainsKey(GameLumpType.sprp)) {
						LumpInfo info = gameLump[GameLumpType.sprp];
						byte[] thisLump = new byte[info.length];
						Array.Copy(gameLump.rawData, info.offset - gameLump.gameLumpOffset, thisLump, 0, info.length);
						_staticProps = StaticProp.LumpFactory(thisLump, version, info.version);
					}
				}
				return _staticProps;
			}
		}

		/// <summary>
		/// Gets the path to this BSP file.
		/// </summary>
		public string filePath { get; private set; }

		/// <summary>
		/// Gets the file name of this map.
		/// </summary>
		public string MapName {
			get {
				int i;
				for (i = 0; i < filePath.Length; ++i) {
					if (filePath[filePath.Length - 1 - i] == '\\') {
						break;
					}
					if (filePath[filePath.Length - 1 - i] == '/') {
						break;
					}
				}
				return filePath.Substring(filePath.Length - i, (filePath.Length) - (filePath.Length - i));
			}
		}

		/// <summary>
		/// Gets the file name of this map without the ".BSP" extension.
		/// </summary>
		public string MapNameNoExtension {
			get {
				string name = MapName;
				int i;
				for (i = 0; i < name.Length; ++i) {
					if (name[name.Length - 1 - i] == '.') {
						break;
					}
				}
				return name.Substring(0, (name.Length - 1 - i) - (0));
			}
		}

		/// <summary>
		/// Gets the folder path where this map is located.
		/// </summary>
		public string Folder {
			get {
				int i;
				for (i = 0; i < filePath.Length; ++i) {
					if (filePath[filePath.Length - 1 - i] == '\\') {
						break;
					}
					if (filePath[filePath.Length - 1 - i] == '/') {
						break;
					}
				}
				return filePath.Substring(0, (filePath.Length - i) - (0));
			}
		}

		/// <summary>
		/// Gets the <see cref="LumpInfo"/> object associated with the lump with index "<paramref name="index"/>".
		/// </summary>
		/// <param name="index">Index of the lump to get information for.</param>
		/// <returns>A <see cref="LumpInfo"/> object containing information about lump "<paramref name="index"/>".</returns>
		public new LumpInfo this[int index] {
			get {
				if (!ContainsKey(index)) {
					base[index] = reader.GetLumpInfo(index, version);
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
			reader = new BSPReader(new FileInfo(filePath));
			this.filePath = filePath;
		}

		/// <summary>
		/// Creates a new <see cref="BSP"/> instance using the file referenced by <paramref name="file"/>. The
		/// <c>List</c>s in this class will be read and populated when accessed through their properties.
		/// </summary>
		/// <param name="file">A reference to the .BSP file.</param>
		public BSP(FileInfo file) : base(16) {
			reader = new BSPReader(file);
			this.filePath = file.FullName;
		}

		/// <summary>
		/// Gets the number of lumps in a given BSP version.
		/// </summary>
		/// <param name="version">The version to get the number of lumps for.</param>
		/// <returns>The number of lumps used by a BSP of version <paramref name="version"/>.</returns>
		public static int GetNumLumps(MapType version) {
			switch (version) {
				case MapType.Quake: {
					return 15;
				}
				case MapType.Daikatana:
				case MapType.Quake2: {
					return 16;
				}
				case MapType.Quake3: {
					return 17;
				}
				case MapType.Raven:
				case MapType.Nightfire: {
					return 18;
				}
				case MapType.FAKK:
				case MapType.SiN: {
					return 20;
				}
				case MapType.SoF: {
					return 22;
				}
				case MapType.MOHAA: {
					return 28;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 30;
				}
				case MapType.CoD:
				case MapType.CoD2: {
					return 31;
				}
				case MapType.CoD4: {
					return 55;
				}
				case MapType.TacticalInterventionEncrypted:
				case MapType.DMoMaM:
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.L4D2:
				case MapType.Vindictus: {
					return 64;
				}
				default: {
					return -1;
				}
			}
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
			int index = (int)indexProperty.GetGetMethod().Invoke(o, null);
			int count = (int)countProperty.GetGetMethod().Invoke(o, null);

			// Get the lump from this class
			List<T> theLump = targetLump.GetGetMethod().Invoke(this, null) as List<T>;
			
			return theLump.GetRange(index, count);
		}
	}
}
