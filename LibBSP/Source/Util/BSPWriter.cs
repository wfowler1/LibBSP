using System.IO;

namespace LibBSP {

	/// <summary>
	/// Handles reading of a BSP file.
	/// </summary>
	public class BSPWriter {

		private BSP _bsp;

		/// <summary>
		/// Constructs a new <see cref="BSPWriter"/> for the given <paramref name="bsp"/>.
		/// </summary>
		/// <param name="bsp">The <see cref="BSP"/> to write.</param>
		public BSPWriter(BSP bsp) {
			_bsp = bsp;
		}

		/// <summary>
		/// Writes the <see cref="BSP"/> to the file at <paramref name="path"/>.
		/// </summary>
		/// <param name="path">The file path to write the <see cref="BSP"/> to.</param>
		public void WriteBSP(string path) {
			BSPHeader header = _bsp.Header.Regenerate();

			using (FileStream stream = File.OpenWrite(path)) {
				stream.Seek(0, SeekOrigin.Begin);
				stream.Write(header.Data, 0, header.Data.Length);
				int offset = header.Data.Length;
				int numLumps = BSP.GetNumLumps(_bsp.MapType);

				for (int i = 0; i < numLumps; ++i) {
					ILump lump = _bsp.GetLoadedLump(i);
					byte[] bytes;
					if (lump != null) {
						bytes = lump.GetBytes();
					} else {
						bytes = _bsp.Reader.ReadLump(_bsp.Header.GetLumpInfo(i));
					}
					stream.Write(bytes, 0, bytes.Length);
					offset += bytes.Length;
				}
			}
		}

	}
}
