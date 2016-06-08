using System;
using System.Linq;
using System.Collections.Generic;

namespace WebPageParser {
	internal static class Utils {
		public static T GetCustomAttribute<T>(this System.Reflection.MemberInfo inf) where T : Attribute {
			return (T)inf.GetCustomAttributes(typeof(T), true).FirstOrDefault();
		}

		public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source){
			return source.Where(p => p != null);
		}

		public static IEnumerable<TResult> NotNullMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> f) {
			return source.Select(f).Where(p=>p!=null).SelectMany(p=>p);
		}
	}
}
