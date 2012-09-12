using System.Collections;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;

namespace EsriDE.Samples.LinqToArcObjects.Prototype
{
	public class FeatureEnumerable : IEnumerable<IFeature>
	{
		private readonly IFeatureClass _featureClass;
		private readonly IQueryFilter _filter;
		private readonly RecyclingPolicy _policy;

		public FeatureEnumerable(IFeatureClass featureClass, IQueryFilter filter, RecyclingPolicy policy)
		{
			_featureClass = featureClass;
			_filter = filter;
			_policy = policy;
		}

		#region IEnumerable<IFeature> Members
		public IEnumerator<IFeature> GetEnumerator()
		{
			var isRecyclingCursor = RecyclingPolicy.Recycle == _policy;

			return
				new FeatureEnumerator<IFeatureClass>(
					_featureClass,
					featureClass => featureClass.Search(_filter, isRecyclingCursor));
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}
}