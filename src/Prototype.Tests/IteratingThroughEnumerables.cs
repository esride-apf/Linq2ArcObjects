using System;
using System.Collections.Generic;
using System.Linq;
using ESRI.ArcGIS.Geodatabase;
using NUnit.Framework;

namespace EsriDE.Samples.LinqToArcObjects.Prototype.Tests
{
	public partial class LearningTest
	{
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

		[Test]
		public void UnderstandingIteratingThroughEnumerablesTest()
		{
			var features = _featureClass.GetFeatures(RecyclingPolicy.DoNotRecycle);

			var timestamp = DateTime.Now;
			var timestampToSave = timestamp.ToShortTimeString();

			SavingTimestamps(features, timestampToSave);
			VerifyingSavedTimestamps(features, timestampToSave);
		}
	}
}