using System;
using System.Collections.Generic;

namespace LibBSP.Doom {
	/// <summary>
	/// This class holds data of a single Doom line Segment
	/// </summary>
	public struct Segment {

		public short startVertex { get; private set; }
		public short endVertex { get; private set; }
		public short angle { get; private set; }
		public short lineDef { get; private set; }
		// 0 for right side of linedef, 1 for left
		public short direction { get; private set; }
		public short dist { get; private set; }

		/// <summary>
		/// Creates a new <c>Segment</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public Segment(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.startVertex = BitConverter.ToInt16(data, 0);
			this.endVertex = BitConverter.ToInt16(data, 2);
			this.angle = BitConverter.ToInt16(data, 4);
			this.lineDef = BitConverter.ToInt16(data, 6);
			this.direction = BitConverter.ToInt16(data, 8);
			this.dist = BitConverter.ToInt16(data, 10);
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <c>Segment</c> objects.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>List</c> of <c>Segment</c> objects</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public static List<Segment> LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 12;
			List<Segment> lump = new List<Segment>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, i * structLength, bytes, 0, structLength);
				lump.Add(new Segment(bytes, type));
			}
			return lump;
		}

	}
}
