using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// List of <see cref="StaticProp"/> objects containing data relevant to Static Props, like the dictionary of actual model paths.
	/// </summary>
	public class StaticProps : List<StaticProp> {

		public string[] dictionary { get; private set; }

		/// <summary>
		/// Parses the passed <c>byte</c> array into a <c>List</c> of <see cref="StaticProp"/> objects.
		/// </summary>
		/// <param name="data">Array of <c>byte</c>s to parse.</param>
		/// <param name="type">Format identifier.</param>
		/// <param name="version">Version of static prop lump this is.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public StaticProps(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			dictionary = new string[0];
			if (data.Length > 0) {
				int offset = 0;
				dictionary = new string[BitConverter.ToInt32(data, 0)];
				offset += 4;
				for (int i = 0; i < dictionary.Length; ++i) {
					byte[] temp = new byte[128];
					Array.Copy(data, offset, temp, 0, 128);
					dictionary[i] = temp.ToNullTerminatedString();
					offset += 128;
				}
				int numLeafDefinitions = BitConverter.ToInt32(data, (dictionary.Length * 128) + 4);
				offset += 4 + (numLeafDefinitions * 2);
				if (type == MapType.Vindictus && version == 6) {
					int numPropScales = BitConverter.ToInt32(data, offset);
					offset += 4 + (numPropScales * 16);
				}
				int numProps = BitConverter.ToInt32(data, offset);
				offset += 4;
				if (numProps > 0) {
					structLength = (data.Length - offset) / numProps;
					byte[] bytes = new byte[structLength];
					for (int i = 0; i < numProps; ++i) {
						Array.Copy(data, offset, bytes, 0, structLength);
						Add(new StaticProp(bytes, type, version));
						offset += structLength;
					}
				}
			}
		}
	}
}
