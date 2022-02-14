#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Reflection;

namespace LibBSP {
#if UNITY
	using Plane = UnityEngine.Plane;
	using Vector3 = UnityEngine.Vector3;
#elif GODOT
	using Plane = Godot.Plane;
	using Vector3 = Godot.Vector3;
#else
	using Plane = System.Numerics.Plane;
	using Vector3 = System.Numerics.Vector3;
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
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
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
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
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
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
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
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
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
		/// Gets or sets the bounding box minimums for this leaf.
		/// </summary>
		public Vector3 Minimums {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						return new Vector3(BitConverter.ToInt16(Data, 8), BitConverter.ToInt16(Data, 10), BitConverter.ToInt16(Data, 12));
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
						return new Vector3(BitConverter.ToInt16(Data, 12), BitConverter.ToInt16(Data, 14), BitConverter.ToInt16(Data, 16));
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.Raven:
					case MapType.Vindictus: {
						return new Vector3(BitConverter.ToInt32(Data, 12), BitConverter.ToInt32(Data, 16), BitConverter.ToInt32(Data, 20));
					}
					case MapType.Nightfire: {
						return Vector3Extensions.ToVector3(Data, 12);
					}
					default: {
						return new Vector3(0, 0, 0);
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						BitConverter.GetBytes((short)value.X()).CopyTo(Data, 8);
						BitConverter.GetBytes((short)value.Y()).CopyTo(Data, 10);
						BitConverter.GetBytes((short)value.Z()).CopyTo(Data, 12);
						break;
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
						BitConverter.GetBytes((short)value.X()).CopyTo(Data, 12);
						BitConverter.GetBytes((short)value.Y()).CopyTo(Data, 14);
						BitConverter.GetBytes((short)value.Z()).CopyTo(Data, 16);
						break;
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.Raven:
					case MapType.Vindictus: {
						BitConverter.GetBytes((int)value.X()).CopyTo(Data, 12);
						BitConverter.GetBytes((int)value.Y()).CopyTo(Data, 16);
						BitConverter.GetBytes((int)value.Z()).CopyTo(Data, 20);
						break;
					}
					case MapType.Nightfire: {
						BitConverter.GetBytes(value.X()).CopyTo(Data, 12);
						BitConverter.GetBytes(value.Y()).CopyTo(Data, 16);
						BitConverter.GetBytes(value.Z()).CopyTo(Data, 20);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the bounding box maximums for this leaf.
		/// </summary>
		public Vector3 Maximums {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						return new Vector3(BitConverter.ToInt16(Data, 14), BitConverter.ToInt16(Data, 16), BitConverter.ToInt16(Data, 18));
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
						return new Vector3(BitConverter.ToInt16(Data, 18), BitConverter.ToInt16(Data, 20), BitConverter.ToInt16(Data, 22));
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.Raven:
					case MapType.Vindictus: {
						return new Vector3(BitConverter.ToInt32(Data, 24), BitConverter.ToInt32(Data, 28), BitConverter.ToInt32(Data, 32));
					}
					case MapType.Nightfire: {
						return Vector3Extensions.ToVector3(Data, 24);
					}
					default: {
						return new Vector3(0, 0, 0);
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						BitConverter.GetBytes((short)value.X()).CopyTo(Data, 14);
						BitConverter.GetBytes((short)value.Y()).CopyTo(Data, 16);
						BitConverter.GetBytes((short)value.Z()).CopyTo(Data, 18);
						break;
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
						BitConverter.GetBytes((short)value.X()).CopyTo(Data, 18);
						BitConverter.GetBytes((short)value.Y()).CopyTo(Data, 20);
						BitConverter.GetBytes((short)value.Z()).CopyTo(Data, 22);
						break;
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4:
					case MapType.MOHAA:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.Raven:
					case MapType.Vindictus: {
						BitConverter.GetBytes((short)value.X()).CopyTo(Data, 24);
						BitConverter.GetBytes((short)value.Y()).CopyTo(Data, 28);
						BitConverter.GetBytes((short)value.Z()).CopyTo(Data, 32);
						break;
					}
					case MapType.Nightfire: {
						BitConverter.GetBytes(value.X()).CopyTo(Data, 24);
						BitConverter.GetBytes(value.Y()).CopyTo(Data, 28);
						BitConverter.GetBytes(value.Z()).CopyTo(Data, 32);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Enumerates the <see cref="Face"/> indices used by this <see cref="Node"/>.
		/// </summary>
		public IEnumerable<Face> Faces {
			get {
				for (int i = 0; i < NumFaceIndices; ++i) {
					yield return Parent.Bsp.faces[FirstFaceIndex + i];
				}
			}
		}

		/// <summary>
		/// Gets or sets the index of the first face reference for this <see cref="Node"/>.
		/// </summary>
		[Index("faces")]
		public int FirstFaceIndex {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						return BitConverter.ToUInt16(Data, 20);
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
						return BitConverter.ToUInt16(Data, 24);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 36);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						Data[20] = bytes[0];
						Data[21] = bytes[1];
						break;
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF:
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
						Data[24] = bytes[0];
						Data[25] = bytes[1];
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 36);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the count of face references for this <see cref="Node"/>.
		/// </summary>
		[Count("faces")]
		public int NumFaceIndices {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						return BitConverter.ToUInt16(Data, 22);
					}
					case MapType.Quake2:
					case MapType.SiN:
					case MapType.SoF:
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
						return BitConverter.ToUInt16(Data, 26);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 40);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						Data[22] = bytes[0];
						Data[23] = bytes[1];
						break;
					}
					case MapType.Quake2:
					case MapType.SiN:
					case MapType.SoF:
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
						Data[26] = bytes[0];
						Data[27] = bytes[1];
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 40);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the area reference for this <see cref="Node"/>.
		/// </summary>
		public int AreaIndex {
			get {
				switch (MapType) {
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
						return BitConverter.ToUInt16(Data, 28);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
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
						Data[28] = bytes[0];
						Data[29] = bytes[1];
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
		/// Creates a new <see cref="Node"/> by copying the fields in <paramref name="source"/>, using
		/// <paramref name="parent"/> to get <see cref="LibBSP.MapType"/> and <see cref="LumpInfo.version"/>
		/// to use when creating the new <see cref="Node"/>.
		/// If the <paramref name="parent"/>'s <see cref="BSP"/>'s <see cref="LibBSP.MapType"/> is different from
		/// the one from <paramref name="source"/>, it does not matter, because fields are copied by name.
		/// </summary>
		/// <param name="source">The <see cref="Node"/> to copy.</param>
		/// <param name="parent">
		/// The <see cref="ILump"/> to use as the <see cref="Parent"/> of the new <see cref="Node"/>.
		/// Use <c>null</c> to use the <paramref name="source"/>'s <see cref="Parent"/> instead.
		/// </param>
		public Node(Node source, ILump parent) {
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

			PlaneIndex = source.PlaneIndex;
			Child1Index = source.Child1Index;
			Child2Index = source.Child2Index;
			Minimums = source.Minimums;
			Maximums = source.Maximums;
			FirstFaceIndex = source.FirstFaceIndex;
			NumFaceIndices = source.NumFaceIndices;
			AreaIndex = source.AreaIndex;
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
				case MapType.Quake:
				case MapType.GoldSrc:
				case MapType.BlueShift: {
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
				case MapType.GoldSrc:
				case MapType.BlueShift:
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
