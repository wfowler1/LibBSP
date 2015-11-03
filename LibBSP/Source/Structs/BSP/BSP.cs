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
	public enum MapType : int {
		Undefined = 0,
		Quake = 29,
		// TYPE_GOLDSRC = 30, // Uses same algorithm and structures as Quake
		Nightfire = 42,
		Vindictus = 346131372,
		STEF2 = 556942937,
		MOHAA = 892416069,
		// TYPE_MOHBT = 1095516506, // Similar enough to MOHAA to use the same structures and algorithm
		Doom = 1145132868, // "DWAD"
		Hexen = 1145132872, // "HWAD"
		STEF2Demo = 1263223129,
		FAKK = 1263223152,
		TacticalInterventionEncrypted = 1268885814,
		CoD2 = 1347633741, // Uses same algorithm and structures as COD1. Read differently.
		SiN = 1347633747, // The headers for SiN and Jedi Outcast are exactly the same
		Raven = 1347633748,
		CoD4 = 1347633759, // Uses same algorithm and structures as COD1. Read differently.
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
		// TYPE_RTCW = 1347633784, // Uses same algorithm and structures as Quake 3
		CoD = 1347633796,
		DMoMaM = 1347895914,
	}

	/// <summary>
	/// Holds data for any and all BSP formats. Any unused lumps in a given format
	/// will be left as null. Then it will be fed into a universal decompile method
	/// which should be able to perform its job based on what data is stored.
	/// </summary>
	public class BSP {

		private MapType _version;

		private BSPReader reader;

		// Map structures
		// Quake 1/GoldSrc
		private Entities _entities;
		private List<Plane> _planes;
		private Textures _textures;
		private List<UIVertex> _vertices;
		private List<Node> _nodes;
		private List<TexInfo> _texInfo;
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
		private List<SourceTexData> _texDatas;
		private List<SourceDispInfo> _dispInfos;
		private SourceDispVertices _dispVerts;
		private NumList _displacementTriangles;
		// public SourceOverlays overlays;
		private List<SourceCubemap> _cubemaps;
		private GameLump _gameLump;
		private SourceStaticProps _staticProps;

		/// <summary>
		/// An XOr encryption key for encrypted map formats. Must be read and set.
		/// </summary>
		public byte[] key = new byte[0];

		/// <summary>
		/// The version of this BSP. DO NOT CHANGE THIS unless you want to force reading a BSP as a certain format.
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

		public bool bigEndian { get { return reader.bigEndian; } }

		public Entities entities {
			get {
				if (_entities == null) {
					int index = Entity.GetIndexForLump(version);
					if (index >= 0) {
						_entities = Entity.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _entities;
			}
		}

		public List<Plane> planes {
			get {
				if (_planes == null) {
					int index = PlaneExtensions.GetIndexForLump(version);
					if (index >= 0) {
						_planes = PlaneExtensions.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _planes;
			}
		}

		public Textures textures {
			get {
				if (_textures == null) {
					int index = Texture.GetIndexForLump(version);
					if (index >= 0) {
						_textures = Texture.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _textures;
			}
		}

		public List<UIVertex> vertices {
			get {
				if (_vertices == null) {
					int index = UIVertexExtensions.GetIndexForLump(version);
					if (index >= 0) {
						_vertices = UIVertexExtensions.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _vertices;
			}
		}

		public List<Node> nodes {
			get {
				if (_nodes == null) {
					int index = Node.GetIndexForLump(version);
					if (index >= 0) {
						_nodes = Node.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _nodes;
			}
		}

		public List<TexInfo> texInfo {
			get {
				if (_texInfo == null) {
					int index = TexInfo.GetIndexForLump(version);
					if (index >= 0) {
						_texInfo = TexInfo.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _texInfo;
			}
		}

		public List<Face> faces {
			get {
				if (_faces == null) {
					int index = Face.GetIndexForLump(version);
					if (index >= 0) {
						_faces = Face.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _faces;
			}
		}

		public List<Leaf> leaves {
			get {
				if (_leaves == null) {
					int index = Leaf.GetIndexForLump(version);
					if (index >= 0) {
						_leaves = Leaf.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _leaves;
			}
		}

		public List<Edge> edges {
			get {
				if (_edges == null) {
					int index = Edge.GetIndexForLump(version);
					if (index >= 0) {
						_edges = Edge.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _edges;
			}
		}

		public List<Model> models {
			get {
				if (_models == null) {
					int index = Model.GetIndexForLump(version);
					if (index >= 0) {
						_models = Model.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _models;
			}
		}

		public List<Brush> brushes {
			get {
				if (_brushes == null) {
					int index = Brush.GetIndexForLump(version);
					if (index >= 0) {
						_brushes = Brush.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _brushes;
			}
		}

		public List<BrushSide> brushSides {
			get {
				if (_brushSides == null) {
					int index = BrushSide.GetIndexForLump(version);
					if (index >= 0) {
						_brushSides = BrushSide.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _brushSides;
			}
		}

		public Textures materials {
			get {
				if (_materials == null) {
					int index = Texture.GetIndexForMaterialLump(version);
					if (index >= 0) {
						_materials = Texture.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _materials;
			}
		}

		public List<Face> originalFaces {
			get {
				if (_originalFaces == null) {
					int index = Face.GetIndexForOriginalFacesLump(version);
					if (index >= 0) {
						_originalFaces = Face.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _originalFaces;
			}
		}

		public List<SourceTexData> texDatas {
			get {
				if (_texDatas == null) {
					int index = SourceTexData.GetIndexForLump(version);
					if (index >= 0) {
						_texDatas = SourceTexData.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _texDatas;
			}
		}

		public List<SourceDispInfo> dispInfos {
			get {
				if (_dispInfos == null) {
					int index = SourceDispInfo.GetIndexForLump(version);
					if (index >= 0) {
						_dispInfos = SourceDispInfo.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _dispInfos;
			}
		}

		public SourceDispVertices dispVerts {
			get {
				if (_dispVerts == null) {
					int index = SourceDispVertex.GetIndexForLump(version);
					if (index >= 0) {
						_dispVerts = SourceDispVertex.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _dispVerts;
			}
		}

		public List<SourceCubemap> cubemaps {
			get {
				if (_cubemaps == null) {
					int index = SourceCubemap.GetIndexForLump(version);
					if (index >= 0) {
						_cubemaps = SourceCubemap.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _cubemaps;
			}
		}

		public NumList markSurfaces {
			get {
				if (_markSurfaces == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForMarkSurfacesLump(version, out type);
					if (index >= 0) {
						_markSurfaces = NumList.LumpFactory(reader.ReadLumpNum(index, version), type);
					}
				}
				return _markSurfaces;
			}
		}

		public NumList surfEdges {
			get {
				if (_surfEdges == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForSurfEdgesLump(version, out type);
					if (index >= 0) {
						_surfEdges = NumList.LumpFactory(reader.ReadLumpNum(index, version), type);
					}
				}
				return _surfEdges;
			}
		}

		public NumList markBrushes {
			get {
				if (_markBrushes == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForMarkBrushesLump(version, out type);
					if (index >= 0) {
						_markBrushes = NumList.LumpFactory(reader.ReadLumpNum(index, version), type);
					}
				}
				return _markBrushes;
			}
		}

		public NumList indices {
			get {
				if (_indices == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForIndicesLump(version, out type);
					if (index >= 0) {
						_indices = NumList.LumpFactory(reader.ReadLumpNum(index, version), type);
					}
				}
				return _indices;
			}
		}

		public NumList texTable {
			get {
				if (_texTable == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForTexTableLump(version, out type);
					if (index >= 0) {
						_texTable = NumList.LumpFactory(reader.ReadLumpNum(index, version), type);
					}
				}
				return _texTable;
			}
		}

		public NumList displacementTriangles {
			get {
				if (_displacementTriangles == null) {
					NumList.DataType type;
					int index = NumList.GetIndexForDisplacementTrianglesLump(version, out type);
					if (index >= 0) {
						_displacementTriangles = NumList.LumpFactory(reader.ReadLumpNum(index, version), type);
					}
				}
				return _displacementTriangles;
			}
		}

		public GameLump gameLump {
			get {
				if (_gameLump == null) {
					int index = GameLump.GetIndexForLump(version);
					if (index >= 0) {
						_gameLump = GameLump.LumpFactory(reader.ReadLumpNum(index, version), version);
					}
				}
				return _gameLump;
			}
		}

		public SourceStaticProps staticProps {
			get {
				if (_staticProps == null) {
					if (gameLump != null && gameLump.ContainsKey(GameLumpType.sprp)) {
						GameLump.GameLumpInfo info = gameLump[GameLumpType.sprp];
						byte[] thisLump = new byte[info.length];
						Array.Copy(gameLump.rawData, info.offset - gameLump.gameLumpOffset, thisLump, 0, info.length);
						_staticProps = SourceStaticProp.LumpFactory(thisLump, version, info.version);
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
		/// Creates a new <c>BSP</c> instance pointing to the file at <paramref name="filePath"/>. The
		/// <c>List</c>s in this class will be read and populated when accessed through their properties.
		/// </summary>
		/// <param name="filePath">The path to the .BSP file</param>
		public BSP(string filePath) {
			reader = new BSPReader(new FileInfo(filePath));
			this.filePath = filePath;
		}

		/// <summary>
		/// Creates a new <c>BSP</c> instance using the file referenced by <paramref name="file"/>. The
		/// <c>List</c>s in this class will be read and populated when accessed through their properties.
		/// </summary>
		/// <param name="file">A reference to the .BSP file</param>
		public BSP(FileInfo file) {
			reader = new BSPReader(file);
			this.filePath = file.FullName;
		}

		/// <summary>
		/// Tells the <see cref="BSPReader"/> object to release file handles for the BSP file.
		/// </summary>
		public void Close() {
			reader.Close();
		}

		/// <summary>
		/// Gets all objects of type <typeparamref name="T"/> referenced through passed object <paramref name="o"/>
		/// contained in the lump <paramref name="lumpName"/> stored in this <c>BSP</c> class. This is done by
		/// reflecting the <c>Type</c> of <paramref name="o"/> and looping through its public properties to find
		/// a member with an <c>IndexAttribute</c> attribute and a member with <c>CountAttribute</c> attribute
		/// both corresponding to <paramref name="lumpName"/>. The index and count are obtained and used to construct
		/// a new <c>List&lt;<typeparamref name="T"/>&gt;</c> object containing the corresponding objects.
		/// </summary>
		/// <typeparam name="T">The type of <c>object</c> stored in the lump <paramref name="lumpName"/>.</typeparam>
		/// <param name="o">The <c>object</c> which contains and index and count corresponding to <paramref name="lumpName"/>.</param>
		/// <param name="lumpName">The name of the property in this <c>BSP</c> object to get a <c>List</c> of objects from.</param>
		/// <returns>The <c>List&lt;<typeparamref name="T"/>&gt;</c> of objects in the lump from the index and length specified in <paramref name="o"/>.</returns>
		/// <exception cref="ArgumentException">The <c>BSP</c> class contains no property corresponding to <paramref name="lumpName"/>.</exception>
		/// <exception cref="ArgumentException">The <c>object</c> referenced by <paramref name="o"/> is missing one or both members with <c>IndexAttribute</c> or <c>CountAttribute</c> attributes corresponding to <paramref name="lumpName"/>.</exception>
		public List<T> GetReferencedObjects<T>(object o, string lumpName)
		{
			// First, find the property in this class corresponding to lumpName, and grab its "get" method
			PropertyInfo targetLump = typeof(BSP).GetProperty(lumpName, BindingFlags.Public | BindingFlags.Instance);
			if (targetLump == null) {
				throw new ArgumentException("The lump " + lumpName + " does not exist in the BSP class");
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
				throw new ArgumentException("An object of type " + objectType.Name + " does not implement both an Index and Count for lump " + lumpName);
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
