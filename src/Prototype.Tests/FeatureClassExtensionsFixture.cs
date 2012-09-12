// ReSharper disable InconsistentNaming
using System;
using System.Linq;
using ESRI.ArcGIS.Geodatabase;
using NUnit.Framework;

namespace EsriDE.Samples.LinqToArcObjects.Prototype.Tests
{
	[TestFixture]
	public class FeatureClassExtensionsFixture : ArcObjectsBaseFixture
	{
		#region Setup/Teardown
		[SetUp]
		public void Setup()
		{
			_featureClass = TestUtils.OpenSampleFeatureClass("Polygons");
		}
		#endregion

		private IFeatureClass _featureClass;

		private void IterateFeatures(RecyclingPolicy policy)
		{
			var features = _featureClass.GetFeatures(policy);
			int consecutiveNumber = 1;
			foreach (var feature in features)
			{
				Assert.That(feature.OID, Is.EqualTo(consecutiveNumber));
				consecutiveNumber++;
			}
		}

		private int IteratingFeaturesTwice(RecyclingPolicy policy, Action<IFeature, int> action)
		{
			var features = _featureClass.GetFeatures(policy);

			var selectedFeatures =
				from feature in features where (Convert.ToDouble(feature.Value[4]) > 3000) select feature;

			var index = 0;

			//this is the first iteration which builds up a list of features
			var list = selectedFeatures.ToList();

			//this is the second iteration which uses this list
			list.ForEach(feature =>
			             	{
			             		action(feature, index);
			             		index++;
			             	});

			return index;
		}

		[Test]
		public void IteratingFeaturesTwice_WithRecycling_VisitsLastFeatureThreeTimes()
		{
			var count = IteratingFeaturesTwice(RecyclingPolicy.Recycle,
			                                   (feature, index) => Assert.That(feature.OID, Is.EqualTo(5)));

			Assert.That(count, Is.EqualTo(3));
		}

		[Test]
		public void IteratingFeaturesTwice_WithoutRecycling_VisitsThreeFeaturesOnceOnly()
		{
			var estimatedOids = new[] {3, 4, 5};
			var count = IteratingFeaturesTwice(RecyclingPolicy.DoNotRecycle, (feature, index) =>
			                                                                 	{
			                                                                 		var estimatedOid = estimatedOids[index];
			                                                                 		Assert.That(feature.OID, Is.EqualTo(estimatedOid));
			                                                                 	});

			Assert.That(count, Is.EqualTo(3));
		}

		[Test]
		public void IteratingFeatures_WithRecycling_VisitsEachFeature()
		{
			IterateFeatures(RecyclingPolicy.Recycle);
		}

		[Test]
		public void IteratingFeatures_WithoutRecycling_VisitsEachFeature()
		{
			IterateFeatures(RecyclingPolicy.DoNotRecycle);
		}
	}
}

// ReSharper restore InconsistentNaming