#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Reflection;

namespace LibBSP {
#if UNITY
	using Vector3 = UnityEngine.Vector3;
#elif GODOT
	using Vector3 = Godot.Vector3;
#else
	using Vector3 = System.Numerics.Vector3;
#endif

	/// <summary>
	/// Holds all data for a Cubemap from Source engine.
	/// </summary>
	public struct Cubemap : ILumpObject {

		/// <summary>
		/// The <see cref="ILump"/> this <see cref="ILumpObject"/> came from.
		/// </summary>
		public ILump Parent { get; private set; }

		/// <summary>
		/// Array of <c>byte</c>s used as the data source for this <see cref="ILumpObject"/>.
		/// </summary>
		public byte[] Data { get; private set; }

		/// <summary>
		/// The <see cref="LibBSP.MapType"/> to use to interpret <see cref="Data"/>.
		/// </summary>
		public MapType MapType {
			get {
				if (Parent == null || Parent.Bsp == null) {
					return MapType.Undefined;
				}
				return Parent.Bsp.version;
			}
		}

		/// <summary>
		/// The version number of the <see cref="ILump"/> this <see cref="ILumpObject"/> came from.
		/// </summary>
		public int LumpVersion {
			get {
				if (Parent == null) {
					return 0;
				}
				return Parent.LumpInfo.version;
			}
		}

		/// <summary>
		/// Gets or sets the position of this <see cref="Cubemap"/>.
		/// </summary>
		public Vector3 Origin {
			get {
				if (MapType.IsSubtypeOf(MapType.Source)) {
					return new Vector3(BitConverter.ToInt32(Data, 0), BitConverter.ToInt32(Data, 4), BitConverter.ToInt32(Data, 8));
				}

				return new Vector3(0, 0, 0);
			}
			set {
				if (MapType.IsSubtypeOf(MapType.Source)) {
					BitConverter.GetBytes((int)value.X()).CopyTo(Data, 0);
					BitConverter.GetBytes((int)value.Y()).CopyTo(Data, 4);
					BitConverter.GetBytes((int)value.Z()).CopyTo(Data, 8);
				}
			}
		}

		/// <summary>
		/// Gets or sets the size of this <see cref="Cubemap"/>.
		/// </summary>
		public int Size {
			get {
				if (MapType.IsSubtypeOf(MapType.Source)) {
					return BitConverter.ToInt32(Data, 12);
				}

				return -1;
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				if (MapType.IsSubtypeOf(MapType.Source)) {
					bytes.CopyTo(Data, 12);
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Cubemap"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="parent">The <see cref="ILump"/> this <see cref="Cubemap"/> came from.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Cubemap(byte[] data, ILump parent = null) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Parent = parent;
		}

		/// <summary>
		/// Creates a new <see cref="Cubemap"/> by copying the fields in <paramref name="source"/>, using
		/// <paramref name="parent"/> to get <see cref="LibBSP.MapType"/> and <see cref="LumpInfo.version"/>
		/// to use when creating the new <see cref="Cubemap"/>.
		/// If the <paramref name="parent"/>'s <see cref="BSP"/>'s <see cref="LibBSP.MapType"/> is different from
		/// the one from <paramref name="source"/>, it does not matter, because fields are copied by name.
		/// </summary>
		/// <param name="source">The <see cref="Cubemap"/> to copy.</param>
		/// <param name="parent">
		/// The <see cref="ILump"/> to use as the <see cref="Parent"/> of the new <see cref="Cubemap"/>.
		/// Use <c>null</c> to use the <paramref name="source"/>'s <see cref="Parent"/> instead.
		/// </param>
		public Cubemap(Cubemap source, ILump parent) {
			Parent = parent;

			if (parent != null && parent.Bsp != null) {
				if (source.Parent != null && source.Parent.Bsp != null && source.Parent.Bsp.version == parent.Bsp.version && source.LumpVersion == parent.LumpInfo.version) {
					Data = new byte[source.Data.Length];
					Array.Copy(source.Data, Data, source.Data.Length);
					return;
				} else {
					Data = new byte[GetStructLength(parent.Bsp.version, parent.LumpInfo.version)];
				}
			} else {
				if (source.Parent != null && source.Parent.Bsp != null) {
					Data = new byte[GetStructLength(source.Parent.Bsp.version, source.Parent.LumpInfo.version)];
				} else {
					Data = new byte[GetStructLength(MapType.Undefined, 0)];
				}
			}

			Origin = source.Origin;
			Size = source.Size;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Lump{Cubemap}"/>.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <returns>A <see cref="Lump{Cubemap}"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> parameter was <c>null</c>.</exception>
		public static Lump<Cubemap> LumpFactory(byte[] data, BSP bsp, LumpInfo lumpInfo) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			return new Lump<Cubemap>(data, GetStructLength(bsp.version, lumpInfo.version), bsp, lumpInfo);
		}

		/// <summary>
		/// Gets the length of this struct's data for the given <paramref name="mapType"/> and <paramref name="lumpVersion"/>.
		/// </summary>
		/// <param name="mapType">The <see cref="LibBSP.MapType"/> of the BSP.</param>
		/// <param name="lumpVersion">The version number for the lump.</param>
		/// <returns>The length, in <c>byte</c>s, of this struct.</returns>
		/// <exception cref="ArgumentException">This struct is not valid or is not implemented for the given <paramref name="mapType"/> and <paramref name="lumpVersion"/>.</exception>
		public static int GetStructLength(MapType mapType, int lumpVersion = 0) {
			if (mapType.IsSubtypeOf(MapType.Source)) {
				return 16;
			}

			throw new ArgumentException("Lump object " + MethodBase.GetCurrentMethod().DeclaringType.Name + " does not exist in map type " + mapType + " or has not been implemented.");
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForLump(MapType type) {
			if (type.IsSubtypeOf(MapType.Source)) {
				return 42;
			}

			return -1;
		}

	}
}
