using System;
using System.Collections.Generic;

namespace LibBSP.Doom {
	/// <summary>
	/// Contains all necessary information for a Doom SECTOR object.
	/// </summary>
	/// <remarks>
	/// The sector defines an area, the heights of the floor and cieling
	/// of the area, the floor and cieling textures, the light level, the
	/// type of sector and a tag number.
	/// </remarks>
	public struct Sector {

		public short floor { get; private set; }
		public short cieling { get; private set; }

		public string floorTexture { get; private set; }
		public string cielingTexture { get; private set; }

		public short light { get; private set; }
		public short type { get; private set; }
		public short tag { get; private set; }

		/// <summary>
		/// Creates a new <c>Sector</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public Sector(byte[] data, MapType type) : this() {
			this.floor = BitConverter.ToInt16(data, 0);
			this.cieling = BitConverter.ToInt16(data, 2);
			this.floorTexture = data.ToNullTerminatedString(4, 8);
			this.cielingTexture = data.ToNullTerminatedString(12, 8);
			this.light = BitConverter.ToInt16(data, 20);
			this.type = BitConverter.ToInt16(data, 22);
			this.tag = BitConverter.ToInt16(data, 24);
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <c>Sector</c> objects.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>List</c> of <c>Sector</c> objects</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public static List<Sector> LumpFactory(byte[] data, MapType type) {
			int structLength = 26;
			List<Sector> lump = new List<Sector>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, i * structLength, bytes, 0, structLength);
				lump.Add(new Sector(bytes, type));
			}
			return lump;
		}
	}
}
