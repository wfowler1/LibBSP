using System;
using System.Collections.Generic;

namespace LibBSP {

	/// <summary>
	/// Holds data for a leaf structure in a BSP map.
	/// </summary>
	public struct Leaf {

		public byte[] data;
		public MapType type;
		public int version;

		public int contents {
			get {
				switch (type) {
					case MapType.SoF:
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.Daikatana:
					case MapType.Vindictus:
					case MapType.Nightfire: {
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
					case MapType.SoF:
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.Daikatana:
					case MapType.Vindictus:
					case MapType.Nightfire: {
						bytes.CopyTo(data, 0);
						break;
					}
				}
			}
		}
		
		[Index("markBrushes")] public int firstMarkBrush {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(data, 16);
					}
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.Daikatana: {
						return BitConverter.ToUInt16(data, 24);
					}
					case MapType.SoF: {
						return BitConverter.ToUInt16(data, 26);
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 40);
					}
					case MapType.Vindictus: {
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
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(data, 16);
						break;
					}
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.Daikatana: {
						data[24] = bytes[0];
						data[25] = bytes[1];
						break;
					}
					case MapType.SoF: {
						data[26] = bytes[0];
						data[27] = bytes[1];
						break;
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						bytes.CopyTo(data, 40);
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(data, 44);
						break;
					}
				}
			}
		}
		
		[Count("markBrushes")] public int numMarkBrushes {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(data, 20);
					}
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.Daikatana: {
						return BitConverter.ToUInt16(data, 26);
					}
					case MapType.SoF: {
						return BitConverter.ToUInt16(data, 28);
					}
					case MapType.Nightfire:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven: {
						return BitConverter.ToInt32(data, 44);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(data, 48);
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
						bytes.CopyTo(data, 20);
						break;
					}
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.Daikatana: {
						data[26] = bytes[0];
						data[27] = bytes[1];
						break;
					}
					case MapType.SoF: {
						data[28] = bytes[0];
						data[29] = bytes[1];
						break;
					}
					case MapType.Nightfire:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven: {
						bytes.CopyTo(data, 44);
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(data, 48);
						break;
					}
				}
			}
		}
		
		[Index("markSurfaces")] public int firstMarkFace {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(data, 8);
					}
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.Daikatana: {
						return BitConverter.ToUInt16(data, 20);
					}
					case MapType.SoF: {
						return BitConverter.ToUInt16(data, 22);
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 32);
					}
					case MapType.Vindictus: {
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
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(data, 8);
						break;
					}
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.Daikatana: {
						data[20] = bytes[0];
						data[21] = bytes[1];
						break;
					}
					case MapType.SoF: {
						data[22] = bytes[0];
						data[23] = bytes[1];
						break;
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						bytes.CopyTo(data, 32);
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(data, 36);
						break;
					}
				}
			}
		}
		
		[Count("markSurfaces")] public int numMarkFaces {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(data, 12);
					}
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.Daikatana: {
						return BitConverter.ToUInt16(data, 22);
					}
					case MapType.SoF: {
						return BitConverter.ToUInt16(data, 24);
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 36);
					}
					case MapType.Vindictus: {
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
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(data, 12);
						break;
					}
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.Daikatana: {
						data[22] = bytes[0];
						data[23] = bytes[1];
						break;
					}
					case MapType.SoF: {
						data[24] = bytes[0];
						data[25] = bytes[1];
						break;
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						bytes.CopyTo(data, 36);
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(data, 40);
						break;
					}
				}
			}
		}
		
		public int pvs {
			get {
				switch (type) {
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 4);
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
						bytes.CopyTo(data, 4);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Leaf"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public Leaf(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="Leaf"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="Leaf"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<Leaf> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Quake:
				case MapType.Quake2:
				case MapType.SiN: {
					structLength = 28;
					break;
				}
				case MapType.Source17:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.L4D2:
				case MapType.TacticalInterventionEncrypted:
				case MapType.SoF:
				case MapType.Daikatana:
				case MapType.DMoMaM: {
					structLength = 32;
					break;
				}
				case MapType.Vindictus: {
					structLength = 56;
					break;
				}
				case MapType.CoD:
				case MapType.CoD2: {
					structLength = 36;
					break;
				}
				case MapType.Nightfire:
				case MapType.Quake3:
				case MapType.FAKK:
				case MapType.STEF2Demo:
				case MapType.STEF2:
				case MapType.Raven: {
					structLength = 48;
					break;
				}
				case MapType.Source18:
				case MapType.Source19: {
					structLength = 56;
					break;
				}
				case MapType.MOHAA: {
					structLength = 64;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Leaf lump factory.");
				}
			}
			int numObjects = data.Length / structLength;
			List<Leaf> lump = new List<Leaf>(numObjects);
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Leaf(bytes, type, version));
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
				case MapType.Raven:
				case MapType.Quake3: {
					return 4;
				}
				case MapType.MOHAA:
				case MapType.FAKK:
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 8;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.Quake:
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
					return 10;
				}
				case MapType.Nightfire: {
					return 11;
				}
				case MapType.CoD: {
					return 21;
				}
				case MapType.CoD2: {
					return 26;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
