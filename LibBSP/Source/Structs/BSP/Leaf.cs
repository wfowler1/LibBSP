#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
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
	/// Holds data for a leaf structure in a BSP map.
	/// </summary>
	public struct Leaf : ILumpObject {

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
		/// Gets or sets the contents flags for this <see cref="Leaf"/>.
		/// </summary>
		public int Contents {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift:
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.SiN:
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
					case MapType.DMoMaM:
					case MapType.Daikatana:
					case MapType.Vindictus:
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 0);
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
					case MapType.BlueShift:
					case MapType.Quake2:
					case MapType.SoF:
					case MapType.SiN:
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
					case MapType.DMoMaM:
					case MapType.Daikatana:
					case MapType.Vindictus:
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 0);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the offset or cluster in the visibility data used by this <see cref="Leaf"/>.
		/// </summary>
		public int Visibility {
			get {
				switch (MapType) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						return BitConverter.ToInt32(Data, 0);
					}
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift:
					case MapType.Nightfire:
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 4);
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
						return BitConverter.ToInt16(Data, 4);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.CoD4: {
						bytes.CopyTo(Data, 0);
						break;
					}
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift:
					case MapType.Nightfire:
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 4);
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
						Data[4] = bytes[0];
						Data[5] = bytes[1];
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the area of this <see cref="Leaf"/> for the AreaPortals system.
		/// </summary>
		public int Area {
			get {
				switch (MapType) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(Data, 4);
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF: {
						return BitConverter.ToInt16(Data, 6);
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
						return Data[6];
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(Data, 4);
						break;
					}
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
					case MapType.SoF: {
						Data[6] = bytes[0];
						Data[7] = bytes[1];
						break;
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
						Data[6] = bytes[0];
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the flags on this <see cref="Leaf"/>.
		/// </summary>
		public int Flags {
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
						return Data[7];
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 8);
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
						Data[7] = bytes[0];
						break;
					}
					case MapType.Vindictus: {
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
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2: {
						return new Vector3(BitConverter.ToInt32(Data, 8), BitConverter.ToInt32(Data, 12), BitConverter.ToInt32(Data, 16));
					}
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
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
						return new Vector3(BitConverter.ToInt16(Data, 8), BitConverter.ToInt16(Data, 10), BitConverter.ToInt16(Data, 12));
					}
					case MapType.SoF: {
						return new Vector3(BitConverter.ToInt16(Data, 10), BitConverter.ToInt16(Data, 12), BitConverter.ToInt16(Data, 14));
					}
					case MapType.Vindictus: {
						return new Vector3(BitConverter.ToInt32(Data, 12), BitConverter.ToInt32(Data, 16), BitConverter.ToInt32(Data, 20));
					}
					case MapType.Nightfire: {
						return Vector3Extensions.ToVector3(Data, 8);
					}
					default: {
						return new Vector3(0, 0, 0);
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2: {
						BitConverter.GetBytes((int)value.X()).CopyTo(Data, 8);
						BitConverter.GetBytes((int)value.Y()).CopyTo(Data, 12);
						BitConverter.GetBytes((int)value.Z()).CopyTo(Data, 16);
						break;
					}
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
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
						BitConverter.GetBytes((short)value.X()).CopyTo(Data, 8);
						BitConverter.GetBytes((short)value.Y()).CopyTo(Data, 10);
						BitConverter.GetBytes((short)value.Z()).CopyTo(Data, 12);
						break;
					}
					case MapType.SoF: {
						BitConverter.GetBytes((short)value.X()).CopyTo(Data, 10);
						BitConverter.GetBytes((short)value.Y()).CopyTo(Data, 12);
						BitConverter.GetBytes((short)value.Z()).CopyTo(Data, 14);
						break;
					}
					case MapType.Vindictus: {
						BitConverter.GetBytes((int)value.X()).CopyTo(Data, 12);
						BitConverter.GetBytes((int)value.Y()).CopyTo(Data, 16);
						BitConverter.GetBytes((int)value.Z()).CopyTo(Data, 20);
						break;
					}
					case MapType.Nightfire: {
						BitConverter.GetBytes(value.X()).CopyTo(Data, 8);
						BitConverter.GetBytes(value.Y()).CopyTo(Data, 12);
						BitConverter.GetBytes(value.Z()).CopyTo(Data, 16);
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
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2: {
						return new Vector3(BitConverter.ToInt32(Data, 20), BitConverter.ToInt32(Data, 24), BitConverter.ToInt32(Data, 28));
					}
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
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
						return new Vector3(BitConverter.ToInt16(Data, 14), BitConverter.ToInt16(Data, 16), BitConverter.ToInt16(Data, 18));
					}
					case MapType.SoF: {
						return new Vector3(BitConverter.ToInt16(Data, 16), BitConverter.ToInt16(Data, 18), BitConverter.ToInt16(Data, 20));
					}
					case MapType.Vindictus: {
						return new Vector3(BitConverter.ToInt32(Data, 24), BitConverter.ToInt32(Data, 28), BitConverter.ToInt32(Data, 32));
					}
					case MapType.Nightfire: {
						return Vector3Extensions.ToVector3(Data, 20);
					}
					default: {
						return new Vector3(0, 0, 0);
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.Quake3:
					case MapType.Raven:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2: {
						BitConverter.GetBytes((short)value.X()).CopyTo(Data, 20);
						BitConverter.GetBytes((short)value.Y()).CopyTo(Data, 24);
						BitConverter.GetBytes((short)value.Z()).CopyTo(Data, 28);
						break;
					}
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift:
					case MapType.Quake2:
					case MapType.Daikatana:
					case MapType.SiN:
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
						BitConverter.GetBytes((short)value.X()).CopyTo(Data, 14);
						BitConverter.GetBytes((short)value.Y()).CopyTo(Data, 16);
						BitConverter.GetBytes((short)value.Z()).CopyTo(Data, 18);
						break;
					}
					case MapType.SoF: {
						BitConverter.GetBytes((short)value.X()).CopyTo(Data, 16);
						BitConverter.GetBytes((short)value.Y()).CopyTo(Data, 18);
						BitConverter.GetBytes((short)value.Z()).CopyTo(Data, 20);
						break;
					}
					case MapType.Vindictus: {
						BitConverter.GetBytes((int)value.X()).CopyTo(Data, 24);
						BitConverter.GetBytes((int)value.Y()).CopyTo(Data, 28);
						BitConverter.GetBytes((int)value.Z()).CopyTo(Data, 32);
						break;
					}
					case MapType.Nightfire: {
						BitConverter.GetBytes(value.X()).CopyTo(Data, 20);
						BitConverter.GetBytes(value.Y()).CopyTo(Data, 24);
						BitConverter.GetBytes(value.Z()).CopyTo(Data, 28);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Enumerates the brush indices used by this <see cref="Leaf"/>.
		/// </summary>
		public IEnumerable<int> MarkBrushes {
			get {
				for (int i = 0; i < NumMarkBrushIndices; ++i) {
					yield return (int)Parent.Bsp.markBrushes[FirstMarkBrushIndex + i];
				}
			}
		}

		/// <summary>
		/// Gets or sets the index of the first mark brush reference for this <see cref="Leaf"/>.
		/// </summary>
		[Index("markBrushes")] public int FirstMarkBrushIndex {
			get {
				switch (MapType) {
					case MapType.CoD4: {
						return BitConverter.ToInt32(Data, 12);
					}
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(Data, 16);
					}
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.DMoMaM:
					case MapType.Daikatana: {
						return BitConverter.ToUInt16(Data, 24);
					}
					case MapType.SoF: {
						return BitConverter.ToUInt16(Data, 26);
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 40);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 44);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD4: {
						bytes.CopyTo(Data, 12);
						break;
					}
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(Data, 16);
						break;
					}
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.DMoMaM:
					case MapType.Daikatana: {
						Data[24] = bytes[0];
						Data[25] = bytes[1];
						break;
					}
					case MapType.SoF: {
						Data[26] = bytes[0];
						Data[27] = bytes[1];
						break;
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 40);
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 44);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the count of mark brush references for this <see cref="Leaf"/>.
		/// </summary>
		[Count("markBrushes")] public int NumMarkBrushIndices {
			get {
				switch (MapType) {
					case MapType.CoD4: {
						return BitConverter.ToInt32(Data, 16);
					}
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(Data, 20);
					}
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.DMoMaM:
					case MapType.Daikatana: {
						return BitConverter.ToUInt16(Data, 26);
					}
					case MapType.SoF: {
						return BitConverter.ToUInt16(Data, 28);
					}
					case MapType.Nightfire:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven: {
						return BitConverter.ToInt32(Data, 44);
					}
					case MapType.Vindictus: {
						return BitConverter.ToInt32(Data, 48);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD4: {
						bytes.CopyTo(Data, 16);
						break;
					}
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(Data, 20);
						break;
					}
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.DMoMaM:
					case MapType.Daikatana: {
						Data[26] = bytes[0];
						Data[27] = bytes[1];
						break;
					}
					case MapType.SoF: {
						Data[28] = bytes[0];
						Data[29] = bytes[1];
						break;
					}
					case MapType.Nightfire:
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven: {
						bytes.CopyTo(Data, 44);
						break;
					}
					case MapType.Vindictus: {
						bytes.CopyTo(Data, 48);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Enumerates the <see cref="Face"/> indices used by this <see cref="Leaf"/>.
		/// </summary>
		public IEnumerable<int> MarkFaces {
			get {
				for (int i = 0; i < NumMarkFaceIndices; ++i) {
					yield return (int)Parent.Bsp.markSurfaces[FirstMarkFaceIndex + i];
				}
			}
		}

		/// <summary>
		/// Gets or sets the index of the first mark face reference for this <see cref="Leaf"/>.
		/// </summary>
		[Index("markSurfaces")] public int FirstMarkFaceIndex {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift:
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.DMoMaM:
					case MapType.Daikatana: {
						return BitConverter.ToUInt16(Data, 20);
					}
					case MapType.SoF: {
						return BitConverter.ToUInt16(Data, 22);
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 32);
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
					case MapType.BlueShift:
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.DMoMaM:
					case MapType.Daikatana: {
						Data[20] = bytes[0];
						Data[21] = bytes[1];
						break;
					}
					case MapType.SoF: {
						Data[22] = bytes[0];
						Data[23] = bytes[1];
						break;
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 32);
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
		/// Gets or sets the count of mark face references for this <see cref="Leaf"/>.
		/// </summary>
		[Count("markSurfaces")] public int NumMarkFaceIndices {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift:
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.DMoMaM:
					case MapType.Daikatana: {
						return BitConverter.ToUInt16(Data, 22);
					}
					case MapType.SoF: {
						return BitConverter.ToUInt16(Data, 24);
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						return BitConverter.ToInt32(Data, 36);
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
					case MapType.BlueShift:
					case MapType.Quake2:
					case MapType.SiN:
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
					case MapType.DMoMaM:
					case MapType.Daikatana: {
						Data[22] = bytes[0];
						Data[23] = bytes[1];
						break;
					}
					case MapType.SoF: {
						Data[24] = bytes[0];
						Data[25] = bytes[1];
						break;
					}
					case MapType.Quake3:
					case MapType.FAKK:
					case MapType.STEF2Demo:
					case MapType.STEF2:
					case MapType.MOHAA:
					case MapType.Raven:
					case MapType.Nightfire: {
						bytes.CopyTo(Data, 36);
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
		/// Gets or sets ambient water sound level for this <see cref="Leaf"/>.
		/// </summary>
		public byte WaterSoundLevel {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						return Data[24];
					}
					default: {
						return 0;
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						Data[24] = value;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets ambient sky sound level for this <see cref="Leaf"/>.
		/// </summary>
		public byte SkySoundLevel {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						return Data[25];
					}
					default: {
						return 0;
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						Data[25] = value;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets ambient slime sound level for this <see cref="Leaf"/>.
		/// </summary>
		public byte SlimeSoundLevel {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						return Data[26];
					}
					default: {
						return 0;
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						Data[26] = value;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets ambient lava sound level for this <see cref="Leaf"/>.
		/// </summary>
		public byte LavaSoundLevel {
			get {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						return Data[27];
					}
					default: {
						return 0;
					}
				}
			}
			set {
				switch (MapType) {
					case MapType.Quake:
					case MapType.GoldSrc:
					case MapType.BlueShift: {
						Data[27] = value;
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// Enumerates the <see cref="StaticModel"/> indices used by this <see cref="Leaf"/>.
		/// </summary>
		public IEnumerable<int> LeafStaticModels {
			get {
				for (int i = 0; i < NumLeafStaticModelIndices; ++i) {
					yield return (int)Parent.Bsp.leafStaticModels[FirstLeafStaticModelIndex + i];
				}
			}
		}

		/// <summary>
		/// Gets or sets the index of the first leaf model reference for this <see cref="Leaf"/>.
		/// </summary>
		[Index("leafModels")] public int FirstLeafStaticModelIndex {
			get {
				switch (MapType) {
					case MapType.MOHAA: {
						return BitConverter.ToInt32(Data, 56);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.MOHAA: {
						bytes.CopyTo(Data, 56);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the count of leaf model references for this <see cref="Leaf"/>.
		/// </summary>
		[Count("leafModels")] public int NumLeafStaticModelIndices {
			get {
				switch (MapType) {
					case MapType.MOHAA: {
						return BitConverter.ToInt32(Data, 60);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.MOHAA: {
						bytes.CopyTo(Data, 60);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Enumerates the patch indices used by this <see cref="Leaf"/>.
		/// </summary>
		public IEnumerable<int> PatchIndices {
			get {
				for (int i = 0; i < NumPatchIndices; ++i) {
					yield return (int)Parent.Bsp.leafPatches[FirstPatchIndicesIndex + i];
				}
			}
		}

		/// <summary>
		/// Gets or sets the index of the first patch index for this <see cref="Leaf"/>.
		/// </summary>
		[Index("patchIndices")] public int FirstPatchIndicesIndex {
			get {
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(Data, 8);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(Data, 8);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the count of patch indices referenced by this <see cref="Leaf"/>.
		/// </summary>
		[Count("patchIndices")] public int NumPatchIndices {
			get {
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2: {
						return BitConverter.ToInt32(Data, 12);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (MapType) {
					case MapType.CoD:
					case MapType.CoD2: {
						bytes.CopyTo(Data, 12);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="Leaf"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="parent">The <see cref="ILump"/> this <see cref="Leaf"/> came from.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Leaf(byte[] data, ILump parent = null) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Parent = parent;
		}

		/// <summary>
		/// Creates a new <see cref="Leaf"/> by copying the fields in <paramref name="source"/>, using
		/// <paramref name="parent"/> to get <see cref="LibBSP.MapType"/> and <see cref="LumpInfo.version"/>
		/// to use when creating the new <see cref="Leaf"/>.
		/// If the <paramref name="parent"/>'s <see cref="BSP"/>'s <see cref="LibBSP.MapType"/> is different from
		/// the one from <paramref name="source"/>, it does not matter, because fields are copied by name.
		/// </summary>
		/// <param name="source">The <see cref="Leaf"/> to copy.</param>
		/// <param name="parent">
		/// The <see cref="ILump"/> to use as the <see cref="Parent"/> of the new <see cref="Leaf"/>.
		/// Use <c>null</c> to use the <paramref name="source"/>'s <see cref="Parent"/> instead.
		/// </param>
		public Leaf(Leaf source, ILump parent) {
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

			Contents = source.Contents;
			Visibility = source.Visibility;
			Area = source.Area;
			Flags = source.Flags;
			Minimums = source.Minimums;
			Maximums = source.Maximums;
			FirstMarkBrushIndex = source.FirstMarkBrushIndex;
			NumMarkBrushIndices = source.NumMarkBrushIndices;
			FirstMarkFaceIndex = source.FirstMarkFaceIndex;
			NumMarkFaceIndices = source.NumMarkFaceIndices;
			WaterSoundLevel = source.WaterSoundLevel;
			SkySoundLevel = source.SkySoundLevel;
			SlimeSoundLevel = source.SlimeSoundLevel;
			LavaSoundLevel = source.LavaSoundLevel;
			FirstLeafStaticModelIndex = source.FirstLeafStaticModelIndex;
			NumLeafStaticModelIndices = source.NumLeafStaticModelIndices;
			FirstPatchIndicesIndex = source.FirstPatchIndicesIndex;
			NumPatchIndices = source.NumPatchIndices;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="Lump{Leaf}"/>.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <returns>A <see cref="Lump{Leaf}"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> parameter was <c>null</c>.</exception>
		public static Lump<Leaf> LumpFactory(byte[] data, BSP bsp, LumpInfo lumpInfo) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			return new Lump<Leaf>(data, GetStructLength(bsp.version, lumpInfo.version), bsp, lumpInfo);
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
				case MapType.CoD4: {
					return 24;
				}
				case MapType.Quake:
				case MapType.GoldSrc:
				case MapType.BlueShift:
				case MapType.Quake2:
				case MapType.SiN: {
					return 28;
				}
				case MapType.Source17:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.L4D2:
				case MapType.TacticalInterventionEncrypted:
				case MapType.SoF:
				case MapType.Daikatana:
				case MapType.DMoMaM: {
					return 32;
				}
				case MapType.CoD:
				case MapType.CoD2: {
					return 36;
				}
				case MapType.Nightfire:
				case MapType.Quake3:
				case MapType.FAKK:
				case MapType.STEF2Demo:
				case MapType.STEF2:
				case MapType.Raven: {
					return 48;
				}
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Vindictus: {
					return 56;
				}
				case MapType.MOHAA: {
					return 64;
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
					return 4;
				}
				case MapType.MOHAA:
				case MapType.FAKK:
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 8;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo:
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
					return 10;
				}
				case MapType.Nightfire: {
					return 11;
				}
				case MapType.CoD: {
					return 21;
				}
				case MapType.CoD2: {
					return 26;
				}
				case MapType.CoD4: {
					return 28;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
