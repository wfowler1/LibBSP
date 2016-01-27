using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// Holds data for a leaf structure in a BSP map.
	/// </summary>
	public struct Leaf {

		public int contents { get; private set; }
		[Index("markBrushes")] public int firstMarkBrush { get; private set; }
		[Count("markBrushes")] public int numMarkBrushes { get; private set; }
		[Index("markSurfaces")] public int firstMarkFace { get; private set; }
		[Count("markSurfaces")] public int numMarkFaces { get; private set; }
		public int pvs { get; private set; }

		/// <summary>
		/// Creates a new <see cref="Leaf"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public Leaf(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			contents = -1;
			firstMarkBrush = -1;
			numMarkBrushes = -1;
			firstMarkFace = -1;
			numMarkFaces = -1;
			pvs = -1;
			switch (type) {
				case MapType.SoF: {
					contents = BitConverter.ToInt32(data, 0);
					firstMarkFace = BitConverter.ToUInt16(data, 22);
					numMarkFaces = BitConverter.ToUInt16(data, 24);
					firstMarkBrush = BitConverter.ToUInt16(data, 26);
					numMarkBrushes = BitConverter.ToUInt16(data, 28);
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
				case MapType.TacticalInterventionEncrypted:
				case MapType.DMoMaM:
				case MapType.Daikatana: {
					contents = BitConverter.ToInt32(data, 0);
					firstMarkBrush = BitConverter.ToUInt16(data, 24);
					numMarkBrushes = BitConverter.ToUInt16(data, 26);
					goto case MapType.Quake;
				}
				case MapType.Quake: {
					firstMarkFace = BitConverter.ToUInt16(data, 20);
					numMarkFaces = BitConverter.ToUInt16(data, 22);
					break;
				}
				case MapType.Vindictus: {
					contents = BitConverter.ToInt32(data, 0);
					firstMarkFace = BitConverter.ToInt32(data, 36);
					numMarkFaces = BitConverter.ToInt32(data, 40);
					firstMarkBrush = BitConverter.ToInt32(data, 44);
					numMarkBrushes = BitConverter.ToInt32(data, 48);
					break;
				}
				case MapType.Nightfire: {
					contents = BitConverter.ToInt32(data, 0);
					pvs = BitConverter.ToInt32(data, 4);
					goto case MapType.Raven;
				}
				case MapType.Quake3:
				case MapType.FAKK:
				case MapType.STEF2Demo:
				case MapType.STEF2:
				case MapType.MOHAA:
				case MapType.Raven: {
					firstMarkFace = BitConverter.ToInt32(data, 32);
					numMarkFaces = BitConverter.ToInt32(data, 36);
					firstMarkBrush = BitConverter.ToInt32(data, 40);
					numMarkBrushes = BitConverter.ToInt32(data, 44);
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Leaf class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="Leaf"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <returns>A <c>List</c> of <see cref="Leaf"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<Leaf> LumpFactory(byte[] data, MapType type) {
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
				case MapType.CoD: {
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
			List<Leaf> lump = new List<Leaf>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Leaf(bytes, type));
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
				case MapType.DMoMaM: {
					return 10;
				}
				case MapType.Nightfire: {
					return 11;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
