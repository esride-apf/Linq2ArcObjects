using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ESRI.ArcGIS.Geodatabase;
using NUnit.Framework;

namespace EsriDE.Samples.LinqToArcObjects.Prototype.Tests
{
	public partial class LearningTest
	{
		[Test]
		public void UnderstandingConcatenatedEnumerablesTest()
		{
			IEnumerable<IFeature> features =
				_featureClass.GetFeatures(RecyclingPolicy.Recycle);

			//getting only the large features (three pieces (OID=3, 4, 5)
			IEnumerable<IFeature> largeFeatures =
				from feature in features where (Convert.ToDouble(feature.Value[4]) > 3000) select feature;

			largeFeatures.ToList().
				ForEach(feature => Debug.WriteLine(feature.OID));
			largeFeatures.ToList().ForEach(feature => Debug.WriteLine(feature.OID));

			//accessing the interesting part of the above block and select OID=4
			var evenFeatures =
				from feature in largeFeatures where (feature.OID%2 == 0) select feature;
			evenFeatures.ToList().ForEach(feature => Debug.WriteLine(feature.OID));
		}

		[Test]
		public void UnderstandingConcatenatedEnumerablesTest2()
		{
			IEnumerable<IFeature> features = _featureClass.GetFeatures(RecyclingPolicy.DoNotRecycle);

			//getting only the large features (three pieces (OID=3, 4, 5)
			/*var largeFeatures =
				features.Where(feature =>
					(feature.GetValue("SHAPE_Area").ToDouble() > 3000));*/
			var largeFeatures =
				from feature in features
				where (feature.GetValue("SHAPE_Area").ToDouble() > 3000)
				select feature;
			largeFeatures.ToList().ForEach(feature => Debug.WriteLine(feature.OID));

			//accessing the interesting part of the above block and select OID=4
			var evenFeatures =
				from feature in largeFeatures where (feature.OID%2 == 0) select feature;
			evenFeatures.ToList().ForEach(feature => Debug.WriteLine(feature.OID));
		}
	}

	internal static class Extensions
	{
		internal static object GetValue(this IFeature feature, string fieldName)
		{
			var fields = feature.Fields;
			var index = fields.FindField(fieldName);
			return feature.Value[index];
		}

		internal static double ToDouble(this object value)
		{
			var result = Convert.ToDouble(value);
			return result;
		}
	}
}
