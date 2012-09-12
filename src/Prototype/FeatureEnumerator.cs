using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;

namespace EsriDE.Samples.LinqToArcObjects.Prototype
{
	public class FeatureEnumerator<T> : IEnumerator<IFeature>
	{
		private readonly Func<T, IFeatureCursor> _resetCursor;
		private readonly T _t;
		private IFeature _feature;
		private IFeatureCursor _featureCursor;

		public FeatureEnumerator(
			T t,
			Func<T, IFeatureCursor> resetCursor)
		{
			_t = t;
			_resetCursor = resetCursor;
			_featureCursor = _resetCursor(_t);
		}

		#region IEnumerator<IFeature> Members
		public void Dispose()
		{
			try
			{
				Marshal.ReleaseComObject(_featureCursor);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		public bool MoveNext()
		{
			_feature = _featureCursor.NextFeature();
			return (null != _feature);
		}

		public void Reset()
		{
			_featureCursor = _resetCursor(_t);
		}

		public IFeature Current
		{
			get { return _feature; }
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}
		#endregion
	}
}