using System.Collections;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;

namespace EsriDE.Samples.LinqToArcObjects.Prototype
{
	public class FeatureSelectionEnumerable : IEnumerable<IFeature>
	{
		private readonly IQueryFilter _queryFilter;
		private readonly ISelectionSet2 _selectionSet;

		public FeatureSelectionEnumerable(ISelectionSet2 selectionSet)
			: this(selectionSet, null)
		{
		}

		public FeatureSelectionEnumerable(ISelectionSet2 selectionSet, IQueryFilter queryFilter)
		{
			_selectionSet = selectionSet;
			_queryFilter = queryFilter;
		}

		#region IEnumerable<IFeature> Members
		public IEnumerator<IFeature> GetEnumerator()
		{
			ICursor cursor;
			_selectionSet.Search(_queryFilter, false, out cursor);
			ISelectionSet selectionSet = _selectionSet;
			return new FeatureEnumerator<ISelectionSet>(selectionSet,
			                                            delegate(ISelectionSet selectionSet2)
			                                            	{
			                                            		ICursor resettedCursor;
			                                            		selectionSet2.Search(null, false, out resettedCursor);
			                                            		var resettedFeatureCursor =
			                                            			(IFeatureCursor) resettedCursor;
			                                            		return resettedFeatureCursor;
			                                            	}
				);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}
}