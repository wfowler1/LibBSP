using System;

namespace LibBSP {
#if UNITY
	using Color = UnityEngine.Color32;
#elif GODOT
	using Color = Godot.Color;
#else
	using Color = System.Drawing.Color;
#endif

	/// <summary>
	/// Holds the visibility data for a BSP.
	/// </summary>
	public class Lightmaps : ILump {

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
		/// Parses the passed <c>byte</c> array into a <see cref="Lightmaps"/> object.
		/// </summary>
		/// <param name="data">Array of <c>byte</c>s to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		public Lightmaps(byte[] data, BSP bsp, LumpInfo lumpInfo = default(LumpInfo)) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Data = data;
			Bsp = bsp;
			LumpInfo = lumpInfo;
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4: {
					return 1;
				}
				case MapType.MOHAA:
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.FAKK: {
					return 2;
				}
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.SiN:
				case MapType.SoF: {
					return 7;
				}
				case MapType.Quake:
				case MapType.GoldSrc:
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
					return 8;
				}
				case MapType.Nightfire: {
					return 10;
				}
				case MapType.Quake3:
				case MapType.Raven: {
					return 14;
				}
				case MapType.Titanfall:
				default: {
					return -1;
				}
			}
		}

	}
}
