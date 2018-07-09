#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER)
#define UNITY
#endif

#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if !UNITY
	using Vector2 = Vector2d;
#endif
	/// <summary>
	/// Static class containing helper methods for <see cref="Rect"/> objects.
	/// </summary>
	public static class RectExtensions {
		
		/// <summary>
		/// Determines if this <see cref="Rect"/> is intersected by the line segment defined by <paramref name="head"/> and <paramref name="tail"/>.
		/// </summary>
		/// <remarks>
		/// Adapted from top answer at 
		/// http://stackoverflow.com/questions/99353/how-to-test-if-a-line-segment-intersects-an-axis-aligned-rectange-in-2d
		/// </remarks>
		/// <param name="rect">This <see cref="Rect"/>.</param>
		/// <param name="head">First point defining the line segment.</param>
		/// <param name="tail">The change and x and y for the coordinates of the tail.</param>
		/// <returns><c>true</c> if the line segment intersects this <see cref="Rect"/> at any point.</returns>
		public static bool IntersectsSegment(Rect rect, Vector2 head, Vector2 tail) {
			// Compute the signed distance from the line to each corner of the box
			double[] dist = new double[4];
			double x1 = head.x;
			double x2 = head.x + tail.x;
			double y1 = head.y;
			double y2 = head.y + tail.y;
			dist[0] = tail.y * rect.min.x + tail.x * rect.min.y + (x2 * y1 - x1 * y2);
			dist[1] = tail.y * rect.min.x + tail.x * rect.max.y + (x2 * y1 - x1 * y2);
			dist[2] = tail.y * rect.max.x + tail.x * rect.min.y + (x2 * y1 - x1 * y2);
			dist[3] = tail.y * rect.max.x + tail.x * rect.max.y + (x2 * y1 - x1 * y2);
			if (dist[0] >= 0 && dist[1] >= 0 && dist[2] >= 0 && dist[3] >= 0) {
				return false;
			} else {
				if (dist[0] <= 0 && dist[1] <= 0 && dist[2] <= 0 && dist[3] <= 0) {
					return false;
				} else {
					// If we get to this point, the line intersects the box. Figure out if the line SEGMENT actually cuts it.
					if ((x1 > rect.max.x && x2 > rect.max.x) || (x1 < rect.min.x && x2 < rect.min.x) || (y1 > rect.max.y && y2 > rect.max.y) || (y1 < rect.min.y && y2 < rect.min.y)) {
						return false;
					} else {
						return true;
					}
				}
			}
		}

	}
}
