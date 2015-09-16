#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP.Doom {
#if !UNITY
	using Vector2 = Vector2d;
#endif

	/// <summary>
	/// Contains all necessary information for a Doom SIDEDEF object.
	/// </summary>
	/// <remarks>
	/// The sidedef is roughly equivalent to the Face (or surface)
	/// object in later BSP versions.
	/// </remarks>
	public struct Sidedef {

		public Vector2 offsets { get; private set; }
		public string highTexture { get; private set; }
		public string midTexture { get; private set; }
		public string lowTexture { get; private set; }
		public short sector { get; private set; }

		/// <summary>
		/// Creates a new <c>Sidedef</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public Sidedef(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			offsets = new Vector2(BitConverter.ToInt16(data, 0), BitConverter.ToInt16(data, 2));
			highTexture = data.ToNullTerminatedString(4, 8);
			midTexture = data.ToNullTerminatedString(12, 8);
			lowTexture = data.ToNullTerminatedString(20, 8);
			sector = BitConverter.ToInt16(data, 28);
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <c>Sidedef</c> objects.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>List</c> of <c>Sidedef</c> objects</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public static List<Sidedef> LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 30;
			List<Sidedef> lump = new List<Sidedef>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, i * structLength, bytes, 0, structLength);
				lump.Add(new Sidedef(bytes, type));
			}
			return lump;
		}

	}
}
