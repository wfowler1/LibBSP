using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// List class for numbers. Can handle any integer data type except <c>ulong</c>.
	/// </summary>
	public class NumList : List<long> {

		public enum DataType : int {
			SByte = 0,
			Byte = 1,
			Int16 = 2,
			UInt16 = 3,
			Int32 = 4,
			UInt32 = 5,
			Int64 = 6,
		}

		public DataType type { get; private set; }

		/// <summary>
		/// Creates a new <c>NumList</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The type of number to store</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException"><paramref name="type" /> was not a member of the DataType enum</exception>
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
		/// Creates a new <c>NumList</c> object from a <c>byte</c> array and returns it.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The type of number to store</param>
		/// <returns>The resulting <c>NumList</c></returns>
		public static NumList LumpFactory(byte[] data, DataType type) {
			return new NumList(data, type);
		}
	}
}
