using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
	public class AttributesExamples : MonoBehaviour
	{
		[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
		public class ShowIfAttribute : PropertyAttribute
		{
			public string _BaseCondition
			{
				get { return mBaseCondition; }
			}

			private string mBaseCondition = String.Empty;

			public ShowIfAttribute(string baseCondition)
			{
				mBaseCondition = baseCondition;
			}
		}



		[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
		public class BigHeaderAttribute : PropertyAttribute
		{
			public string _Text
			{
				get { return mText; }
			}

			private string mText = String.Empty;

			public BigHeaderAttribute(string text)
			{
				mText = text;
			}
		}
	}
}