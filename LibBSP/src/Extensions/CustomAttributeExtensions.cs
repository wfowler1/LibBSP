#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)

using System;
using System.Reflection;

namespace LibBSP {
	public static class CustomAttributeExtensions {

		public static T GetCustomAttribute<T>(this MemberInfo element) where T : Attribute {
			return Attribute.GetCustomAttribute(element, typeof(T)) as T;
		}

	}
}

#endif
