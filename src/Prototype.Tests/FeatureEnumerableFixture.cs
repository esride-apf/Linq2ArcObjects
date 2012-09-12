using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ESRI.ArcGIS.Geodatabase;
using NUnit.Framework;

namespace EsriDE.Samples.LinqToArcObjects.Prototype.Tests
{
	[TestFixture]
	public class FeatureEnumerableFixture : ArcObjectsBaseFixture
	{
		private IFeatureClass _featureClass;

		#region Setup/Teardown
		[SetUp]
		public void Setup()
		{
			_featureClass = TestUtils.OpenSampleFeatureClass("Polygons");
		}
		#endregion

		[Test]
		public void WritingTimestamp_ByMultipleEnumerableCalls_WritesTimestamp()
		{
			var features = new FeatureEnumerable(_featureClass, null, RecyclingPolicy.Recycle);

			foreach (var feature in features)
			{
				Debug.WriteLine(feature.OID);
			}

			foreach (var feature in features)
			{
				Debug.WriteLine(feature.OID);
			}

			features.ToList().ForEach(feature => Debug.WriteLine(feature.OID));

			var timestamp = DateTime.Now;
			var timestampToSave = timestamp.ToShortTimeString();

			SavingTimestamps(features, timestampToSave);
			VerifyingSavedTimestamps(features, timestampToSave);
		}

		private static void VerifyingSavedTimestamps(IEnumerable<IFeature> features, string estimatedTimestamp)
		{
			var featureList = features.ToList();
			foreach (var feature in featureList)
			{
				var readedTimestamp = feature.Value[2].ToString();
				Console.WriteLine(string.Format("Feature: Oid={0}, TimeStamp={1}", feature.OID, readedTimestamp));
				Assert.That(readedTimestamp, Is.EqualTo(estimatedTimestamp));
			}
		}

		private static void SavingTimestamps(IEnumerable<IFeature> features, string timestamp)
		{
			foreach (var feature in features)
			{
				feature.Value[2] = timestamp;
				feature.Store();
			}
		}

		
	}
}