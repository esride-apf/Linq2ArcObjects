using System.Collections.Generic;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;

namespace EsriDE.Samples.LinqToArcObjects.Prototype
{
	public static class FeatureClassExtensions
	{
		public static IEnumerable<IFeature> GetFeatures(this IFeatureClass featureClass, RecyclingPolicy policy)
		{
			return featureClass.GetFeatures(null, policy);
		}

		public static IEnumerable<IFeature> GetFeatures(this IFeatureClass featureClass,
		                                                IQueryFilter queryFilter, RecyclingPolicy policy)
		{
			IFeatureCursor featureCursor =
				featureClass.Search(queryFilter, RecyclingPolicy.Recycle == policy);

			IFeature feature;
			while (null != (feature = featureCursor.NextFeature()))
			{
				yield return feature;
			}

			//this is skipped in unit tests with cursor-mock
			if (Marshal.IsComObject(featureCursor))
			{
				Marshal.ReleaseComObject(featureCursor);
			}
		}
	}
}