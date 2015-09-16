#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
#define UNITY
#endif

using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// Class containing all data for a single brush, including side definitions or a patch definition
	/// </summary>
	public class MAPBrush {

		public List<MAPBrushSide> sides = new List<MAPBrushSide>(6);
		public MAPPatch patch;

		public bool isDetail { get; private set; }

		/// <summary>
		/// Creates a new <c>MAPBrush</c> object using the supplied <c>string</c> array as data
		/// </summary>
		/// <param name="lines">Data to parse</param>
		public MAPBrush(string[] lines) {
			int braceCount = 0;
			bool brushDef3 = false;
			bool inPatch = false;
			List<string> child = new List<string>();
			foreach (string line in lines) {
				if (line[0] == '{') {
					braceCount++;
					if (braceCount == 1 || brushDef3) { continue; }
				} else if (line[0] == '}') {
					braceCount--;
					if (braceCount == 0 || brushDef3) { continue; }
				}

				if (braceCount == 1 || brushDef3) {
					// Source engine
					if (line.Length >= "side".Length && line.Substring(0, "side".Length) == "side") {
						continue;
					}
						// id Tech does this kinda thing
					else if (line.Length >= "patch".Length && line.Substring(0, "patch".Length) == "patch") {
						inPatch = true;
						// Gonna need this line too. We can switch on the type of patch definition, make things much easier.
						child.Add(line);
						continue;
					} else if (inPatch) {
						child.Add(line);
						inPatch = false;
						patch = new MAPPatch(child.ToArray());
						child = new List<string>();
						continue;
					} else if (line.Length >= "brushDef3".Length && line.Substring(0, "brushDef3".Length) == "brushDef3") {
						brushDef3 = true;
						continue;
					} else if (line == "\"BRUSHFLAGS\" \"DETAIL\"") {
						isDetail = true;
						continue;
					} else if (line.Length >= "\"id\"".Length && line.Substring(0, "\"id\"".Length) == "\"id\"") {
						continue;
					} else {
						child.Add(line);
						sides.Add(new MAPBrushSide(child.ToArray()));
						child = new List<string>();
					}
				} else if (braceCount > 1) {
					child.Add(line);
				}
			}
		}

	}
}
