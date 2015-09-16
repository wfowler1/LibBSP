using System;
using System.Collections.Generic;

namespace LibBSP.Doom {
	/// <summary>
	/// Contains all necessary information for a Doom LINEDEF object.
	/// </summary>
	/// <remarks>
	/// The linedef is a strange animal. It roughly equates to the Planes of other
	/// BSP formats, but also defines what sectors are on what side.
	/// </remarks>
	public struct Linedef {

		public short start { get; private set; }
		public short end { get; private set; }
		public short flags { get; private set; }
		public short action { get; private set; }
		public short tag { get; private set; }
		public short right { get; private set; }
		public short left { get; private set; }
		public byte[] arguments { get; private set; }

		public bool oneSided {
			get {
				return (right == -1 || left == -1);
			}
		}

		/// <summary>
		/// Creates a new <c>Linedef</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public Linedef(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			start = BitConverter.ToInt16(data, 0);
			end = BitConverter.ToInt16(data, 2);
			flags = BitConverter.ToInt16(data, 4);
			action = -1;
			tag = -1;
			right = -1;
			left = -1;
			arguments = new byte[5];
			switch (type) {
				case MapType.Doom: {
					action = BitConverter.ToInt16(data, 6);
					tag = BitConverter.ToInt16(data, 8);
					right = BitConverter.ToInt16(data, 10);
					left = BitConverter.ToInt16(data, 12);
					break;
				}
				case MapType.Hexen: {
					action = data[6];
					arguments[0] = data[7];
					arguments[1] = data[8];
					arguments[2] = data[9];
					arguments[3] = data[10];
					arguments[4] = data[11];
					right = BitConverter.ToInt16(data, 12);
					left = BitConverter.ToInt16(data, 14);
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Linedef class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <c>Linedef</c> objects.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>List</c> of <c>Linedef</c> objects</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public static List<Linedef> LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Doom: {
					structLength = 14;
					break;
				}
				case MapType.Hexen: {
					structLength = 16;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Linedef lump factory.");
				}
			}
			List<Linedef> lump = new List<Linedef>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, i * structLength, bytes, 0, structLength);
				lump.Add(new Linedef(bytes, type));
			}
			return lump;
		}
	}
}
