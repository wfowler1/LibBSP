using System;
using System.Collections.Generic;

namespace LibBSP {

	/// <summary>
	/// Contains all data needed for a node in a BSP tree.
	/// </summary>
	public struct Node {

		public byte[] data;
		public MapType type;
		public int version;

		public int plane {
			get {
				return BitConverter.ToInt32(data, 0);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 0);
			}
		}

		public int child1 {
			get {
				switch (type) {
					case MapType.Quake: {
						return BitConverter.ToInt16(data, 4);
					}
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.Nightfire:
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(data, 4);
					}
					default: {
						return 0;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.Quake: {
						data[4] = bytes[0];
						data[5] = bytes[1];
						break;
					}
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.Nightfire:
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(data, 4);
						break;
					}
				}
			}
		}

		public int child2 {
			get {
				switch (type) {
					case MapType.Quake: {
						return BitConverter.ToInt16(data, 6);
					}
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.Nightfire:
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(data, 8);
					}
					default: {
						return 0;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.Quake: {
						data[6] = bytes[0];
						data[7] = bytes[1];
						break;
					}
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.Nightfire:
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(data, 8);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Node"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public Node(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="Node"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="Node"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<Node> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Quake: {
					structLength = 24;
					break;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.SoF:
				case MapType.Daikatana: {
					structLength = 28;
					break;
				}
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
					structLength = 32;
					break;
				}
				case MapType.Vindictus: {
					structLength = 48;
					break;
				}
				case MapType.Quake3:
				case MapType.FAKK:
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.MOHAA:
				case MapType.Raven:
				case MapType.Nightfire: {
					structLength = 36;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Node lump factory.");
				}
			}
			int numObjects = data.Length / structLength;
			List<Node> lump = new List<Node>(numObjects);
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Node(bytes, type, version));
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
					return 3;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 4;
				}
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
					return 5;
				}
				case MapType.Nightfire: {
					return 8;
				}
				case MapType.FAKK:
				case MapType.MOHAA: {
					return 9;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 11;
				}
				case MapType.CoD: {
					return 20;
				}
				case MapType.CoD2: {
					return 25;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
