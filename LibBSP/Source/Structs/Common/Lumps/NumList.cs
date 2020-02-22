using System;
using System.Collections;
using System.Collections.Generic;

namespace LibBSP {

	/// <summary>
	/// List class for numbers. Can handle any integer data type except <c>ulong</c>.
	/// </summary>
	public class NumList : IList<long>, ICollection<long>, IEnumerable<long>, IList, ICollection, IEnumerable, ILump {

		/// <summary>
		/// The <see cref="BSP"/> this <see cref="ILump"/> came from.
		/// </summary>
		public BSP Bsp { get; protected set; }

		/// <summary>
		/// The <see cref="LumpInfo"/> associated with this <see cref="ILump"/>.
		/// </summary>
		public LumpInfo LumpInfo { get; protected set; }

		/// <summary>
		/// Enum of the types that may be used in this class.
		/// </summary>
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

		public byte[] data;
		public DataType type { get; private set; }

		/// <summary>
		/// Creates a new <see cref="NumList"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The type of number to store.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public NumList(byte[] data, DataType type, BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			Bsp = bsp;
			LumpInfo = lumpInfo;
			this.data = data;
			this.type = type;
		}

		/// <summary>
		/// Creates an empty <see cref="NumList"/> object.
		/// </summary>
		/// <param name="type">The type of number to store.</param>
		public NumList(DataType type) {
			this.data = new byte[0];
			this.type = type;
		}

		/// <summary>
		/// Creates a new <see cref="NumList"/> object from a <c>byte</c> array and returns it.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The type of number to store.</param>
		/// <returns>The resulting <see cref="NumList"/>.</returns>
		public static NumList LumpFactory(byte[] data, DataType type, BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) {
			return new NumList(data, type, bsp, lumpInfo);
		}

		/// <summary>
		/// Gets the length, in bytes, of the numerical primitive used by this instance of this class.
		/// </summary>
		public int StructLength {
			get {
				switch (type) {
					case DataType.Byte:
					case DataType.SByte: {
						return sizeof(sbyte);
					}
					case DataType.UInt16:
					case DataType.Int16: {
						return sizeof(short);
					}
					case DataType.UInt32:
					case DataType.Int32: {
						return sizeof(int);
					}
					case DataType.Int64: {
						return sizeof(long);
					}
					default: {
						return 0;
					}
				}
			}
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
				case MapType.L4D2:
				case MapType.DMoMaM: {
					dataType = DataType.UInt16;
					return 16;
				}
				case MapType.CoD: {
					dataType = DataType.UInt32;
					return 23;
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
				case MapType.L4D2:
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
				case MapType.L4D2:
				case MapType.DMoMaM: {
					dataType = DataType.UInt16;
					return 17;
				}
				case MapType.CoD: {
					dataType = DataType.UInt32;
					return 22;
				}
				case MapType.CoD2: {
					dataType = DataType.UInt32;
					return 27;
				}
				case MapType.CoD4: {
					dataType = DataType.UInt32;
					return 29;
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
				case MapType.CoD: {
					dataType = DataType.UInt16;
					return 8;
				}
				case MapType.CoD2: {
					dataType = DataType.UInt16;
					return 9;
				}
				case MapType.CoD4: {
					dataType = DataType.UInt16;
					return 11;
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
		/// Gets the index for the patch indices lump in the BSP file for a specific map format, and the type of data the format uses.
		/// </summary>
		/// <param name="version">The map type.</param>
		/// <param name="dataType"><c>out</c> parameter that will contain the data type this version uses.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForPatchIndicesLump(MapType version, out DataType dataType) {
			switch (version) {
				case MapType.CoD: {
					dataType = DataType.UInt16;
					return 26;
				}
			}
			dataType = DataType.Invalid;
			return -1;
		}

		/// <summary>
		/// Gets the index for the leaf patch indices lump in the BSP file for a specific map format, and the type of data the format uses.
		/// </summary>
		/// <param name="version">The map type.</param>
		/// <param name="dataType"><c>out</c> parameter that will contain the data type this version uses.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForLeafPatchesLump(MapType version, out DataType dataType) {
			switch (version) {
				case MapType.CoD:{
					dataType = DataType.UInt32;
					return 26;
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
				case MapType.L4D2:
				case MapType.DMoMaM:
				case MapType.Titanfall: {
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
				case MapType.L4D2:
				case MapType.DMoMaM: {
					dataType = DataType.UInt16;
					return 48;
				}
			}
			dataType = DataType.Invalid;
			return -1;
		}
		#endregion

		#region ICollection
		public void Add(long value) {
			byte[] temp = new byte[data.Length + StructLength];
			Array.Copy(data, 0, temp, 0, data.Length);
			byte[] bytes = BitConverter.GetBytes(value);
			Array.Copy(bytes, 0, temp, data.Length, StructLength);
			data = temp;
		}

		public void Clear() {
			data = new byte[0];
		}

		public bool Contains(long value) {
			foreach (long curr in this) {
				if (curr == value) {
					return true;
				}
			}
			return false;
		}

		public void CopyTo(long[] array, int arrayIndex) {
			for (int i = 0; i < Count; ++i) {
				array[i + arrayIndex] = this[i];
			}
		}

		void ICollection.CopyTo(Array array, int arrayIndex) {
			for (int i = 0; i < Count; ++i) {
				array.SetValue(this[i], i + arrayIndex);
			}
		}

		public bool Remove(long value) {
			for (int i = 0; i < Count; ++i) {
				if (this[i] == value) {
					RemoveAt(i);
					return true;
				}
			}

			return false;
		}

		public int Count {
			get {
				return data.Length / StructLength;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public object SyncRoot {
			get {
				return data.SyncRoot;
			}
		}

		public bool IsSynchronized {
			get { return data.IsSynchronized; }
		}
		#endregion

		#region IEnumerable
		public IEnumerator<long> GetEnumerator() {
			for (int i = 0; i < Count; ++i) {
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			for (int i = 0; i < Count; ++i) {
				yield return this[i];
			}
		}
		#endregion

		#region IList
		public int Add(object obj) {
			if (obj is byte || obj is sbyte || obj is short || obj is ushort || obj is int || obj is uint || obj is long) {
				Add((long)obj);
				return Count - 1;
			}

			return -1;
		}

		public bool Contains(object obj) {
			if (obj is byte || obj is sbyte || obj is short || obj is ushort || obj is int || obj is uint || obj is long) {
				return Contains((long)obj);
			}

			return false;
		}

		public int IndexOf(object obj) {
			if (obj is byte || obj is sbyte || obj is short || obj is ushort || obj is int || obj is uint || obj is long) {
				return IndexOf((long)obj);
			}

			return -1;
		}

		public void Insert(int index, object obj) {
			if (obj is byte || obj is sbyte || obj is short || obj is ushort || obj is int || obj is uint || obj is long) {
				Insert(index, (long)obj);
			}
		}

		public void Remove(object obj) {
			if (obj is byte || obj is sbyte || obj is short || obj is ushort || obj is int || obj is uint || obj is long) {
				Remove((long)obj);
			}
		}

		public int IndexOf(long value) {
			for (int i = 0; i < Count; ++i) {
				if (this[i] == value) {
					return i;
				}
			}

			return -1;
		}

		object IList.this[int index] {
			get {
				return this[index];
			}
			set {
				if (value is byte || value is sbyte || value is short || value is ushort || value is int || value is uint || value is long) {
					this[index] = (long)value;
				}
			}
		}

		public bool IsFixedSize {
			get {
				return false;
			}
		}

		public void Insert(int index, long value) {
			byte[] temp = new byte[data.Length + StructLength];
			Array.Copy(data, 0, temp, 0, StructLength * index);
			byte[] bytes = BitConverter.GetBytes(value);
			Array.Copy(bytes, 0, temp, StructLength * index, StructLength);
			Array.Copy(data, StructLength * index, temp, StructLength * (index + 1), data.Length - StructLength * index);
			data = temp;
		}

		public void RemoveAt(int index) {
			byte[] temp = new byte[data.Length - StructLength];
			Array.Copy(data, 0, temp, 0, StructLength * index);
			Array.Copy(data, StructLength * (index + 1), temp, StructLength * index, data.Length - (StructLength * (index + 1)));
			data = temp;
		}

		public long this[int index] {
			get {
				switch (type) {
					case DataType.SByte: {
						return (sbyte)data[index];
					}
					case DataType.Byte: {
						return data[index];
					}
					case DataType.Int16: {
						return BitConverter.ToInt16(data, index * 2);
					}
					case DataType.UInt16: {
						return BitConverter.ToUInt16(data, index * 2);
					}
					case DataType.Int32: {
						return BitConverter.ToInt32(data, index * 4);
					}
					case DataType.UInt32: {
						return BitConverter.ToUInt32(data, index * 4);
					}
					case DataType.Int64: {
						return BitConverter.ToInt64(data, index * 8);
					}
					default: {
						return 0;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case DataType.SByte:
					case DataType.Byte: {
						data[index] = bytes[0];
						break;
					}
					case DataType.Int16:
					case DataType.UInt16: {
						data[index * 2] = bytes[0];
						data[(index * 2) + 1] = bytes[1];
						break;
					}
					case DataType.Int32:
					case DataType.UInt32: {
						data[index * 4] = bytes[0];
						data[(index * 4) + 1] = bytes[1];
						data[(index * 4) + 2] = bytes[2];
						data[(index * 4) + 3] = bytes[3];
						break;
					}
					case DataType.Int64: {
						bytes.CopyTo(data, index * 8);
						break;
					}
				}
			}
		}
		#endregion
	}
}
