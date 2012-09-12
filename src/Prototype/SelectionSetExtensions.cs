using System.Collections.Generic;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;

namespace EsriDE.Samples.LinqToArcObjects.Prototype
{
	public static class SelectionSetExtensions
	{
		public static IEnumerable<IFeature> GetFeatures(this ISelectionSet2 set, IQueryFilter filter, RecyclingPolicy policy)
		{
			ICursor cursor;
			set.Search(filter, RecyclingPolicy.Recycle == policy, out cursor);
			var featureCursor = (IFeatureCursor) cursor;

			IFeature feature;
			while (null != (feature = featureCursor.NextFeature()))
			{
				yield return feature;
			}

			//this is skipped in unit test with cursor-mock
			if (Marshal.IsComObject(cursor))
			{
				Marshal.ReleaseComObject(cursor);
			}
		}
	}
}