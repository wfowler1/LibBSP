#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if UNITY
	using Vector2d = Vector2;
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
	public struct Face {

		public int plane { get; private set; }
		public int side { get; private set; }
		[Index("edges")] public int firstEdge { get; private set; }
		[Count("edges")] public int numEdges { get; private set; }
		public int texture { get; private set; }
		[Index("vertices")] public int firstVertex { get; private set; }
		[Count("vertices")] public int numVertices { get; private set; }
		public int material { get; private set; }
		public int textureInfo { get; private set; }
		public int displacement { get; private set; }
		public int original { get; private set; }
		public int flags { get; private set; }
		[Index("indices")] public int firstIndex { get; private set; }
		[Count("indices")] public int numIndices { get; private set; }
		public int unknown { get; private set; }
		public int lightStyles { get; private set; }
		public int lightMaps { get; private set; }
		public Vector2d patchSize { get; private set; }

		/// <summary>
		/// Creates a new <see cref="Face"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public Face(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			plane = -1;
			side = -1;
			firstEdge = -1;
			numEdges = -1;
			texture = -1;
			firstVertex = -1;
			numVertices = -1;
			material = -1;
			textureInfo = -1;
			displacement = -1;
			original = -1;
			flags = -1;
			firstIndex = -1;
			numIndices = -1;
			unknown = -1;
			lightStyles = -1;
			lightMaps = -1;
			patchSize = new Vector2d(float.NaN, float.NaN);
			switch (type) {
				case MapType.CoD: {
					texture = BitConverter.ToInt16(data, 0);
					firstVertex = BitConverter.ToInt32(data, 4);
					numVertices = BitConverter.ToInt16(data, 8);
					numIndices = BitConverter.ToInt16(data, 10);
					firstIndex = BitConverter.ToInt32(data, 12);
					break;
				}
				case MapType.Quake:
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.SiN:
				case MapType.SoF: {
					plane = BitConverter.ToUInt16(data, 0);
					side = BitConverter.ToUInt16(data, 2);
					firstEdge = BitConverter.ToInt32(data, 4);
					numEdges = BitConverter.ToUInt16(data, 8);
					texture = BitConverter.ToUInt16(data, 10);
					break;
				}
				case MapType.Quake3:
				case MapType.Raven:
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.MOHAA:
				case MapType.FAKK: {
					texture = BitConverter.ToInt32(data, 0);
					flags = BitConverter.ToInt32(data, 8);
					firstVertex = BitConverter.ToInt32(data, 12);
					numVertices = BitConverter.ToInt32(data, 16);
					firstIndex = BitConverter.ToInt32(data, 20);
					numIndices = BitConverter.ToInt32(data, 24);
					patchSize = new Vector2d(BitConverter.ToInt32(data, 96), BitConverter.ToInt32(data, 100));
					break;
				}
				case MapType.Source17: {
					plane = BitConverter.ToUInt16(data, 32);
					side = (int)data[34];
					firstEdge = BitConverter.ToInt32(data, 36);
					numEdges = BitConverter.ToUInt16(data, 40);
					textureInfo = BitConverter.ToUInt16(data, 42);
					displacement = BitConverter.ToInt16(data, 44);
					original = BitConverter.ToInt32(data, 96);
					break;
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
					plane = BitConverter.ToUInt16(data, 0);
					side = (int)data[2];
					firstEdge = BitConverter.ToInt32(data, 4);
					numEdges = BitConverter.ToUInt16(data, 8);
					textureInfo = BitConverter.ToUInt16(data, 10);
					displacement = BitConverter.ToInt16(data, 12);
					original = BitConverter.ToInt32(data, 44);
					break;
				}
				case MapType.Vindictus: {
					plane = BitConverter.ToInt32(data, 0);
					side = (int)data[4];
					firstEdge = BitConverter.ToInt32(data, 8);
					numEdges = BitConverter.ToInt32(data, 12);
					textureInfo = BitConverter.ToInt32(data, 16);
					displacement = BitConverter.ToInt32(data, 20);
					if (version == 2) {
						original = BitConverter.ToInt32(data, 60);
					} else {
						original = BitConverter.ToInt32(data, 56);
					}
					break;
				}
				case MapType.Nightfire: {
					plane = BitConverter.ToInt32(data, 0);
					firstVertex = BitConverter.ToInt32(data, 4);
					numVertices = BitConverter.ToInt32(data, 8);
					firstIndex = BitConverter.ToInt32(data, 12);
					numIndices = BitConverter.ToInt32(data, 16);
					flags = BitConverter.ToInt32(data, 20);
					texture = BitConverter.ToInt32(data, 24);
					material = BitConverter.ToInt32(data, 28);
					textureInfo = BitConverter.ToInt32(data, 32);
					unknown = BitConverter.ToInt32(data, 36);
					lightStyles = BitConverter.ToInt32(data, 40);
					lightMaps = BitConverter.ToInt32(data, 44);
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Face class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="Face"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="Face"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<Face> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.CoD: {
					structLength = 16;
					break;
				}
				case MapType.Quake:
				case MapType.Quake2:
				case MapType.Daikatana: {
					structLength = 20;
					break;
				}
				case MapType.SiN: {
					structLength = 36;
					break;
				}
				case MapType.SoF: {
					structLength = 40;
					break;
				}
				case MapType.Nightfire: {
					structLength = 48;
					break;
				}
				case MapType.Source17: {
					structLength = 104;
					break;
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
					structLength = 56;
					break;
				}
				case MapType.Vindictus: {
					if (version == 2) {
						structLength = 76;
					} else {
						structLength = 72;
					}
					break;
				}
				case MapType.Quake3: {
					structLength = 104;
					break;
				}
				case MapType.FAKK:
				case MapType.MOHAA: {
					structLength = 108;
					break;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					structLength = 132;
					break;
				}
				case MapType.Raven: {
					structLength = 148;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Face lump factory.");
				}
			}
			int numObjects = data.Length / structLength;
			List<Face> lump = new List<Face>(numObjects);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < numObjects; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Face(bytes, type, version));
			}
			return lump;
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
				case MapType.Nightfire: {
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
				default: {
					return -1;
				}
			}
		}

	}
}
