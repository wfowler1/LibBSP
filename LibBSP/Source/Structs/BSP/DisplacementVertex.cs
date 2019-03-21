#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;

namespace LibBSP {
#if UNITY
	using Vector3d = UnityEngine.Vector3;
#elif GODOT
	using Vector3d = Godot.Vector3;
#endif

	/// <summary>
	/// Holds all the data for a displacement in a Source map.
	/// </summary>
	public struct DisplacementVertex {

		public byte[] data;
		public MapType type;
		public int version;

		/// <summary>
		/// The normalized vector direction this vertex points from "flat".
		/// </summary>
		public Vector3d normal {
			get {
				return new Vector3d(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
			}
			set {
				value.GetBytes().CopyTo(data, 0);
			}
		}

		/// <summary>
		/// Magnitude of normal, before normalization.
		/// </summary>
		public float dist {
			get {
				return BitConverter.ToSingle(data, 12);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 12);
			}
		}

		/// <summary>
		/// Alpha value of texture at this vertex.
		/// </summary>
		public float alpha {
			get {
				return BitConverter.ToSingle(data, 16);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 16);
			}
		}

		/// <summary>
		/// Creates a new <see cref="DisplacementVertex"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public DisplacementVertex(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="DisplacementVertices"/> object.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <see cref="DisplacementVertices"/> object.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public static DisplacementVertices LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			return new DisplacementVertices(data, type, version);
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
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
					return 33;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
