using System;
using System.Collections.Generic;
using System.Reflection;

namespace LibBSP {

	/// <summary>
	/// <c>List</c>&lt;<see cref="Texture"/>&gt; with some useful methods for manipulating <see cref="Texture"/> objects,
	/// especially when handling them as a group.
	/// </summary>
	public class Textures : Lump<Texture> {

		/// <summary>
		/// Creates an empty <see cref="Textures"/> object.
		/// </summary>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		public Textures(BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) : base(bsp, lumpInfo) { }

		/// <summary>
		/// Creates a new <see cref="Textures"/> that contains elements copied from the passed <see cref="IEnumerable{Texture}"/>.
		/// </summary>
		/// <param name="items">The elements to copy into this <c>Lump</c>.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		public Textures(IEnumerable<Texture> items, BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) : base(items, bsp, lumpInfo) { }

		/// <summary>
		/// Creates an empty <see cref="Textures"/> object with the specified initial capactiy.
		/// </summary>
		/// <param name="capacity">The number of elements that can initially be stored.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		public Textures(int capacity, BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) : base(capacity, bsp, lumpInfo) { }

		/// <summary>
		/// Parses the passed <c>byte</c> array into a <see cref="Textures"/> object.
		/// </summary>
		/// <param name="data">Array of <c>byte</c>s to parse.</param>
		/// <param name="structLength">Number of <c>byte</c>s to copy into the children. Will be recalculated based on BSP format.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> or <paramref name="bsp"/> was <c>null</c>.</exception>
		public Textures(byte[] data, int structLength, BSP bsp, LumpInfo lumpInfo = default(LumpInfo)) : base(bsp, lumpInfo) {
			if (data == null || bsp == null) {
				throw new ArgumentNullException();
			}

			switch (bsp.version) {
				case MapType.Nightfire: {
					structLength = 64;
					break;
				}
				case MapType.Quake3:
				case MapType.Raven:
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4: {
					structLength = 72;
					break;
				}
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.SoF:
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.FAKK: {
					structLength = 76;
					break;
				}
				case MapType.MOHAA: {
					structLength = 140;
					break;
				}
				case MapType.SiN: {
					structLength = 180;
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
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					int offset = 0;
					for (int i = 0; i < data.Length; ++i) {
						if (data[i] == (byte)0x00) {
							// They are null-terminated strings, of non-constant length (not padded)
							byte[] myBytes = new byte[i - offset];
							Array.Copy(data, offset, myBytes, 0, i - offset);
							Add(new Texture(myBytes, this));
							offset = i + 1;
						}
					}
					return;
				}
				case MapType.Quake: {
					int numElements = BitConverter.ToInt32(data, 0);
					structLength = 40;
					for (int i = 0; i < numElements; ++i) {
						byte[] myBytes = new byte[structLength];
						Array.Copy(data, BitConverter.ToInt32(data, (i + 1) * 4), myBytes, 0, structLength);
						Add(new Texture(myBytes, this));
					}
					return;
				}
				default: {
					throw new ArgumentException("Lump object Texture does not exist in map type " + bsp.version + " or has not been implemented.");
				}
			}

			int numObjects = data.Length / structLength;
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				Add(new Texture(bytes, this));
			}
		}

		/// <summary>
		/// Gets the name of the texture at the specified offset.
		/// </summary>
		/// <param name="offset">Lump offset of the texture name to find.</param>
		/// <returns>The name of the texture at offset <paramref name="offset" />, or null if it doesn't exist.</returns>
		public string GetTextureAtOffset(uint offset) {
			int current = 0;
			for (int i = 0; i < Count; ++i) {
				if (current < offset) {
					// Add 1 for the missing null byte.
					current += this[i].Name.Length + 1;
				} else {
					return this[i].Name;
				}
			}
			// If we get to this point, the strings ended before target offset was reached
			return null;
		}

		/// <summary>
		/// Finds the offset of the specified texture name.
		/// </summary>
		/// <param name="name">The texture name to find in the lump.</param>
		/// <returns>The offset of the specified texture, or -1 if it wasn't found.</returns>
		public int GetOffsetOf(string name) {
			int offset = 0;
			for (int i = 0; i < Count; ++i) {
				if (this[i].Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) {
					return offset;
				} else {
					offset += this[i].Name.Length + 1;
				}
			}
			// If we get here, the requested texture didn't exist.
			return -1;
		}

	}
}
