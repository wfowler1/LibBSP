using System;
using System.Collections.Generic;

namespace LibBSP {
	public enum GameLumpType : int {
		sprp = 1936749168,
	}

	/// <summary>
	/// Class containing the identification and information for the various Game Lumps in Source
	/// engine BSPs. The only one we're really concerned with is the Static Props.
	/// </summary>
	public class GameLump : Dictionary<GameLumpType, GameLump.GameLumpInfo> {

		public struct GameLumpInfo {
			public ushort flags;
			public ushort version;
			public int offset;
			public int length;
		}

		private byte[] _rawData;
		private int _gameLumpOffset;

		/// <summary>
		/// Byte array representing the raw data read from the BSP for the game lump
		/// </summary>
		public byte[] rawData {
			get {
				return _rawData;
			}
		}

		/// <summary>
		/// The amount to subtract from all the <c>GameLumpInfo.offset</c> values to find the offset relative to the start of the Game Lump data. May be 0.
		/// </summary>
		public int gameLumpOffset {
			get {
				return _gameLumpOffset;
			}
		}

		/// <summary>
		/// Creates a new <c>GameLump</c> object by parsing a <c>byte</c> array into a <c>Dictionary</c> of <c>GameLumpInfo</c> objects.
		/// These objects contain offsets, lengths and versions of the GameLump lumps.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public GameLump(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			_rawData = data;

			int structLength = 0;
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
				case MapType.Source27: {
					structLength = 16;
					break;
				}
				case MapType.DMoMaM: {
					structLength = 20;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the GameLump class.");
				}
			}

			int numGameLumps = BitConverter.ToInt32(data, 0);
			if (numGameLumps > 0) {
				int headerLength = 4 + (numGameLumps * structLength);
				int lowestLumpOffset = Int32.MaxValue;

				for (int i = 0; i < numGameLumps; ++i) {
					GameLumpInfo info = new GameLumpInfo {
						flags = BitConverter.ToUInt16(data, (i * structLength) + 8),
						version = BitConverter.ToUInt16(data, (i * structLength) + 10),
						offset = BitConverter.ToInt32(data, (i * structLength) + 12),
						length = BitConverter.ToInt32(data, (i * structLength) + 16),
					};
					this[(GameLumpType)BitConverter.ToInt32(data, (i * structLength) + 4)] = info;

					if (info.offset < lowestLumpOffset) {
						lowestLumpOffset = info.offset;
					}
				}

				// If the offset values are given relative to the beginning of the lump, this should give 0.
				_gameLumpOffset = lowestLumpOffset - headerLength;
			}
		}

		/// <summary>
		/// Passes a <c>byte</c> array into the constructor for <c>GameLump</c>.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A new <c>GameLump</c> object</returns>
		/// <remarks>This is only here for consistency with the other lump structures.</remarks>
		public static GameLump LumpFactory(byte[] data, MapType type) {
			return new GameLump(data, type);
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump</returns>
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
				case MapType.DMoMaM: {
					return 35;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
