#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP.Doom {
	/// <summary>
	/// Gathers all relevant information from the lumps of a Doom Map.
	/// </summary>
	public class DoomMap {

		// Since all Doom engine maps were incorporated into the WAD, we need to keep
		// track of both the location of the WAD file and the internal name of the map.
		public string wadPath { get; private set; }
		public string mapName { get; private set; }
		public MapType version { get; private set; }

		public List<Thing> things;
		public List<Linedef> linedefs;
		public List<Sidedef> sidedefs;
		public List<UIVertex> vertices;
		public List<Segment> segs;
		public List<Edge> subsectors;
		public List<Node> nodes;
		public List<Sector> sectors;

		/// <summary>
		/// Gets the folder path for the WAD containing this map
		/// </summary>
		public string Folder {
			get {
				int i;
				for (i = 0; i < wadPath.Length; ++i) {
					if (wadPath[wadPath.Length - 1 - i] == '\\') {
						break;
					}
					if (wadPath[wadPath.Length - 1 - i] == '/') {
						break;
					}
				}
				return wadPath.Substring(0, (wadPath.Length - i) - (0));
			}
		}

		/// <summary>
		/// Gets the name of the WAD file containing this map
		/// </summary>
		public string WadName {
			get {
				System.IO.FileInfo newFile = new System.IO.FileInfo(wadPath);
				return newFile.Name.Substring(0, (newFile.Name.Length - 4) - (0));
			}
		}

		/// <summary>
		/// Creates a new <c>DoomMap</c> instance with no initial data. The <c>List</c>s in this class must be populated elsewhere.
		/// </summary>
		/// <param name="wadpath">The path to the .WAD file</param>
		/// <param name="map">The name of the map within the WAD file</param>
		/// <param name="version">The version of the map</param>
		public DoomMap(string wadpath, string map, MapType version) {
			this.wadPath = wadpath;
			this.mapName = map;
			this.version = version;
		}
	}
}
