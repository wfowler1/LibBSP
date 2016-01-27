using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// List class for numbers. Can handle any integer data type except <c>ulong</c>.
	/// </summary>
	public class NumList : List<long> {

		public enum DataType : int {
			Invalid = 0,
			SByte = 1,
			Byte = 2,
			Int16 = 3,
			UInt16 = 4,
			Int32 = 5,
			UInt32 = 6,
			Int64 = 7,
		}

		public DataType type { get; private set; }

		/// <summary>
		/// Creates a new <see cref="NumList"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The type of number to store.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException"><paramref name="type"/> was not a member of the DataType enum.</exception>
		public NumList(byte[] data, DataType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.type = type;
			switch (type) {
				case DataType.SByte: {
					unchecked {
						for (int i = 0; i < data.Length; ++i) {
							Add((long)((sbyte)data[i]));
						}
					}
					break;
				}
				case DataType.Byte: {
					for (int i = 0; i < data.Length; ++i) {
						Add((long)(data[i]));
					}
					break;
				}
				case DataType.Int16: {
					for (int i = 0; i < data.Length / 2; ++i) {
						Add((long)BitConverter.ToInt16(data, i * 2));
					}
					break;
				}
				case DataType.UInt16: {
					for (int i = 0; i < data.Length / 2; ++i) {
						Add((long)BitConverter.ToUInt16(data, i * 2));
					}
					break;
				}
				case DataType.Int32: {
					for (int i = 0; i < data.Length / 4; ++i) {
						Add((long)BitConverter.ToInt32(data, i * 4));
					}
					break;
				}
				case DataType.UInt32: {
					for (int i = 0; i < data.Length / 4; ++i) {
						Add((long)BitConverter.ToUInt32(data, i * 4));
					}
					break;
				}
				case DataType.Int64: {
					for (int i = 0; i < data.Length / 8; ++i) {
						Add(BitConverter.ToInt64(data, i * 8));
					}
					break;
				}
				default: {
					throw new ArgumentException(type + " isn't a member of the DataType enum.");
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="NumList"/> object from a <c>byte</c> array and returns it.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The type of number to store.</param>
		/// <returns>The resulting <see cref="NumList"/>.</returns>
		public static NumList LumpFactory(byte[] data, DataType type) {
			return new NumList(data, type);
		}

		#region IndicesForLumps
		/// <summary>
		/// Gets the index for the Mark Surfaces lump in the BSP file for a specific map format, and the type of data the format uses.
		/// </summary>
		/// <param name="version">The map type.</param>
		/// <param name="dataType"><c>out</c> parameter that will contain the data type this version uses.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForMarkSurfacesLump(MapType version, out DataType dataType) {
			switch (version) {
				case MapType.Raven:
				case MapType.Quake3: {
					dataType = DataType.Int32;
					return 5;
				}
				case MapType.FAKK:
				case MapType.MOHAA: {
					dataType = DataType.Int32;
					return 7;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					dataType = DataType.UInt16;
					return 9;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					dataType = DataType.UInt32;
					return 9;
				}
				case MapType.Quake: {
					dataType = DataType.UInt16;
					return 11;
				}
				case MapType.Nightfire: {
					dataType = DataType.UInt32;
					return 12;
				}
				case MapType.Vindictus: {
					dataType = DataType.UInt32;
					return 16;
				}
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
					dataType = DataType.UInt16;
					return 16;
				}
			}
			dataType = DataType.Invalid;
			return -1;
		}

		/// <summary>
		/// Gets the index for the Surface Edges lump in the BSP file for a specific map format, and the type of data the format uses.
		/// </summary>
		/// <param name="version">The map type.</param>
		/// <param name="dataType"><c>out</c> parameter that will contain data type this version uses.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForSurfEdgesLump(MapType version, out DataType dataType) {
			switch (version) {
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					dataType = DataType.Int32;
					return 12;
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
					dataType = DataType.Int32;
					return 13;
				}
			}
			dataType = DataType.Invalid;
			return -1;
		}

		/// <summary>
		/// Gets the index for the Mark Brushes lump in the BSP file for a specific map format, and the type of data the format uses.
		/// </summary>
		/// <param name="version">The map type.</param>
		/// <param name="dataType"><c>out</c> parameter that will contain the data type this version uses.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForMarkBrushesLump(MapType version, out DataType dataType) {
			switch (version) {
				case MapType.Quake3:
				case MapType.MOHAA:
				case MapType.FAKK: {
					dataType = DataType.UInt32;
					return 6;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					dataType = DataType.UInt32;
					return 8;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					dataType = DataType.UInt16;
					return 10;
				}
				case MapType.Nightfire: {
					dataType = DataType.UInt32;
					return 13;
				}
				case MapType.Vindictus: {
					dataType = DataType.UInt32;
					return 17;
				}
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
					dataType = DataType.UInt16;
					return 17;
				}
			}
			dataType = DataType.Invalid;
			return -1;
		}

		/// <summary>
		/// Gets the index for the indices lump in the BSP file for a specific map format, and the type of data the format uses.
		/// </summary>
		/// <param name="version">The map type.</param>
		/// <param name="dataType"><c>out</c> parameter that will contain the data type this version uses.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForIndicesLump(MapType version, out DataType dataType) {
			switch (version) {
				case MapType.FAKK:
				case MapType.MOHAA: {
					dataType = DataType.UInt32;
					return 5;
				}
				case MapType.Nightfire: {
					dataType = DataType.UInt32;
					return 6;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					dataType = DataType.UInt32;
					return 7;
				}
				case MapType.Raven:
				case MapType.Quake3: {
					dataType = DataType.UInt32;
					return 11;
				}
			}
			dataType = DataType.Invalid;
			return -1;
		}

		/// <summary>
		/// Gets the index for the texture table lump in the BSP file for a specific map format, and the type of data the format uses.
		/// </summary>
		/// <param name="version">The map type.</param>
		/// <param name="dataType"><c>out</c> parameter that will contain the data type this version uses.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForTexTableLump(MapType version, out DataType dataType) {
			switch (version) {
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
					dataType = DataType.Int32;
					return 44;
				}
			}
			dataType = DataType.Invalid;
			return -1;
		}

		/// <summary>
		/// Gets the index for the displacement triangles lump in the BSP file for a specific map format, and the type of data the format uses.
		/// </summary>
		/// <param name="version">The map type.</param>
		/// <param name="dataType"><c>out</c> parameter that will contain the data type this version uses.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForDisplacementTrianglesLump(MapType version, out DataType dataType) {
			switch (version) {
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
					dataType = DataType.UInt16;
					return 48;
				}
			}
			dataType = DataType.Invalid;
			return -1;
		}
		#endregion
	}
}
