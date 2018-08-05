using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// Holds the data for a terrain object in MoHAA.
	/// </summary>
	public struct LODTerrain {

		public byte flags { get; private set; }
		public byte scale { get; private set; }
		public byte[] lightmapCoords { get; private set; }
		public float[] textureCoords { get; private set; }
		public sbyte x { get; private set; }
		public sbyte y { get; private set; }
		public short baseZ { get; private set; }
		public ushort texture { get; private set; }
		public short lightmap { get; private set; }
		public short[][] vertexFlags { get; private set; }
		public byte[][] heightmap { get; private set; }

		/// <summary>
		/// Creates a new <see cref="LODTerrain"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public LODTerrain(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			lightmapCoords = new byte[2];
			textureCoords = new float[8];
			vertexFlags = new short[2][];
			heightmap = new byte[9][];
			switch (type) {
				case MapType.MOHAA: {
					flags = data[0];
					scale = data[1];
					lightmapCoords[0] = data[2];
					lightmapCoords[1] = data[3];
					for (int i = 0; i < 8; ++i) {
						textureCoords[i] = BitConverter.ToSingle(data, 4 + (4 * i));
					}
					x = (sbyte)data[36];
					y = (sbyte)data[37];
					baseZ = BitConverter.ToInt16(data, 38);
					texture = BitConverter.ToUInt16(data, 40);
					lightmap = BitConverter.ToInt16(data, 42);
					for (int i = 0; i < vertexFlags.Length; ++i) {
						vertexFlags[i] = new short[63];
						for (int j = 0; j < vertexFlags[i].Length; ++j) {
							vertexFlags[i][j] = BitConverter.ToInt16(data, 52 + (i * 126) + (j * 2));
						}
					}
					for (int i = 0; i < heightmap.Length; ++i) {
						heightmap[i] = new byte[9];
						for (int j = 0; j < heightmap[i].Length; ++j) {
							heightmap[i][j] = data[304 + (i * 9) + j];
						}
					}
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the LODTerrain class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="LODTerrain"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="LODTerrain"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<LODTerrain> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.MOHAA: {
					structLength = 388;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the LODTerrain lump factory.");
				}
			}
			List<LODTerrain> lump = new List<LODTerrain>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new LODTerrain(bytes, type, version));
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
				case MapType.MOHAA: {
					return 22;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
