#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Reflection;

namespace LibBSP {
#if UNITY
	using Plane = UnityEngine.Plane;
#elif GODOT
	using Plane = Godot.Plane;
#else
	using Plane = System.Numerics.Plane;
#endif

	/// <summary>
	/// Contains all data needed for a node in a BSP tree.
	/// </summary>
	public struct Node : ILumpObject {

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
		/// Gets the Plane used by this <see cref="Node"/>.
		/// </summary>
		public Plane Plane {
			get {
				return Parent.Bsp.planes[PlaneIndex];
			}
		}

		/// <summary>
		/// Gets or sets the index of the Plane used by this <see cref="Node"/>.
		/// </summary>
		public int PlaneIndex {
			get {
				return BitConverter.ToInt32(Data, 0);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(Data, 0);
			}
		}

		/// <summary>
		/// Gets the first child of this <see cref="Node"/>. If <see cref="Child1Index"/> is positive, the child will be
		/// another <see cref="Node"/>. Otherwise it's a <see cref="Leaf"/>.
		/// </summary>
		public ILumpObject Child1 {
			get {
				if (Child1Index >= 0) {
					return Parent.Bsp.nodes[Child1Index];
				}
				return Parent.Bsp.leaves[-Child1Index];
			}
		}

		/// <summary>
		/// Gets or sets the index of the first child of this <see cref="Node"/>, positive for other <see cref="Node"/>s, negative for <see cref="Leaf"/>s.
		/// </summary>
		public int Child1Index {
			get {
				switch (MapType) {
					case MapType.Quake: {
						return BitConverter.ToInt16(Data, 4);
					}
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.Nightfire:
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.TacticalInterventionEncrypted:
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(Data, 4);
					}
					default: {
						return 0;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake: {
						Data[4] = bytes[0];
						Data[5] = bytes[1];
						break;
					}
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.Nightfire:
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.TacticalInterventionEncrypted:
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(Data, 4);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets the second child of this <see cref="Node"/>. If <see cref="Child2Index"/> is positive, the child will be
		/// another <see cref="Node"/>. Otherwise it's a <see cref="Leaf"/>.
		/// </summary>
		public ILumpObject Child2 {
			get {
				if (Child2Index >= 0) {
					return Parent.Bsp.nodes[Child2Index];
				}
				return Parent.Bsp.leaves[-Child2Index];
			}
		}

		/// <summary>
		/// Gets or sets the index of the second child of this <see cref="Node"/>, positive for other <see cref="Node"/>s, negative for <see cref="Leaf"/>s.
		/// </summary>
		public int Child2Index {
			get {
				switch (MapType) {
					case MapType.Quake: {
						return BitConverter.ToInt16(Data, 6);
					}
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.Nightfire:
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.TacticalInterventionEncrypted:
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(Data, 8);
					}
					default: {
						return 0;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake: {
						Data[6] = bytes[0];
						Data[7] = bytes[1];
						break;
					}
					case MapType.SiN:
					case MapType.SoF:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.Nightfire:
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source22:
					case MapType.Source23:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.TacticalInterventionEncrypted:
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.Raven:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(Data, 8);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Node"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="parent">The <see cref="ILump"/> this <see cref="Node"/> came from.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Node(byte[] data, ILump parent = null) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Parent = parent;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Lump{Node}"/>.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <returns>A <see cref="Lump{Node}"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> parameter was <c>null</c>.</exception>
		public static Lump<Node> LumpFactory(byte[] data, BSP bsp, LumpInfo lumpInfo) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			return new Lump<Node>(data, GetStructLength(bsp.version, lumpInfo.version), bsp, lumpInfo);
		}

		/// <summary>
		/// Gets the length of this struct's data for the given <paramref name="mapType"/> and <paramref name="lumpVersion"/>.
		/// </summary>
		/// <param name="mapType">The <see cref="LibBSP.MapType"/> of the BSP.</param>
		/// <param name="lumpVersion">The version number for the lump.</param>
		/// <returns>The length, in <c>byte</c>s, of this struct.</returns>
		/// <exception cref="ArgumentException">This struct is not valid or is not implemented for the given <paramref name="mapType"/> and <paramref name="lumpVersion"/>.</exception>
		public static int GetStructLength(MapType mapType, int lumpVersion = 0) {
			switch (mapType) {
				case MapType.Quake: {
					return 24;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.SoF:
				case MapType.Daikatana: {
					return 28;
				}
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.L4D2:
				case MapType.TacticalInterventionEncrypted:
				case MapType.DMoMaM: {
					return 32;
				}
				case MapType.Vindictus: {
					return 48;
				}
				case MapType.Quake3:
				case MapType.FAKK:
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.MOHAA:
				case MapType.Raven:
				case MapType.Nightfire: {
					return 36;
				}
				default: {
					throw new ArgumentException("Lump object " + MethodBase.GetCurrentMethod().DeclaringType.Name + " does not exist in map type " + mapType + " or has not been implemented.");
				}
			}
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.Raven:
				case MapType.Quake3: {
					return 3;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 4;
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
					return 5;
				}
				case MapType.Nightfire: {
					return 8;
				}
				case MapType.FAKK:
				case MapType.MOHAA: {
					return 9;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 11;
				}
				case MapType.CoD: {
					return 20;
				}
				case MapType.CoD2: {
					return 25;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
