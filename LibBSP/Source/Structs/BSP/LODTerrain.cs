using System;
using System.Collections.Generic;

namespace LibBSP {

	/// <summary>
	/// Holds the data for a terrain object in MoHAA.
	/// </summary>
	public struct LODTerrain {

		public byte[] data;
		public MapType type;
		public int version;

		public byte flags {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return data[0];
					}
					default: {
						return 0;
					}
				}
			}
			set {
				switch (type) {
					case MapType.MOHAA: {
						data[0] = value;
						break;
					}
				}
			}
		}
		
		public byte scale {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return data[1];
					}
					default: {
						return 0;
					}
				}
			}
			set {
				switch (type) {
					case MapType.MOHAA: {
						data[1] = value;
						break;
					}
				}
			}
		}
		
		public byte[] lightmapCoords {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return new byte[] { data[2], data[3] };
					}
					default: {
						return null;
					}
				}
			}
			set {
				if (value.Length != 2) {
					throw new ArgumentException("LightmapCoords array must have 2 elements.");
				}
				switch (type) {
					case MapType.MOHAA: {
						data[2] = value[0];
						data[3] = value[1];
						break;
					}
				}
			}
		}
		
		public float[] textureCoords {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return new float[] {
							BitConverter.ToSingle(data, 4),
							BitConverter.ToSingle(data, 8),
							BitConverter.ToSingle(data, 12),
							BitConverter.ToSingle(data, 16),
							BitConverter.ToSingle(data, 20),
							BitConverter.ToSingle(data, 24),
							BitConverter.ToSingle(data, 28),
							BitConverter.ToSingle(data, 32),
						};
					}
					default: {
						return null;
					}
				}
			}
			set {
				if (value.Length != 8) {
					throw new ArgumentException("TextureCoords array must have 8 elements.");
				}
				switch (type) {
					case MapType.MOHAA: {
						int offset = 4;
						for (int i = 0; i < value.Length; ++i) {
							BitConverter.GetBytes(value[i]).CopyTo(data, offset + (i * 4));
						}
						break;
					}
				}
			}
		}
		
		public sbyte x {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return (sbyte)data[36];
					}
					default: {
						return 0;
					}
				}
			}
			set {
				switch (type) {
					case MapType.MOHAA: {
						data[36] = (byte)value;
						break;
					}
				}
			}
		}
		
		public sbyte y {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return (sbyte)data[37];
					}
					default: {
						return 0;
					}
				}
			}
			set {
				switch (type) {
					case MapType.MOHAA: {
						data[37] = (byte)value;
						break;
					}
				}
			}
		}
		
		public short baseZ {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return BitConverter.ToInt16(data, 38);
					}
					default: {
						return 0;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.MOHAA: {
						bytes.CopyTo(data, 38);
						break;
					}
				}
			}
		}
		
		public ushort texture {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return BitConverter.ToUInt16(data, 40);
					}
					default: {
						return 0;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.MOHAA: {
						bytes.CopyTo(data, 40);
						break;
					}
				}
			}
		}
		
		public short lightmap {
			get {
				switch (type) {
					case MapType.MOHAA: {
						return BitConverter.ToInt16(data, 42);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.MOHAA: {
						bytes.CopyTo(data, 42);
						break;
					}
				}
			}
		}
		
		public ushort[,] vertexFlags {
			get {
				ushort[,] ret = new ushort[2,63];
				switch (type) {
					case MapType.MOHAA: {
						for (int i = 0; i < ret.GetLength(0); ++i) {
							for (int j = 0; j < ret.GetLength(1); ++j) {
								ret[i, j] = BitConverter.ToUInt16(data, 52 + (i * 126) + (j * 2));
							}
						}
						break;
					}
				}
				return ret;
			}
			set {
				if (value.GetLength(0) != 2 || value.GetLength(1) != 63) {
					throw new ArgumentException("VertexFlags array must be size (2, 63) elements.");
				}
				for (int i = 0; i < value.GetLength(0); ++i) {
					for (int j = 0; j < value.GetLength(1); ++j) {
						BitConverter.GetBytes(value[i, j]).CopyTo(data, 52 + (i * 126) + (j * 2));
					}
				}
			}
		}
		
		public byte[,] heightmap {
			get {
				byte[,] ret = new byte[9, 9];
				switch (type) {
					case MapType.MOHAA: {
						for (int i = 0; i < ret.GetLength(0); ++i) {
							for (int j = 0; j < ret.GetLength(1); ++j) {
								ret[i, j] = data[304 + (i * 9) + j];
							}
						}
						break;
					}
				}
				return ret;
			}
			set {
				if (value.GetLength(0) != 9 || value.GetLength(1) != 9) {
					throw new ArgumentException("Heightmap array must be size (9, 9) elements.");
				}
				for (int i = 0; i < value.GetLength(0); ++i) {
					for (int j = 0; j < value.GetLength(1); ++j) {
						data[304 + (i * 9) + j] = value[i, j];
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="LODTerrain"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public LODTerrain(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
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
			int numObjects = data.Length / structLength;
			List<LODTerrain> lump = new List<LODTerrain>(numObjects);
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
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
