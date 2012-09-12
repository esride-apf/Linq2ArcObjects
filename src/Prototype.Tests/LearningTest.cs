using ESRI.ArcGIS.Geodatabase;
using NUnit.Framework;

namespace EsriDE.Samples.LinqToArcObjects.Prototype.Tests
{
	[TestFixture]
	public partial class LearningTest : ArcObjectsBaseFixture
	{
		#region Setup/Teardown
		[SetUp]
		public void Setup()
		{
			_featureClass = TestUtils.OpenSampleFeatureClass("Polygons");
		}
		#endregion

		private IFeatureClass _featureClass;
	}
}