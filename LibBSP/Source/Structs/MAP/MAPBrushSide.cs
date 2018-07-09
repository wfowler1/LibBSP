#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER)
#define UNITY
#endif

using System.Collections.Generic;
using System;
using System.Globalization;
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
	[Serializable] public class MAPBrushSide {

		private static IFormatProvider _format = CultureInfo.CreateSpecificCulture("en-US");

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
		/// Creates a new empty <see cref="MAPBrushSide"/> object. Internal data will have to be set manually.
		/// </summary>
		public MAPBrushSide() { }

		/// <summary>
		/// Constructs a <see cref="MAPBrushSide"/> object using the provided <c>string</c> array as the data.
		/// </summary>
		/// <param name="lines">Data to parse.</param>
		public MAPBrushSide(string[] lines) {
			// If lines.Length is 1, then this line contains all data for a brush side
			if (lines.Length == 1) {
				string[] tokens = lines[0].SplitUnlessInContainer(' ', '\"', StringSplitOptions.RemoveEmptyEntries);

				float dist = 0;

				// If this succeeds, assume brushDef3
				if (Single.TryParse(tokens[4], out dist)) {
					plane = new Plane(new Vector3(Single.Parse(tokens[1], _format), Single.Parse(tokens[2], _format), Single.Parse(tokens[3], _format)), dist);
					textureS = new Vector3(Single.Parse(tokens[8], _format), Single.Parse(tokens[9], _format), Single.Parse(tokens[10], _format));
					textureT = new Vector3(Single.Parse(tokens[13], _format), Single.Parse(tokens[14], _format), Single.Parse(tokens[15], _format));
					texture = tokens[18];
				} else {
					Vector3 v1 = new Vector3(Single.Parse(tokens[1], _format), Single.Parse(tokens[2], _format), Single.Parse(tokens[3], _format));
					Vector3 v2 = new Vector3(Single.Parse(tokens[6], _format), Single.Parse(tokens[7], _format), Single.Parse(tokens[8], _format));
					Vector3 v3 = new Vector3(Single.Parse(tokens[11], _format), Single.Parse(tokens[12], _format), Single.Parse(tokens[13], _format));
					vertices = new Vector3[] { v1, v2, v3 };
					plane = new Plane(v1, v2, v3);
					texture = tokens[15];
					// GearCraft
					if (tokens[16] == "[") {
						textureS = new Vector3(Single.Parse(tokens[17], _format), Single.Parse(tokens[18], _format), Single.Parse(tokens[19], _format));
						textureShiftS = Double.Parse(tokens[20], _format);
						textureT = new Vector3(Single.Parse(tokens[23], _format), Single.Parse(tokens[24], _format), Single.Parse(tokens[25], _format));
						textureShiftT = Double.Parse(tokens[26], _format);
						texRot = Single.Parse(tokens[28], _format);
						texScaleX = Double.Parse(tokens[29], _format);
						texScaleY = Double.Parse(tokens[30], _format);
						flags = Int32.Parse(tokens[31]);
						material = tokens[32];
					} else {
						//<x_shift> <y_shift> <rotation> <x_scale> <y_scale> <content_flags> <surface_flags> <value>
						textureShiftS = Single.Parse(tokens[16], _format);
						textureShiftT = Single.Parse(tokens[17], _format);
						texRot = Single.Parse(tokens[18], _format);
						texScaleX = Double.Parse(tokens[19], _format);
						texScaleY = Double.Parse(tokens[20], _format);
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
								Vector3 v1 = new Vector3(Single.Parse(components[0], _format), Single.Parse(components[1], _format), Single.Parse(components[2], _format));
								components = points[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								Vector3 v2 = new Vector3(Single.Parse(components[0], _format), Single.Parse(components[1], _format), Single.Parse(components[2], _format));
								components = points[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								Vector3 v3 = new Vector3(Single.Parse(components[0], _format), Single.Parse(components[1], _format), Single.Parse(components[2], _format));
								plane = new Plane(v1, v2, v3);
								break;
							}
							case "uaxis": {
								string[] split = tokens[1].SplitUnlessBetweenDelimiters(' ', '[', ']', StringSplitOptions.RemoveEmptyEntries);
								texScaleX = Single.Parse(split[1], _format);
								split = split[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								textureS = new Vector3(Single.Parse(split[0], _format), Single.Parse(split[1], _format), Single.Parse(split[2], _format));
								textureShiftS = Single.Parse(split[3], _format);
								break;
							}
							case "vaxis": {
								string[] split = tokens[1].SplitUnlessBetweenDelimiters(' ', '[', ']', StringSplitOptions.RemoveEmptyEntries);
								texScaleY = Single.Parse(split[1], _format);
								split = split[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								textureT = new Vector3(Single.Parse(split[0], _format), Single.Parse(split[1], _format), Single.Parse(split[2], _format));
								textureShiftT = Single.Parse(split[3], _format);
								break;
							}
							case "rotation": {
								texRot = Single.Parse(tokens[1], _format);
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
