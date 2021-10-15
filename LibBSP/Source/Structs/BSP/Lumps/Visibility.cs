using System;

namespace LibBSP {

	/// <summary>
	/// Holds the visibility data for a BSP.
	/// </summary>
	public class Visibility : ILump {
		
		/// <summary>
		/// The <see cref="BSP"/> this <see cref="ILump"/> came from.
		/// </summary>
		public BSP Bsp { get; protected set; }

		/// <summary>
		/// The <see cref="LibBSP.LumpInfo"/> associated with this <see cref="ILump"/>.
		/// </summary>
		public LumpInfo LumpInfo { get; protected set; }

		/// <summary>
		/// Array of <c>byte</c>s used as the data source for visibility info.
		/// </summary>
		public byte[] Data { get; protected set; }

		/// <summary>
		/// Gets or sets the number of visibility clusters in this <see cref="Visibility"/> lump.
		/// </summary>
		public int NumClusters {
			get {
				switch (Bsp.version) {
					case MapType.Quake2:
					case MapType.SiN:
					case MapType.Daikatana:
					case MapType.SoF:
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
					case MapType.DMoMaM:
					case MapType.Quake3:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.Raven: {
						return BitConverter.ToInt32(Data, 0);
					}
				}

				return -1;
			}
			set {
				switch (Bsp.version) {
					case MapType.Quake2:
					case MapType.SiN:
					case MapType.Daikatana:
					case MapType.SoF:
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
					case MapType.DMoMaM:
					case MapType.Quake3:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.Raven: {
						BitConverter.GetBytes(value).CopyTo(Data, 0);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the size in bytes of the visibility data for this map's <see cref="Leaf"/> clusters.
		/// </summary>
		public int ClusterSize {
			get {
				switch (Bsp.version) {
					case MapType.Quake3:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.Raven: {
						return BitConverter.ToInt32(Data, 4);
					}
				}

				return -1;
			}
			set {
				switch (Bsp.version) {
					case MapType.Quake3:
					case MapType.STEF2:
					case MapType.STEF2Demo:
					case MapType.MOHAA:
					case MapType.FAKK:
					case MapType.CoD:
					case MapType.CoD2:
					case MapType.Raven: {
						BitConverter.GetBytes(value).CopyTo(Data, 4);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Parses the passed <c>byte</c> array into a <see cref="Visibility"/> object.
		/// </summary>
		/// <param name="data">Array of <c>byte</c>s to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Visibility(byte[] data, BSP bsp, LumpInfo lumpInfo = default(LumpInfo)) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Bsp = bsp;
			LumpInfo = lumpInfo;
		}

		/// <summary>
		/// Can <see cref=Leaf"/> <paramref name="leaf"/> see the <see cref="Leaf"/> or cluster at index <paramref name="other"/>.
		/// </summary>
		/// <param name="leaf">The <see cref="Leaf"/>.</param>
		/// <param name="other">The index of the other <see cref="Leaf"/> or cluster to determine visibility for.</param>
		/// <returns>Whether <paramref name="leaf"/> can see the <see cref="Leaf"/> or cluster at index <paramref name="other"/>.</returns>
		public bool CanSee(Leaf leaf, int other) {
			int offset = GetOffsetForClusterPVS(leaf.Visibility);
			if (offset < 0) {
				offset = leaf.Visibility;
			}

			for (int i = 1; i < leaf.Parent.Bsp.leaves.Count; offset++) {
				if (Data[offset] == 0 && Bsp.version != MapType.Nightfire) {
					i += 8 * Data[offset + 1];
					if (i > other) {
						return false;
					}
					offset++;
				} else {
					for (int bit = 1; bit != 0; bit = bit * 2, i++) {
						if (other == i) {
							return ((Data[offset] & bit) > 0);
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the offset from the beginning of this lump for the given <paramref name="cluster"/>'s PVS data, if applicable.
		/// </summary>
		/// <param name="cluster">The cluster to get the offset to the PVS data for.</param>
		/// <returns>
		/// The offset from the beginning of this lump to the given <paramref name="cluster"/>'s PVS data, or
		/// -1 if this BSP does not use vis clusters.
		/// </returns>
		public int GetOffsetForClusterPVS(int cluster) {
			switch (Bsp.version) {
				case MapType.Quake2:
				case MapType.Daikatana:
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
				case MapType.DMoMaM:
				case MapType.L4D2:
				case MapType.TacticalInterventionEncrypted:
				case MapType.Vindictus: {
					return BitConverter.ToInt32(Data, 4 + (cluster * 8));
				}
			}

			return -1;
		}

		/// <summary>
		/// Gets the offset from the beginning of this lump for the given <paramref name="cluster"/>'s PAS data, if applicable.
		/// </summary>
		/// <param name="cluster">The cluster to get the offset to the PAS data for.</param>
		/// <returns>
		/// The offset from the beginning of this lump to the given <paramref name="cluster"/>'s PAS data, or
		/// -1 if this BSP does not use vis clusters.
		/// </returns>
		public int GetOffsetForClusterPAS(int cluster) {
			switch (Bsp.version) {
				case MapType.Quake2:
				case MapType.Daikatana:
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
				case MapType.DMoMaM:
				case MapType.L4D2:
				case MapType.TacticalInterventionEncrypted:
				case MapType.Vindictus: {
					return BitConverter.ToInt32(Data, 8 + (cluster * 8));
				}
			}

			return -1;
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.SiN:
				case MapType.SoF: {
					return 3;
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
					return 4;
				}
				case MapType.Nightfire: {
					return 7;
				}
				case MapType.FAKK:
				case MapType.MOHAA: {
					return 15;
				}
				case MapType.Quake3:
				case MapType.Raven: {
					return 16;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 17;
				}
				case MapType.CoD: {
					return 28;
				}
				case MapType.CoD2: {
					return 36;
				}
				case MapType.CoD4:
				case MapType.Titanfall:
				default: {
					return -1;
				}
			}
		}

	}
}
