#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
#define UNITY
#endif

using System.Collections.Generic;
using System;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if !UNITY
	using Vector3 = Vector3d;
#endif
	/// <summary>
	/// Class containing data for a brush side. Please note vertices must be set manually or generated through CSG.
	/// </summary>
	public class MAPBrushSide {

		public Vector3[] vertices;
		public Plane plane;
		public string texture;
		public Vector3 textureS;
		public double textureShiftS;
		public Vector3 textureT;
		public double textureShiftT;
		public float texRot;
		public double texScaleX;
		public double texScaleY;
		public int flags;
		public string material;
		public double lgtScale;
		public double lgtRot;
		public MAPDisplacement displacement;

		/// <summary>
		/// Creates a new empty <c>MAPBrushSide</c> object. Internal data will have to be set manually.
		/// </summary>
		public MAPBrushSide() { }

		/// <summary>
		/// Constructs a <c>MAPBrushSide</c> object using the provided <c>string</c> array as the data.
		/// </summary>
		/// <param name="lines">Data to parse.</param>
		public MAPBrushSide(string[] lines) {
			// If lines.Length is 1, then this line contains all data for a brush side
			if (lines.Length == 1) {
				string[] tokens = lines[0].SplitUnlessInContainer(' ', '\"', StringSplitOptions.RemoveEmptyEntries);

				float dist = 0;

				// If this succeeds, assume brushDef3
				if (Single.TryParse(tokens[4], out dist)) {
					plane = new Plane(new Vector3(Single.Parse(tokens[1]), Single.Parse(tokens[2]), Single.Parse(tokens[3])), dist);
					textureS = new Vector3(Single.Parse(tokens[8]), Single.Parse(tokens[9]), Single.Parse(tokens[10]));
					textureT = new Vector3(Single.Parse(tokens[13]), Single.Parse(tokens[14]), Single.Parse(tokens[15]));
					texture = tokens[18];
				} else {
					Vector3 v1 = new Vector3(Single.Parse(tokens[1]), Single.Parse(tokens[2]), Single.Parse(tokens[3]));
					Vector3 v2 = new Vector3(Single.Parse(tokens[6]), Single.Parse(tokens[7]), Single.Parse(tokens[8]));
					Vector3 v3 = new Vector3(Single.Parse(tokens[11]), Single.Parse(tokens[12]), Single.Parse(tokens[13]));
					plane = new Plane(v1, v2, v3);
					texture = tokens[15];
					// GearCraft
					if (tokens[16] == "[") {
						textureS = new Vector3(Single.Parse(tokens[17]), Single.Parse(tokens[18]), Single.Parse(tokens[19]));
						textureShiftS = Double.Parse(tokens[20]);
						textureT = new Vector3(Single.Parse(tokens[23]), Single.Parse(tokens[24]), Single.Parse(tokens[25]));
						textureShiftT = Double.Parse(tokens[26]);
						texRot = Single.Parse(tokens[28]);
						texScaleX = Double.Parse(tokens[29]);
						texScaleY = Double.Parse(tokens[30]);
						flags = Int32.Parse(tokens[31]);
						material = tokens[32];
					} else {
						//<x_shift> <y_shift> <rotation> <x_scale> <y_scale> <content_flags> <surface_flags> <value>
						textureShiftS = Single.Parse(tokens[16]);
						textureShiftT = Single.Parse(tokens[17]);
						texRot = Single.Parse(tokens[18]);
						texScaleX = Double.Parse(tokens[19]);
						texScaleY = Double.Parse(tokens[20]);
						flags = Int32.Parse(tokens[22]);
					}
				}
			} else {
				bool inDispInfo = false;
				int braceCount = 0;
				List<string> child = new List<string>(37);
				foreach (string line in lines) {
					if (line == "{") {
						++braceCount;
					} else if (line == "}") {
						--braceCount;
						if (braceCount == 1) {
							child.Add(line);
							displacement = new MAPDisplacement(child.ToArray());
							child = new List<string>(37);
							inDispInfo = false;
						}
					} else if (line == "dispinfo") {
						inDispInfo = true;
						continue;
					}

					if (braceCount == 1) {
						string[] tokens = line.SplitUnlessInContainer(' ', '\"', StringSplitOptions.RemoveEmptyEntries);
						switch (tokens[0]) {
							case "material": {
								texture = tokens[1];
								break;
							}
							case "plane": {
								string[] points = tokens[1].SplitUnlessBetweenDelimiters(' ', '(', ')', StringSplitOptions.RemoveEmptyEntries);
								string[] components = points[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								Vector3 v1 = new Vector3(Single.Parse(components[0]), Single.Parse(components[1]), Single.Parse(components[2]));
								components = points[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								Vector3 v2 = new Vector3(Single.Parse(components[0]), Single.Parse(components[1]), Single.Parse(components[2]));
								components = points[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								Vector3 v3 = new Vector3(Single.Parse(components[0]), Single.Parse(components[1]), Single.Parse(components[2]));
								plane = new Plane(v1, v2, v3);
								break;
							}
							case "uaxis": {
								string[] split = tokens[1].SplitUnlessBetweenDelimiters(' ', '[', ']', StringSplitOptions.RemoveEmptyEntries);
								texScaleX = Single.Parse(split[1]);
								split = split[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								textureS = new Vector3(Single.Parse(split[0]), Single.Parse(split[1]), Single.Parse(split[2]));
								textureShiftS = Single.Parse(split[3]);
								break;
							}
							case "vaxis": {
								string[] split = tokens[1].SplitUnlessBetweenDelimiters(' ', '[', ']', StringSplitOptions.RemoveEmptyEntries);
								texScaleY = Single.Parse(split[1]);
								split = split[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								textureT = new Vector3(Single.Parse(split[0]), Single.Parse(split[1]), Single.Parse(split[2]));
								textureShiftT = Single.Parse(split[3]);
								break;
							}
							case "rotation": {
								texRot = Single.Parse(tokens[1]);
								break;
							}
						}
					} else if (braceCount > 1) {
						if (inDispInfo) {
							child.Add(line);
						}
					}
				}
			}
		}

	}
}
