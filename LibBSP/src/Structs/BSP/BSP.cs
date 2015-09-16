#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
#define UNITY
#endif

using System;
using System.Collections.Generic;
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
		TacticalIntervention = 1268885814,
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

		public MapType version { get; set; }

		public string filePath { get; private set; }

		// Map structures
		// Quake 1/GoldSrc
		public Entities entities;
		public List<Plane> planes;
		public Textures textures;
		public List<UIVertex> vertices;
		public List<Node> nodes;
		public List<TexInfo> texInfo;
		public List<Face> faces;
		public List<Leaf> leaves;
		public NumList markSurfaces;
		public List<Edge> edges;
		public NumList surfEdges;
		public List<Model> models;
		public byte[] pvs;
		// Quake 2
		public List<Brush> brushes;
		public List<BrushSide> brushSides;
		public NumList markBrushes;
		// MOHAA
		// public MoHAAStaticProps staticProps;
		// Nightfire
		public Textures materials;
		public NumList indices;
		// Source
		public List<Face> originalFaces;
		public NumList texTable;
		public List<SourceTexData> texDatas;
		public List<SourceDispInfo> dispInfos;
		public SourceDispVertices dispVerts;
		public NumList displacementTriangles;
		public SourceStaticProps staticProps;
		public List<SourceCubemap> cubemaps;
		// public SourceOverlays overlays;

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
		/// Creates a new <c>BSP</c> instance with no initial data. The <c>List</c>s in this class must be populated elsewhere.
		/// </summary>
		/// <param name="filePath">The path to the .BSP file</param>
		/// <param name="version">The version of the map</param>
		public BSP(string filePath, MapType version) {
			this.filePath = filePath;
			this.version = version;
		}
	}
}
