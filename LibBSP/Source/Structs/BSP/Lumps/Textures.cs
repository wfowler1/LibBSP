using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// <c>List</c>&lt;<see cref="Texture"/>&gt; with some useful methods for manipulating <see cref="Texture"/> objects,
	/// especially when handling them as a group.
	/// </summary>
	public class Textures : List<Texture> {

		/// <summary>
		/// Parses a <c>byte</c> array into this <c>List</c> of <see cref="Texture"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public Textures(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
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
				case MapType.TacticalInterventionEncrypted:
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					int offset = 0;
					byte[] bytes;
					for (int i = 0; i < data.Length; ++i) {
						if (data[i] == (byte)0x00) {
							// They are null-terminated strings, of non-constant length (not padded)
							bytes = new byte[i - offset];
							Array.Copy(data, offset, bytes, 0, i - offset);
							Add(new Texture(bytes, type, version));
							offset = i + 1;
						}
					}
					return;
				}
				case MapType.Quake: {
					int numElements = BitConverter.ToInt32(data, 0);
					structLength = 40;
					byte[] bytes = new byte[structLength];
					for (int i = 0; i < numElements; ++i) {
						Array.Copy(data, BitConverter.ToInt32(data, (i + 1) * 4), bytes, 0, structLength);
						Add(new Texture(bytes, type, version));
					}
					return;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Node class.");
				}
			}

			{
				byte[] bytes = new byte[structLength];
				for (int i = 0; i < data.Length / structLength; ++i) {
					Array.Copy(data, (i * structLength), bytes, 0, structLength);
					Add(new Texture(bytes, type, version));
				}
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
					current += this[i].name.Length + 1;
				} else {
					return this[i].name;
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
				if (this[i].name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) {
					return offset;
				} else {
					offset += this[i].name.Length + 1;
				}
			}
			// If we get here, the requested texture didn't exist.
			return -1;
		}
	}
}
