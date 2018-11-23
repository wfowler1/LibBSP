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

		public byte[] data;
		public MapType type;
		public int version;

		public int plane {
			get {
				switch (type) {
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
						return BitConverter.ToUInt16(data, 0);
					}
					case MapType.Source17: {
						return BitConverter.ToUInt16(data, 32);
					}
					case MapType.Nightfire:
					case MapType.Vindictus: {
						return BitConverter.ToInt32(data, 0);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
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
						data[0] = bytes[0];
						data[1] = bytes[1];
						break;
					}
					case MapType.Source17: {
						data[32] = bytes[0];
						data[33] = bytes[1];
						break;
					}
					case MapType.Nightfire:
					case MapType.Vindictus: {
						bytes.CopyTo(data, 0);
						break;
					}
				}
			}
		}
		
		public int side {
			get {
				switch (type) {
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF: {
						return BitConverter.ToUInt16(data, 2);
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
						return (int)data[2];
					}
					case MapType.Vindictus: {
						return (int)data[4];
					}
					case MapType.Source17: {
						return (int)data[34];
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF: {
						data[2] = bytes[0];
						data[3] = bytes[1];
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
						data[2] = bytes[0];
						break;
					}
					case MapType.Vindictus: {
						data[4] = bytes[0];
						break;
					}
					case MapType.Source17: {
						data[34] = bytes[0];
						break;
					}
				}
			}
		}
		
		[Index("edges")] public int firstEdge {
			get {
				switch (type) {
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
						return BitConverter.ToInt32(data, 4);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(data, 8);
					}
					case MapType.Source17: {
						return BitConverter.ToInt32(data, 36);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
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
						bytes.CopyTo(data, 4);
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(data, 8);
						break;
					}
					case MapType.Source17: {
						bytes.CopyTo(data, 36);
						break;
					}
				}
			}
		}
		
		[Count("edges")] public int numEdges {
			get {
				switch (type) {
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
						return BitConverter.ToUInt16(data, 8);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(data, 12);
					}
					case MapType.Source17: {
						return BitConverter.ToUInt16(data, 40);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
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
						data[8] = bytes[0];
						data[9] = bytes[1];
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(data, 12);
						break;
					}
					case MapType.Source17: {
						data[40] = bytes[0];
						data[41] = bytes[1];
						break;
					}
				}
			}
		}
		
		public int texture {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt16(data, 0);
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return BitConverter.ToInt32(data, 0);
					}
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF: {
						return BitConverter.ToUInt16(data, 10);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 24);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
						data[0] = bytes[0];
						data[1] = bytes[1];
						break;
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						bytes.CopyTo(data, 0);
						break;
					}
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF: {
						data[10] = bytes[0];
						data[11] = bytes[1];
						break;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(data, 24);
						break;
					}
				}
			}
		}
		
		[Index("vertices")] public int firstVertex {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 4);
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return BitConverter.ToInt32(data, 12);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.Nightfire: {
						bytes.CopyTo(data, 4);
						break;
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						bytes.CopyTo(data, 12);
						break;
					}
				}
			}
		}
		
		[Count("vertices")] public int numVertices {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt16(data, 8);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 8);
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return BitConverter.ToInt32(data, 16);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
						data[8] = bytes[0];
						data[9] = bytes[1];
						break;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(data, 8);
						break;
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						bytes.CopyTo(data, 16);
						break;
					}
				}
			}
		}
		
		public int material {
			get {
				switch (type) {
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 28);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.Nightfire: {
						bytes.CopyTo(data, 28);
						break;
					}
				}
			}
		}
		
		public int textureInfo {
			get {
				switch (type) {
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
						return BitConverter.ToUInt16(data, 10);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(data, 16);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 32);
					}
					case MapType.Source17: {
						return BitConverter.ToUInt16(data, 42);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
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
						data[10] = bytes[0];
						data[11] = bytes[1];
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(data, 16);
						break;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(data, 32);
						break;
					}
					case MapType.Source17: {
						data[42] = bytes[0];
						data[43] = bytes[1];
						break;
					}
				}
			}
		}
		
		public int displacement {
			get {
				switch (type) {
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
						return BitConverter.ToInt16(data, 12);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(data, 20);
					}
					case MapType.Source17: {
						return BitConverter.ToInt16(data, 44);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
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
						data[12] = bytes[0];
						data[13] = bytes[1];
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(data, 20);
						break;
					}
					case MapType.Source17: {
						data[44] = bytes[0];
						data[45] = bytes[1];
						break;
					}
				}
			}
		}
		
		public int original {
			get {
				switch (type) {
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
						return BitConverter.ToInt32(data, 44);
					}
					case MapType.Vindictus: {
						if (version == 2) {
							return BitConverter.ToInt32(data, 60);
						}
						else {
							return BitConverter.ToInt32(data, 56);
						}
					}
					case MapType.Source17: {
						return BitConverter.ToInt32(data, 96);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
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
						bytes.CopyTo(data, 44);
						break;
					}
					case MapType.Vindictus: {
						if (version == 2) {
							bytes.CopyTo(data, 60);
						}
						else {
							bytes.CopyTo(data, 56);
						}
						break;
					}
					case MapType.Source17: {
						bytes.CopyTo(data, 96);
						break;
					}
				}
			}
		}
		
		public int flags {
			get {
				switch (type) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return BitConverter.ToInt32(data, 8);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 20);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						bytes.CopyTo(data, 8);
						break;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(data, 20);
						break;
					}
				}
			}
		}
		
		[Index("indices")] public int firstIndex {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 12);
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return BitConverter.ToInt32(data, 20);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.Nightfire: {
						bytes.CopyTo(data, 12);
						break;
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						bytes.CopyTo(data, 20);
						break;
					}
				}
			}
		}
		
		[Count("indices")] public int numIndices {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt16(data, 10);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 16);
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return BitConverter.ToInt32(data, 24);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
						data[10] = bytes[0];
						data[11] = bytes[1];
						return;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(data, 16);
						break;
					}
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						bytes.CopyTo(data, 24);
						break;
					}
				}
			}
		}
		
		public int unknown {
			get {
				switch (type) {
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 36);
					}
					default: {
						return 0;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.Nightfire: {
						bytes.CopyTo(data, 36);
						break;
					}
				}
			}
		}
		
		public int lightStyles {
			get {
				switch (type) {
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 40);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.Nightfire: {
						bytes.CopyTo(data, 40);
						break;
					}
				}
			}
		}
		
		public int lightMaps {
			get {
				switch (type) {
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 44);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.Nightfire: {
						bytes.CopyTo(data, 44);
						break;
					}
				}
			}
		}
		
		public Vector2d patchSize {
			get {
				switch (type) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						return new Vector2d(BitConverter.ToInt32(data, 96), BitConverter.ToInt32(data, 100));
					}
					default: {
						return new Vector2d(float.NaN, float.NaN);
					}
				}
			}
			set {
				switch (type) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK: {
						byte[] bytes = BitConverter.GetBytes((int)value.x);
						bytes.CopyTo(data, 96);
						bytes = BitConverter.GetBytes((int)value.y);
						bytes.CopyTo(data, 100);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Face"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Face(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
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
				case MapType.CoD:
				case MapType.CoD2: {
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
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
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
				case MapType.CoD2: {
					return 7;
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
