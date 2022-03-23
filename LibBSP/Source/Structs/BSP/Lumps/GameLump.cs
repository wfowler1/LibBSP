using System;
using System.Collections.Generic;

namespace LibBSP {

	/// <summary>
	/// Enum containing known game lumps.
	/// </summary>
	public enum GameLumpType : int {
		/// <summary> LDR detail prop lighting. </summary>
		hlpd = 1685089384,
		/// <summary> HDR detail prop lighting. </summary>
		tlpd = 1685089396,
		/// <summary> Detail props. </summary>
		prpd = 1685090928,
		/// <summary> <see cref="StaticProps"/>. </summary>
		prps = 1936749168,
	}

	/// <summary>
	/// Class containing the identification and information for the various Game Lumps in Source
	/// engine BSPs.
	/// </summary>
	public class GameLump : Dictionary<GameLumpType, LumpInfo>, ILump {

		public Dictionary<GameLumpType, ILump> gameLumps;

		/// <summary>
		/// The <see cref="BSP"/> this <see cref="ILump"/> came from.
		/// </summary>
		public BSP Bsp { get; protected set; }

		/// <summary>
		/// The <see cref="LibBSP.LumpInfo"/> associated with this <see cref="ILump"/>.
		/// </summary>
		public LumpInfo LumpInfo { get; protected set; }

		/// <summary>
		/// Parses the passed <c>byte</c> array into a <see cref="GameLump"/> object.
		/// </summary>
		/// <param name="data">Array of <c>byte</c>s to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public GameLump(byte[] data, BSP bsp, LumpInfo lumpInfo = default(LumpInfo)) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			Bsp = bsp;
			LumpInfo = lumpInfo;

			int structLength = 0;
			if (bsp.version == MapType.DMoMaM
				|| bsp.version == MapType.Vindictus) {
				structLength = 20;
			} else if (bsp.version.IsSubtypeOf(MapType.Source)
				|| bsp.version == MapType.Titanfall) {
				structLength = 16;
			} else {
				throw new ArgumentException("Game lump does not exist in map type " + bsp.version + " or has not been implemented.");
			}

			int numGameLumps = BitConverter.ToInt32(data, 0);
			gameLumps = new Dictionary<GameLumpType, ILump>(numGameLumps);

			if (numGameLumps > 0) {
				int lumpDictionaryOffset = (bsp.version == MapType.DMoMaM) ? 8 : 4;
				int lowestLumpOffset = int.MaxValue;

				for (int i = 0; i < numGameLumps; ++i) {
					int lumpIdent = BitConverter.ToInt32(data, (i * structLength) + lumpDictionaryOffset);
					int lumpFlags;
					int lumpVersion;
					int lumpOffset;
					int lumpLength;

					if (bsp.version == MapType.Vindictus) {
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
						lumpFile = lumpInfo.lumpFile,
					};

					this[(GameLumpType)info.ident] = info;

					if (info.offset < lowestLumpOffset) {
						lowestLumpOffset = info.offset;
					}
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="GameLump"/> object.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <returns>A <see cref="GameLump"/> object.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> parameter was <c>null</c>.</exception>
		public static GameLump LumpFactory(byte[] data, BSP bsp, LumpInfo lumpInfo) {
			if (data == null) {
				throw new ArgumentNullException();
			}

			return new GameLump(data, bsp, lumpInfo);
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForLump(MapType type) {
			if (type.IsSubtypeOf(MapType.Source)
				|| type == MapType.Titanfall) {
				return 35;
			}

			return -1;
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

		/// <summary>
		/// The <see cref="LibBSP.StaticProps"/> object in the BSP file extracted from this lump, if available.
		/// </summary>
		public StaticProps StaticProps {
			get {
				if (!gameLumps.ContainsKey(GameLumpType.prps)) {
					byte[] bytes = GetData(GameLumpType.prps);
					if (bytes != null) {
						gameLumps[GameLumpType.prps] = StaticProp.LumpFactory(bytes, Bsp, this[GameLumpType.prps]);
					}
				}

				return gameLumps[GameLumpType.prps] as StaticProps;
			}
		}

		/// <summary>
		/// Has the Static Props lump been loaded yet?
		/// </summary>
		public bool StaticPropsLoaded {
			get {
				return gameLumps.ContainsKey(GameLumpType.prps);
			}
		}

		/// <summary>
		/// Gets the bytes for a <see cref="GameLumpType"/>, if it exists.
		/// </summary>
		/// <param name="lump">The <see cref="GameLumpType"/> to get data for.</param>
		/// <returns>The data for <paramref name="lump"/>, or <c>null</c> if it does not exist.</returns>
		public byte[] GetData(GameLumpType lump) {
			if (ContainsKey(lump)) {
				LumpInfo info = this[lump];
				byte[] thisLump;
				if (gameLumps.ContainsKey(lump)) {
					thisLump = gameLumps[lump].GetBytes();
				} else {
					// GameLump lumps may have their offset specified from either the beginning of the GameLump, or the beginning of the file.
					if (GetLowestLumpOffset() < LumpInfo.offset) {
						thisLump = Bsp.reader.ReadLump(new LumpInfo() {
							ident = info.ident,
							flags = info.flags,
							version = info.version,
							offset = info.offset + LumpInfo.offset,
							length = info.length
						});
					} else {
						thisLump = Bsp.reader.ReadLump(info);
					}
				}

				return thisLump;
			}

			return null;
		}

		/// <summary>
		/// Gets all the data in this lump as a byte array.
		/// </summary>
		/// <returns>The data.</returns>
		public byte[] GetBytes() {
			if (Count == 0) {
				return new byte[0];
			}

			int lumpInfoLength = (Bsp.version == MapType.DMoMaM || Bsp.version == MapType.Vindictus) ? 20 : 16;
			int lumpDictionaryOffset = (Bsp.version == MapType.DMoMaM) ? 8 : 4;
			int length = lumpDictionaryOffset + (lumpInfoLength * Count);

			Dictionary<GameLumpType, byte[]> lumpBytes = new Dictionary<GameLumpType, byte[]>(gameLumps.Count);
			foreach (GameLumpType type in Keys) {
				if (gameLumps.ContainsKey(type)) {
					lumpBytes[type] = gameLumps[type].GetBytes();
				} else {
					lumpBytes[type] = GetData(type);
				}

				length += lumpBytes[type].Length;
			}

			byte[] bytes = new byte[length];
			BitConverter.GetBytes(lumpBytes.Count).CopyTo(bytes, 0);
			int lumpNumber = 0;
			int offset = lumpDictionaryOffset + (lumpBytes.Count * lumpInfoLength);
			int headerEntryLength = (Bsp.version == MapType.DMoMaM || Bsp.version == MapType.Vindictus) ? 20 : 16;

			foreach (KeyValuePair<GameLumpType, byte[]> pair in lumpBytes) {
				LumpInfo info = this[pair.Key];
				info.length = pair.Value.Length;
				info.offset = offset;
				if (offset < length) {
					info.offset += LumpInfo.offset;
				}
				this[pair.Key] = info;

				BitConverter.GetBytes(info.ident).CopyTo(bytes, (lumpNumber * headerEntryLength) + lumpDictionaryOffset);

				if (Bsp.version == MapType.Vindictus) {
					BitConverter.GetBytes(info.flags).CopyTo(bytes, (lumpNumber * headerEntryLength) + lumpDictionaryOffset + 4);
					BitConverter.GetBytes(info.version).CopyTo(bytes, (lumpNumber * headerEntryLength) + lumpDictionaryOffset + 8);
					BitConverter.GetBytes(info.offset).CopyTo(bytes, (lumpNumber * headerEntryLength) + lumpDictionaryOffset + 12);
					BitConverter.GetBytes(info.length).CopyTo(bytes, (lumpNumber * headerEntryLength) + lumpDictionaryOffset + 16);
				} else {
					BitConverter.GetBytes((short)info.flags).CopyTo(bytes, (lumpNumber * headerEntryLength) + lumpDictionaryOffset + 4);
					BitConverter.GetBytes((short)info.version).CopyTo(bytes, (lumpNumber * headerEntryLength) + lumpDictionaryOffset + 6);
					BitConverter.GetBytes(info.offset).CopyTo(bytes, (lumpNumber * headerEntryLength) + lumpDictionaryOffset + 8);
					BitConverter.GetBytes(info.length).CopyTo(bytes, (lumpNumber * headerEntryLength) + lumpDictionaryOffset + 12);
				}

				lumpBytes[pair.Key].CopyTo(bytes, offset);

				offset += lumpBytes[pair.Key].Length;
				++lumpNumber;
			}

			return bytes;
		}
	}
}
