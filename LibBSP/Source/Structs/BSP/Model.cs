using System;
using System.Collections.Generic;

namespace LibBSP {

	/// <summary>
	/// A class containing all data needed for the models lump in any given BSP.
	/// </summary>
	/// <remarks>
	/// In general, we need to use models to find one or more leaves containing the
	/// information for the solids described by this model. Some formats do it by
	/// referencing a head node to iterate through and find the leaves. Others
	/// directly point to a set of leaves, and still others simply directly reference
	/// brushes. The ideal format simply points to brush information from here (Quake
	/// 3-based engines do), but most of them don't.
	/// </remarks>
	public struct Model {

		public byte[] data;
		public MapType type;
		public int version;

		[Index("nodes")] public int headNode {
			get {
				switch (type) {
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
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
					case MapType.Vindictus: {
						return BitConverter.ToInt32(data, 36);
					}
					case MapType.DMoMaM: {
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
					case MapType.Quake:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
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
					case MapType.Vindictus: {
						bytes.CopyTo(data, 36);
						break;
					}
					case MapType.DMoMaM: {
						bytes.CopyTo(data, 40);
						break;
					}
				}
			}
		}

		[Index("leaves")] public int firstLeaf {
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

		[Count("leaves")] public int numLeaves {
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

		[Index("brushes")] public int firstBrush {
			get {
				switch (type) {
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Quake3:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.FAKK: {
						return BitConverter.ToInt32(data, 32);
					}
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
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
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Quake3:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.FAKK: {
						bytes.CopyTo(data, 32);
						break;
					}
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						bytes.CopyTo(data, 40);
						break;
					}
				}
			}
		}

		[Count("brushes")] public int numBrushes {
			get {
				switch (type) {
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Quake3:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.FAKK: {
						return BitConverter.ToInt32(data, 36);
					}
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
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
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Quake3:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.FAKK: {
						bytes.CopyTo(data, 36);
						break;
					}
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						bytes.CopyTo(data, 44);
						break;
					}
				}
			}
		}

		[Index("faces")] public int firstFace {
			get {
				switch (type) {
					case MapType.CoD4: {
						return BitConverter.ToInt16(data, 24);
					}
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Quake3:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.FAKK: {
						return BitConverter.ToInt32(data, 24);
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
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
					case MapType.Vindictus: {
						return BitConverter.ToInt32(data, 40);
					}
					case MapType.DMoMaM: {
						return BitConverter.ToInt32(data, 44);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 48);
					}
					case MapType.Quake: {
						return BitConverter.ToInt32(data, 56);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD4: {
						data[24] = bytes[0];
						data[25] = bytes[1];
						break;
					}
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Quake3:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.FAKK: {
						bytes.CopyTo(data, 24);
						break;
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
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
					case MapType.Vindictus: {
						bytes.CopyTo(data, 40);
						break;
					}
					case MapType.DMoMaM: {
						bytes.CopyTo(data, 44);
						break;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(data, 48);
						break;
					}
					case MapType.Quake: {
						bytes.CopyTo(data, 56);
						break;
					}
				}
			}
		}

		[Count("faces")] public int numFaces {
			get {
				switch (type) {
					case MapType.CoD4: {
						return BitConverter.ToInt16(data, 28);
					}
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Quake3:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.FAKK: {
						return BitConverter.ToInt32(data, 28);
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
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
					case MapType.Vindictus: {
						return BitConverter.ToInt32(data, 44);
					}
					case MapType.DMoMaM: {
						return BitConverter.ToInt32(data, 48);
					}
					case MapType.Nightfire: {
						return BitConverter.ToInt32(data, 52);
					}
					case MapType.Quake: {
						return BitConverter.ToInt32(data, 60);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD4: {
						data[28] = bytes[0];
						data[29] = bytes[1];
						break;
					}
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.Quake3:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.FAKK: {
						bytes.CopyTo(data, 28);
						break;
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
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
					case MapType.Vindictus: {
						bytes.CopyTo(data, 44);
						break;
					}
					case MapType.DMoMaM: {
						bytes.CopyTo(data, 48);
						break;
					}
					case MapType.Nightfire: {
						bytes.CopyTo(data, 52);
						break;
					}
					case MapType.Quake: {
						bytes.CopyTo(data, 60);
						break;
					}
				}
			}
		}

		[Index("markSurfaces")]
		public int firstLeafPatch {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(data, 32);
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
						bytes.CopyTo(data, 32);
						break;
					}
				}
			}
		}

		[Count("markSurfaces")] public int numLeafPatches {
			get {
				switch (type) {
					case MapType.CoD:
					case MapType.CoD2: {
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
						bytes.CopyTo(data, 36);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Model"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Model(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="Model"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="Model"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<Model> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Titanfall: {
					structLength = 32;
					break;
				}
				case MapType.Quake3:
				case MapType.Raven:
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.MOHAA:
				case MapType.FAKK: {
					structLength = 40;
					break;
				}
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4:
				case MapType.SiN:
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
				case MapType.Vindictus: {
					structLength = 48;
					break;
				}
				case MapType.DMoMaM: {
					structLength = 52;
					break;
				}
				case MapType.Nightfire: {
					structLength = 56;
					break;
				}
				case MapType.Quake: {
					structLength = 64;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Leaf lump factory.");
				}
			}
			int numObjects = data.Length / structLength;
			List<Model> lump = new List<Model>(numObjects);
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Model(bytes, type, version));
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
					return 7;
				}
				case MapType.MOHAA:
				case MapType.FAKK:
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 13;
				}
				case MapType.Quake:
				case MapType.Nightfire:
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
				case MapType.Titanfall: {
					return 14;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 15;
				}
				case MapType.CoD: {
					return 27;
				}
				case MapType.CoD2: {
					return 35;
				}
				case MapType.CoD4: {
					return 37;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
