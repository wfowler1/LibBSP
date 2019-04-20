#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;

namespace LibBSP {
#if UNITY
	using Vector2d = UnityEngine.Vector2;
#elif GODOT
	using Vector2d = Godot.Vector2;
#endif

	/// <summary>
	/// Holds the data for a patch in a CoD BSP.
	/// </summary>
	public struct Patch {

		public byte[] data;
		public MapType type;
		public int version;

		public short shader {
			get {
				switch (type) {
					case MapType.CoD: {
						return BitConverter.ToInt16(data, 0);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD: {
						bytes.CopyTo(data, 0);
						break;
					}
				}
			}
		}
		
		public short patchType {
			get {
				switch (type) {
					case MapType.CoD: {
						return BitConverter.ToInt16(data, 2);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD: {
						bytes.CopyTo(data, 2);
						break;
					}
				}
			}
		}
		
		public Vector2d dimensions {
			get {
				switch (type) {
					case MapType.CoD: {
						if (patchType == 0) {
							return new Vector2d(BitConverter.ToInt16(data, 4), BitConverter.ToInt16(data, 6));
						} else {
							return new Vector2d(float.NaN, float.NaN);
						}
					}
					default: {
						return new Vector2d(float.NaN, float.NaN);
					}
				}
			}
			set {
				switch (type) {
					case MapType.CoD: {
						if (patchType == 0) {
							BitConverter.GetBytes((short)value.x).CopyTo(data, 4);
							BitConverter.GetBytes((short)value.y).CopyTo(data, 6);
						}
						break;
					}
				}
			}
		}
		
		public int flags {
			get {
				switch (type) {
					case MapType.CoD: {
						if (patchType == 0) {
							return BitConverter.ToInt32(data, 8);
						} else {
							return -1;
						}
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD: {
						if (patchType == 0) {
							bytes.CopyTo(data, 8);
						}
						break;
					}
				}
			}
		}
		
		[Index("patchVerts")] public int firstVertex {
			get {
				switch (type) {
					case MapType.CoD: {
						if (patchType == 0) {
							return BitConverter.ToInt32(data, 12);
						} else if (patchType == 1) {
							return BitConverter.ToInt32(data, 8);
						} else {
							return -1;
						}
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD: {
						if (patchType == 0) {
							bytes.CopyTo(data, 12);
						} else if (patchType == 1) {
							bytes.CopyTo(data, 8);
						}
						break;
					}
				}
			}
		}
		
		[Count("patchVerts")] public int numVertices {
			get {
				switch (type) {
					case MapType.CoD: {
						if (patchType == 0) {
							return BitConverter.ToInt16(data, 4) * BitConverter.ToInt16(data, 6);
						} else if (patchType == 1) {
							return BitConverter.ToInt16(data, 4);
						} else {
							return -1;
						}
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD: {
						if (patchType == 1) {
							data[4] = bytes[0];
							data[5] = bytes[1];
						}
						break;
					}
				}
			}
		}
		
		[Count("patchIndices")] public int numIndices {
			get {
				switch (type) {
					case MapType.CoD: {
						if (patchType == 1) {
							return BitConverter.ToInt16(data, 6);
						} else {
							return -1;
						}
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD: {
						if (patchType == 1) {
							data[6] = bytes[0];
							data[7] = bytes[1];
						}
						break;
					}
				}
			}
		}
		
		[Index("patchIndices")] public int firstIndex {
			get {
				switch (type) {
					case MapType.CoD: {
						if (patchType == 1) {
							return BitConverter.ToInt32(data, 12);
						} else {
							return -1;
						}
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
					case MapType.CoD: {
						if (patchType == 1) {
							bytes.CopyTo(data, 12);
						}
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Patch"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Patch(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="Patch"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="Patch"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<Patch> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.CoD: {
					structLength = 16;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Patch lump factory.");
				}
			}
			int numObjects = data.Length / structLength;
			List<Patch> lump = new List<Patch>(numObjects);
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Patch(bytes, type, version));
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
				case MapType.CoD: {
					return 24;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
