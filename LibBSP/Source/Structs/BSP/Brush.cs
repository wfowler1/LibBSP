using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// Holds the data used by the brush structures of all formats of BSP.
	/// </summary>
	public struct Brush {

		[Index("brushSides")] public int firstSide { get; private set; }
		[Count("brushSides")] public int numSides { get; private set; }
		public int texture { get; private set; }
		public int contents { get; private set; }

		/// <summary>
		/// Creates a new <see cref="Brush"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public Brush(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			firstSide = -1;
			numSides = -1;
			texture = -1;
			contents = -1;
			switch (type) {
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
				case MapType.Vindictus:
				case MapType.TacticalInterventionEncrypted:
				case MapType.DMoMaM: {
					firstSide = BitConverter.ToInt32(data, 0);
					numSides = BitConverter.ToInt32(data, 4);
					contents = BitConverter.ToInt32(data, 8);
					break;
				}
				case MapType.Nightfire: {
					contents = BitConverter.ToInt32(data, 0);
					firstSide = BitConverter.ToInt32(data, 4);
					numSides = BitConverter.ToInt32(data, 8);
					break;
				}
				case MapType.MOHAA:
				case MapType.STEF2Demo:
				case MapType.Raven:
				case MapType.Quake3:
				case MapType.FAKK: {
					firstSide = BitConverter.ToInt32(data, 0);
					numSides = BitConverter.ToInt32(data, 4);
					texture = BitConverter.ToInt32(data, 8);
					break;
				}
				case MapType.STEF2: {
					numSides = BitConverter.ToInt32(data, 0);
					firstSide = BitConverter.ToInt32(data, 4);
					texture = BitConverter.ToInt32(data, 8);
					break;
				}
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4: {
					numSides = BitConverter.ToInt16(data, 0);
					texture = BitConverter.ToInt16(data, 2);
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Brush class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="Brush"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="Brush"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<Brush> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
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
				case MapType.TacticalInterventionEncrypted:
				case MapType.Vindictus:
				case MapType.DMoMaM:
				case MapType.Nightfire:
				case MapType.STEF2:
				case MapType.MOHAA:
				case MapType.STEF2Demo:
				case MapType.Raven:
				case MapType.Quake3:
				case MapType.FAKK: {
					structLength = 12;
					break;
				}
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4: {
					structLength = 4;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Brush lump factory.");
				}
			}
			List<Brush> lump = new List<Brush>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Brush(bytes, type, version));
			}
			return lump;
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.CoD: {
					return 4;
				}
				case MapType.CoD2: {
					return 6;
				}
				case MapType.CoD4:
				case MapType.Raven:
				case MapType.Quake3: {
					return 8;
				}
				case MapType.FAKK: {
					return 11;
				}
				case MapType.MOHAA: {
					return 12;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 13;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 14;
				}
				case MapType.Nightfire: {
					return 15;
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
				case MapType.DMoMaM: {
					return 18;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
