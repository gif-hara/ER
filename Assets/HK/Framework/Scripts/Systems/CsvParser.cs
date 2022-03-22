using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System;
using System.Linq;

namespace HK.Framework.Systems
{
	/// <summary>
	/// .
	/// </summary>
	public class CsvParser
	{
		public static List<T> Parse<T>(TextAsset textAsset, Func<List<string>, T> parser) where T : class
		{
			Assert.IsNotNull(textAsset);
			Assert.IsNotNull(parser);
			return textAsset.text.Trim().Replace("\r", "").Split('\n').Where((t, i) => i != 0).Select(t => parser(t.Split(',').ToList())).ToList();
		}

		public static List<string[]> Split(TextAsset textAsset)
		{
			Assert.IsNotNull(textAsset);
			return textAsset.text.Trim().Replace("\r", "").Split('\n').Where((t, i) => i != 0).Select(t => t.Split(',')).ToList();
		}
	}
}