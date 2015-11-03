using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// Holds all the data for an edge in a BSP map. Doubles as a subsector class for Doom maps, and has accessors for them.
	/// </summary>
	public class Edge {

		public int firstVertex { get; private set; }
		public int secondVertex { get; private set; }
		public int NumSegs { get { return firstVertex; } private set { firstVertex = value; } }
		public int FirstSeg { get { return secondVertex; } private set { secondVertex = value; } }

		/// <summary>
		/// Creates a new <c>Edge</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public Edge(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			switch (type) {
				case MapType.Quake:
				case MapType.SiN:
				case MapType.Daikatana:
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
				case MapType.Quake2:
				case MapType.SoF: {
					firstVertex = BitConverter.ToUInt16(data, 0);
					secondVertex = BitConverter.ToUInt16(data, 2);
					break;
				}
				case MapType.Vindictus: {
					firstVertex = BitConverter.ToInt32(data, 0);
					secondVertex = BitConverter.ToInt32(data, 4);
					break;
				}
				case MapType.Doom:
				case MapType.Hexen: {
					firstVertex = BitConverter.ToUInt16(data, 0);
					secondVertex = BitConverter.ToUInt16(data, 2);
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Edge class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <c>Edge</c> objects.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>List</c> of <c>Edge</c> objects</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public static List<Edge> LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Quake:
				case MapType.SiN:
				case MapType.Daikatana:
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
				case MapType.Quake2:
				case MapType.SoF: {
					structLength = 4;
					break;
				}
				case MapType.Vindictus: {
					structLength = 8;
					break;
				}
				case MapType.Doom:
				case MapType.Hexen: {
					structLength = 4;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Edge lump factory.");
				}
			}
			List<Edge> lump = new List<Edge>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Edge(bytes, type));
			}
			return lump;
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 11;
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
				case MapType.DMoMaM: {
					return 12;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
