using System;
using System.Collections.Generic;

namespace LibBSP {

	/// <summary>
	/// Enum containing known game lumps.
	/// </summary>
	public enum GameLumpType : int {
		hlpd = 1685089384,
		tlpd = 1685089396,
		prpd = 1685090928,
		sprp = 1936749168,
	}

	/// <summary>
	/// Class containing the identification and information for the various Game Lumps in Source
	/// engine BSPs.
	/// </summary>
	public class GameLump : Dictionary<GameLumpType, LumpInfo> {

		/// <summary>
		/// Creates a new <see cref="GameLump"/> object by parsing a <c>byte</c> array into a <c>Dictionary</c> of <see cref="LumpInfo"/> objects.
		/// These objects contain offsets, lengths and versions of the GameLump lumps.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public GameLump(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			int structLength = 0;
			switch (type) {
				case MapType.TacticalInterventionEncrypted:
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.L4D2:
				case MapType.Source27: {
					structLength = 16;
					break;
				}
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					structLength = 20;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " doesn't use a Game lump or the lump is unknown.");
				}
			}

			int numGameLumps = BitConverter.ToInt32(data, 0);
			if (numGameLumps > 0) {
				int lumpDictionaryOffset = (type == MapType.DMoMaM) ? 8 : 4;
				int lowestLumpOffset = int.MaxValue;

				for (int i = 0; i < numGameLumps; ++i) {
					int lumpIdent = BitConverter.ToInt32(data, (i * structLength) + lumpDictionaryOffset);
					int lumpFlags;
					int lumpVersion;
					int lumpOffset;
					int lumpLength;
					if (type == MapType.Vindictus) {
						lumpFlags = BitConverter.ToInt32(data, (i * structLength) + lumpDictionaryOffset + 4);
						lumpVersion = BitConverter.ToInt32(data, (i * structLength) + lumpDictionaryOffset + 8);
						lumpOffset = BitConverter.ToInt32(data, (i * structLength) + lumpDictionaryOffset + 12);
						lumpLength = BitConverter.ToInt32(data, (i * structLength) + lumpDictionaryOffset + 16);
					} else {
						lumpFlags = BitConverter.ToUInt16(data, (i * structLength) + lumpDictionaryOffset + 4);
						lumpVersion = BitConverter.ToUInt16(data, (i * structLength) + lumpDictionaryOffset + 6);
						lumpOffset = BitConverter.ToInt32(data, (i * structLength) + lumpDictionaryOffset + 8);
						lumpLength = BitConverter.ToInt32(data, (i * structLength) + lumpDictionaryOffset + 12);
					}
					LumpInfo info = new LumpInfo {
						ident = lumpIdent,
						flags = lumpFlags,
						version = lumpVersion,
						offset = lumpOffset,
						length = lumpLength,
					};
					this[(GameLumpType)info.ident] = info;

					if (info.offset < lowestLumpOffset) {
						lowestLumpOffset = info.offset;
					}
				}
			}
		}

		/// <summary>
		/// Passes a <c>byte</c> array into the constructor for <see cref="GameLump"/>.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A new <see cref="GameLump"/> object.</returns>
		/// <remarks>This is only here for consistency with the other lump structures.</remarks>
		public static GameLump LumpFactory(byte[] data, MapType type, int version = 0) {
			return new GameLump(data, type, version);
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
					return 35;
				}
				default: {
					return -1;
				}
			}
		}

		/// <summary>
		/// Gets the lowest offset used for a Game Lump.
		/// </summary>
		/// <returns>The lowest offset used for a Game Lump, or <c>int.MaxValue</c> if no Game Lumps exist.</returns>
		public int GetLowestLumpOffset() {
			int lowest = int.MaxValue;
			foreach (LumpInfo info in Values) {
				if (info.offset < lowest) {
					lowest = info.offset;
				}
			}
			return lowest;
		}
	}
}
