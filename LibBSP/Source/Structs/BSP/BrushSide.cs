using System;
using System.Collections.Generic;

namespace LibBSP {

	/// <summary>
	/// Holds the data used by the brush side structures of all formats of BSP.
	/// </summary>
	public struct BrushSide {

		public byte[] data;
		public MapType type;
		public int version;

		public int plane {
			get {
				switch (type) {
					case MapType.SiN:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SoF:
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
					case MapType.DMoMaM: {
						return BitConverter.ToUInt16(data, 0);
					}
					case MapType.Raven:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.Vindictus: {
						return BitConverter.ToInt32(data, 0);
					}
					case MapType.STEF2:
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
					case MapType.SiN:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SoF:
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
					case MapType.DMoMaM: {
						data[0] = bytes[0];
						data[1] = bytes[1];
						break;
					}
					case MapType.Raven:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.Vindictus: {
						bytes.CopyTo(data, 0);
						break;
					}
					case MapType.STEF2:
					case MapType.Nightfire: {
						bytes.CopyTo(data, 4);
						break;
					}
				}
			}
		}
		
		public float dist {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						return BitConverter.ToSingle(data, 0);
					}
					default: {
						return float.NaN;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						bytes.CopyTo(data, 0);
						break;
					}
				}
			}
		}

		public int texture {
			get {
				switch (type) {
					case MapType.STEF2: {
						return BitConverter.ToInt32(data, 0);
					}
					case MapType.SiN:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SoF:
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
					case MapType.DMoMaM: {
						return BitConverter.ToInt16(data, 2);
					}
					case MapType.Vindictus:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.Raven: {
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
					case MapType.STEF2: {
						bytes.CopyTo(data, 0);
						break;
					}
					case MapType.SiN:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SoF:
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
					case MapType.DMoMaM: {
						data[2] = bytes[0];
						data[3] = bytes[1];
						break;
					}
					case MapType.Vindictus:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.Raven: {
						bytes.CopyTo(data, 4);
						break;
					}
				}
			}
		}
		
		public int face {
			get {
				switch (type) {
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 0);
					}
					case MapType.Raven: {
						return BitConverter.ToInt32(data, 8);
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
						bytes.CopyTo(data, 0);
						break;
					}
					case MapType.Raven: {
						bytes.CopyTo(data, 8);
						break;
					}
				}
			}
		}
		
		public int displacement {
			get {
				switch (type) {
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
					case MapType.DMoMaM: {
						return BitConverter.ToInt16(data, 4);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(data, 8);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
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
					case MapType.DMoMaM: {
						data[4] = bytes[0];
						data[5] = bytes[1];
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(data, 8);
						break;
					}
				}
			}
		}
		
		public bool bevel {
			get {
				switch (type) {
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
					case MapType.DMoMaM: {
						return data[6] > 0;
					}
					case MapType.Vindictus: {
						return data[12] > 0;
					}
					default: {
						return false;
					}
				}
			}
			set {
				switch (type) {
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
					case MapType.DMoMaM: {
						data[6] = (byte)(value ? 1 : 0);
						break;
					}
					case MapType.Vindictus: {
						data[12] = (byte)(value ? 1 : 0);
						break;
					}
				}
			}
		}

		public bool thin {
			get {
				switch (type) {
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
					case MapType.DMoMaM: {
						return data[7] > 0;
					}
					default: {
						return false;
					}
				}
			}
			set {
				switch (type) {
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
					case MapType.DMoMaM: {
						data[7] = (byte)(value ? 1 : 0);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="BrushSide"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public BrushSide(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="BrushSide"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="BrushSide"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<BrushSide> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.SoF: {
					structLength = 4;
					break;
				}
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4:
				case MapType.SiN:
				case MapType.Nightfire:
				case MapType.Quake3:
				case MapType.STEF2:
				case MapType.STEF2Demo:
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
				case MapType.FAKK: {
					structLength = 8;
					break;
				}
				case MapType.MOHAA:
				case MapType.Raven: {
					structLength = 12;
					break;
				}
				case MapType.Vindictus: {
					structLength = 16;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the BrushSide lump factory.");
				}
			}
			int numObjects = data.Length / structLength;
			List<BrushSide> lump = new List<BrushSide>(numObjects);
			for (int i = 0; i < numObjects; i++) {
				byte[] bytes = new byte[structLength];
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new BrushSide(bytes, type, version));
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
				case MapType.CoD: {
					return 3;
				}
				case MapType.CoD4:
				case MapType.CoD2: {
					return 5;
				}
				case MapType.Raven:
				case MapType.Quake3: {
					return 9;
				}
				case MapType.FAKK: {
					return 10;
				}
				case MapType.MOHAA: {
					return 11;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 12;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 15;
				}
				case MapType.Nightfire: {
					return 16;
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
				case MapType.DMoMaM: {
					return 19;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
