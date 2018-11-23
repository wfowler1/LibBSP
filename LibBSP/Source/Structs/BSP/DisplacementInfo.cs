#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if UNITY
	using Vector3d = Vector3;
#endif

	/// <summary>
	/// Holds all data for a Displacement from Source engine.
	/// </summary>
	public struct DisplacementInfo {

		public byte[] data;
		public MapType type;
		public int version;

		public Vector3d startPosition {
			get {
				return new Vector3d(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
			}
			set {
				value.GetBytes().CopyTo(data, 0);
			}
		}
		
		public int dispVertStart {
			get {
				return BitConverter.ToInt32(data, 12);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 12);
			}
		}

		public int dispTriStart {
			get {
				return BitConverter.ToInt32(data, 16);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 16);
			}
		}
		
		public int power {
			get {
				return BitConverter.ToInt32(data, 20);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 20);
			}
		}

		public int minTess {
			get {
				return BitConverter.ToInt32(data, 24);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 24);
			}
		}

		public float smoothingAngle {
			get {
				return BitConverter.ToSingle(data, 28);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 28);
			}
		}

		public int contents {
			get {
				return BitConverter.ToInt32(data, 32);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 32);
			}
		}

		public ushort mapFace {
			get {
				return BitConverter.ToUInt16(data, 36);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 36);
			}
		}

		public int lightmapAlphaStart {
			get {
				return BitConverter.ToInt32(data, 38);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 38);
			}
		}

		public int lightmapSamplePositionStart {
			get {
				return BitConverter.ToInt32(data, 42);
			}
			set {
				BitConverter.GetBytes(value).CopyTo(data, 42);
			}
		}

		public uint[] allowedVerts {
			get {
				uint[] allowedVerts = new uint[10];
				int offset = -1;
				switch (type) {
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.TacticalInterventionEncrypted:
					case MapType.DMoMaM: {
						offset = 136;
						break;
					}
					case MapType.Source22: {
						offset = 140;
						break;
					}
					case MapType.Source23: {
						offset = 144;
						break;
					}
					case MapType.Vindictus: {
						offset = 192;
						break;
					}
				}
				if (offset >= 0) {
					for (int i = 0; i < 10; ++i) {
						allowedVerts[i] = BitConverter.ToUInt32(data, offset + (i * 4));
					}
				}
				return allowedVerts;
			}
			set {
				if (value.Length != 10) {
					throw new ArgumentException("AllowedVerts array must have 10 elements.");
				}
				int offset = -1;
				switch (type) {
					case MapType.Source17:
					case MapType.Source18:
					case MapType.Source19:
					case MapType.Source20:
					case MapType.Source21:
					case MapType.Source27:
					case MapType.L4D2:
					case MapType.TacticalInterventionEncrypted:
					case MapType.DMoMaM: {
						offset = 136;
						break;
					}
					case MapType.Source22: {
						offset = 140;
						break;
					}
					case MapType.Source23: {
						offset = 144;
						break;
					}
					case MapType.Vindictus: {
						offset = 192;
						break;
					}
				}
				if (offset >= 0) {
					for (int i = 0; i < value.Length; ++i) {
						BitConverter.GetBytes(value[i]).CopyTo(data, offset + (i * 4));
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="DisplacementInfo"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public DisplacementInfo(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="DisplacementInfo"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="DisplacementInfo"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<DisplacementInfo> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source27:
				case MapType.L4D2:
				case MapType.TacticalInterventionEncrypted:
				case MapType.DMoMaM: {
					structLength = 176;
					break;
				}
				case MapType.Source22: {
					structLength = 180;
					break;
				}
				case MapType.Source23: {
					structLength = 184;
					break;
				}
				case MapType.Vindictus: {
					structLength = 232;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the SourceDispInfo lump factory.");
				}
			}
			int numObjects = data.Length / structLength;
			List<DisplacementInfo> lump = new List<DisplacementInfo>(numObjects);
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new DisplacementInfo(bytes, type, version));
			}
			return lump;
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
					return 26;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
