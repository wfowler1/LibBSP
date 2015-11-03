#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
#define UNITY
#endif
using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#else
using System.Drawing;
#endif

namespace LibBSP {
#if !UNITY
	using Vector2 = Vector2d;
	using Vector3 = Vector3d;
	using Color32 = Color;
#endif
	/// <summary>
	/// Class containing all data necessary to render a Bezier patch.
	/// </summary>
	public class MAPPatch {

		public UIVertex[] points;
		public Vector2 dims;
		public string texture;

		/// <summary>
		/// Creates a new empty <c>MAPPatch</c> object. Internal data will have to be set manually.
		/// </summary>
		public MAPPatch() { }

		/// <summary>
		/// Constructs a new <c>MAPPatch</c> object using the supplied string array as data.
		/// </summary>
		/// <param name="lines">Data to parse.</param>
		public MAPPatch(string[] lines) {

			texture = lines[2];
			List<UIVertex> vertices = new List<UIVertex>(9);

			switch (lines[0]) {
				case "patchDef3":
				case "patchDef2": {
					string[] line = lines[3].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					dims = new Vector2(Single.Parse(line[1]), Single.Parse(line[2]));
					for (int i = 0; i < dims.x; ++i) {
						line = lines[i + 5].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						for (int j = 0; j < dims.y; ++j) {
							Vector3 point = new Vector3(Single.Parse(line[2 + (j * 7)]), Single.Parse(line[3 + (j * 7)]), Single.Parse(line[4 + (j * 7)]));
							Vector2 uv = new Vector2(Single.Parse(line[5 + (j * 7)]), Single.Parse(line[6 + (j * 7)]));
							UIVertex vertex = new UIVertex() {
								position = point,
								uv0 = uv
							};
							vertices.Add(vertex);
						}
					}
					break;
				}
				case "patchTerrainDef3": {
					string[] line = lines[3].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					dims = new Vector2(Single.Parse(line[1]), Single.Parse(line[2]));
					for (int i = 0; i < dims.x; ++i) {
						line = lines[i + 5].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						for (int j = 0; j < dims.y; ++j) {
							Vector3 point = new Vector3(Single.Parse(line[2 + (j * 12)]), Single.Parse(line[3 + (j * 12)]), Single.Parse(line[4 + (j * 12)]));
							Vector2 uv = new Vector2(Single.Parse(line[5 + (j * 12)]), Single.Parse(line[6 + (j * 12)]));
							Color32 color = Color32Extensions.FromArgb(Byte.Parse(line[7 + (j * 12)]), Byte.Parse(line[8 + (j * 12)]), Byte.Parse(line[9 + (j * 12)]), Byte.Parse(line[10 + (j * 12)]));
							UIVertex vertex = new UIVertex() {
								position = point,
								uv0 = uv,
								color = color
							};
							vertices.Add(vertex);
						}
					}
					break;
				}
				default: {
					throw new ArgumentException(string.Format("Unknown patch type {0}! Call a scientist! ", lines[0]));
				}
			}

		}

	}
}
