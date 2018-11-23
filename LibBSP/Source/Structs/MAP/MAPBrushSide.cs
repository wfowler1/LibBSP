#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System.Collections.Generic;
using System;
using System.Globalization;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if UNITY
	using Vector2d = Vector2;
	using Vector3d = Vector3;
#endif

	/// <summary>
	/// Class containing data for a brush side. Please note vertices must be set manually or generated through CSG.
	/// </summary>
	[Serializable] public class MAPBrushSide {

		private static IFormatProvider _format = CultureInfo.CreateSpecificCulture("en-US");

		public Vector3d[] vertices;
		public Plane plane;
		public string texture;
		public TextureInfo textureInfo;
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
				if (float.TryParse(tokens[4], out dist)) {
					plane = new Plane(new Vector3d(float.Parse(tokens[1], _format), float.Parse(tokens[2], _format), float.Parse(tokens[3], _format)), dist);
					textureInfo = new TextureInfo(new Vector3d(float.Parse(tokens[8], _format), float.Parse(tokens[9], _format), float.Parse(tokens[10], _format)),
												  new Vector3d(float.Parse(tokens[13], _format), float.Parse(tokens[14], _format), float.Parse(tokens[15], _format)),
												  Vector2d.zero,
												  Vector2d.one,
												  0, 0, 0);
					texture = tokens[18];
				} else {
					Vector3d v1 = new Vector3d(float.Parse(tokens[1], _format), float.Parse(tokens[2], _format), float.Parse(tokens[3], _format));
					Vector3d v2 = new Vector3d(float.Parse(tokens[6], _format), float.Parse(tokens[7], _format), float.Parse(tokens[8], _format));
					Vector3d v3 = new Vector3d(float.Parse(tokens[11], _format), float.Parse(tokens[12], _format), float.Parse(tokens[13], _format));
					vertices = new Vector3d[] { v1, v2, v3 };
					plane = new Plane(v1, v2, v3);
					texture = tokens[15];
					// GearCraft
					if (tokens[16] == "[") {
						textureInfo = new TextureInfo(new Vector3d(float.Parse(tokens[17], _format), float.Parse(tokens[18], _format), float.Parse(tokens[19], _format)),
													  new Vector3d(float.Parse(tokens[23], _format), float.Parse(tokens[24], _format), float.Parse(tokens[25], _format)),
						                              new Vector2d(float.Parse(tokens[20], _format), float.Parse(tokens[26], _format)),
						                              new Vector2d(float.Parse(tokens[29], _format), float.Parse(tokens[30], _format)),
													  int.Parse(tokens[31]), 0, double.Parse(tokens[28], _format));
						material = tokens[32];
					} else {
						//<x_shift> <y_shift> <rotation> <x_scale> <y_scale> <content_flags> <surface_flags> <value>
						Vector3d[] axes = TextureInfo.TextureAxisFromPlane(plane);
						textureInfo = new TextureInfo(axes[0],
						                              axes[1],
						                              new Vector2d(float.Parse(tokens[16], _format), float.Parse(tokens[17], _format)),
						                              new Vector2d(float.Parse(tokens[19], _format), float.Parse(tokens[20], _format)),
						                              int.Parse(tokens[22]), 0, double.Parse(tokens[18], _format));
					}
				}
			} else {
				bool inDispInfo = false;
				int braceCount = 0;
				textureInfo = new TextureInfo();
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
								Vector3d v1 = new Vector3d(float.Parse(components[0], _format), float.Parse(components[1], _format), float.Parse(components[2], _format));
								components = points[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								Vector3d v2 = new Vector3d(float.Parse(components[0], _format), float.Parse(components[1], _format), float.Parse(components[2], _format));
								components = points[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								Vector3d v3 = new Vector3d(float.Parse(components[0], _format), float.Parse(components[1], _format), float.Parse(components[2], _format));
								plane = new Plane(v1, v2, v3);
								break;
							}
							case "uaxis": {
								string[] split = tokens[1].SplitUnlessBetweenDelimiters(' ', '[', ']', StringSplitOptions.RemoveEmptyEntries);
								textureInfo.scale = new Vector2d(float.Parse(split[1], _format), textureInfo.scale.y);
								split = split[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								textureInfo.uAxis = new Vector3d(float.Parse(split[0], _format), float.Parse(split[1], _format), float.Parse(split[2], _format));
								textureInfo.translation = new Vector2d(float.Parse(split[3], _format), textureInfo.translation.y);
								break;
							}
							case "vaxis": {
								string[] split = tokens[1].SplitUnlessBetweenDelimiters(' ', '[', ']', StringSplitOptions.RemoveEmptyEntries);
								textureInfo.scale = new Vector2d(textureInfo.scale.x, float.Parse(split[1], _format));
								split = split[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								textureInfo.vAxis = new Vector3d(float.Parse(split[0], _format), float.Parse(split[1], _format), float.Parse(split[2], _format));
								textureInfo.translation = new Vector2d(textureInfo.translation.x, float.Parse(split[3], _format));
								break;
							}
							case "rotation": {
								textureInfo.rotation = double.Parse(tokens[1], _format);
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
