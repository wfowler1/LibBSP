#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if !UNITY
	using Vector3 = Vector3d;
#endif
	/// <summary>
	/// Class containing all data necessary to render a displacement from Source engine.
	/// </summary>
	public struct MAPDisplacement {

		public int power;
		public Vector3 start;
		public Vector3[][] normals;
		public float[][] distances;
		public float[][] alphas;

		/// <summary>
		/// Constructs a <c>MAPDisplacement</c> object using the provided <c>string</c> array as the data.
		/// </summary>
		/// <param name="lines">Data to parse</param>
		public MAPDisplacement(string[] lines) {
			power = 0;
			start = new Vector3(Single.NaN, Single.NaN, Single.NaN);
			normals = null;
			distances = null;
			alphas = null;
			Dictionary<int, string[]> normalsTokens = new Dictionary<int, string[]>(5);
			Dictionary<int, string[]> distancesTokens = new Dictionary<int, string[]>(5);
			Dictionary<int, string[]> alphasTokens = new Dictionary<int, string[]>(5);
			int braceCount = 0;
			bool inNormals = false;
			bool inDistances = false;
			bool inAlphas = false;
			foreach (string line in lines) {
				if (line == "{") {
					++braceCount;
					continue;
				} else if (line == "}") {
					--braceCount;
					if (braceCount == 1) {
						inNormals = false;
						inDistances = false;
						inAlphas = false;
					}
					continue;
				} else if (line == "normals") {
					inNormals = true;
					continue;
				} else if (line == "distances") {
					inDistances = true;
					continue;
				} else if (line == "alphas") {
					inAlphas = true;
					continue;
				}

				if (braceCount == 1) {
					string[] tokens = line.SplitUnlessInContainer(' ', '\"', StringSplitOptions.RemoveEmptyEntries);
					switch (tokens[0]) {
						case "power": {
							power = Int32.Parse(tokens[1]);
							int side = (int)Math.Pow(2, power) + 1;
							normals = new Vector3[side][];
							distances = new float[side][];
							alphas = new float[side][];
							for (int i = 0; i < side; ++i) {
								normals[i] = new Vector3[side];
								distances[i] = new float[side];
								alphas[i] = new float[side];
							}
							break;
						}
						case "startposition": {
							string[] point = tokens[1].Substring(1, tokens[1].Length - 2).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
							start = new Vector3(Single.Parse(point[0]), Single.Parse(point[1]), Single.Parse(point[2]));
							break;
						}
					}
				} else if (braceCount > 1) {
					if (inNormals) {
						string[] tokens = line.SplitUnlessInContainer(' ', '\"', StringSplitOptions.RemoveEmptyEntries);
						int row = Int32.Parse(tokens[0].Substring(3));
						string[] points = tokens[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						normalsTokens[row] = points;
					} else if (inDistances) {
						string[] tokens = line.SplitUnlessInContainer(' ', '\"', StringSplitOptions.RemoveEmptyEntries);
						int row = Int32.Parse(tokens[0].Substring(3));
						string[] nums = tokens[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						distancesTokens[row] = nums;
					} else if (inAlphas) {
						string[] tokens = line.SplitUnlessInContainer(' ', '\"', StringSplitOptions.RemoveEmptyEntries);
						int row = Int32.Parse(tokens[0].Substring(3));
						string[] nums = tokens[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						alphasTokens[row] = nums;
					}
				}
			}

			if (power == 0) {
				throw new ArgumentException("Bad data given to MAPDisplacement, no power specified!");
			}

			if (start.x == Single.NaN) {
				throw new ArgumentException("Bad data given to MAPDisplacement, no starting point specified!");
			}

			foreach (int i in normalsTokens.Keys) {
				for (int j = 0; j < normalsTokens[i].Length / 3; j++) {
					normals[i][j] = new Vector3(Single.Parse(normalsTokens[i][j * 3]), Single.Parse(normalsTokens[i][(j * 3) + 1]), Single.Parse(normalsTokens[i][(j * 3) + 2]));
					distances[i][j] = Single.Parse(distancesTokens[i][j]);
					alphas[i][j] = Single.Parse(alphasTokens[i][j]);
				}
			}

		}

	}
}
