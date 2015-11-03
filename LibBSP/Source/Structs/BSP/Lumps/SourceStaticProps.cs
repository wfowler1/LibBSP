using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// List of <c>SourceStaticProp</c> objects containing data relevant to Static Props, like the dictionary of actual model paths.
	/// </summary>
	public class SourceStaticProps : List<SourceStaticProp> {

		public string[] dictionary { get; private set; }

		/// <summary>
		/// Parses the passed <c>byte</c> array into a <c>List</c> of <c>SourceStaticProp</c> objects
		/// </summary>
		/// <param name="data">Array of <c>byte</c>s to parse</param>
		/// <param name="type">Format identifier</param>
		/// <param name="version">Version of static prop lump this is</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		public SourceStaticProps(byte[] data, MapType type, int version) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			dictionary = new string[0];
			if (data.Length > 0) {
				int offset = 0;
				dictionary = new string[BitConverter.ToInt32(data, 0)];
				for (int i = 0; i < dictionary.Length; ++i) {
					byte[] temp = new byte[128];
					Array.Copy(data, (i * 128) + 4, temp, 0, 128);
					dictionary[i] = temp.ToNullTerminatedString();
				}
				int numLeafDefinitions = BitConverter.ToInt32(data, (dictionary.Length * 128) + 4);
				int numProps = BitConverter.ToInt32(data, (dictionary.Length * 128) + (numLeafDefinitions * 2) + 8);
				if (numProps > 0) {
					structLength = (data.Length - ((dictionary.Length * 128) + (numLeafDefinitions * 2) + 12)) / numProps;
					byte[] bytes = new byte[structLength];
					for (int i = 0; i < numProps; ++i) {
						Array.Copy(data, (dictionary.Length * 128) + (numLeafDefinitions * 2) + 12 + (i * structLength), bytes, 0, structLength);
						Add(new SourceStaticProp(bytes, type, version));
						offset += structLength;
					}
				}
			}
		}
	}
}
