// ReSharper disable InconsistentNaming
using ESRI.ArcGIS.Geodatabase;
using NUnit.Framework;
using Rhino.Mocks;

namespace EsriDE.Samples.LinqToArcObjects.Prototype.Tests
{
	[TestFixture]
	public class FeaturesExtensionsFixture : ArcObjectsBaseFixture
	{
		// ReSharper disable PossibleInterfaceMemberAmbiguity
		public interface ICursorAndIFeatureCursor : ICursor, IFeatureCursor
		{
		}
		// ReSharper restore PossibleInterfaceMemberAmbiguity

		#region Setup/Teardown
		[SetUp]
		public void Setup()
		{
			_repository = new MockRepository();
		}
		#endregion

		private MockRepository _repository;

		[Test]
		public void WriteAuditInformations_ForSampleFeatures_IteratesOverTheseFeaturesAndWritesAuditInformations()
		{
			var featureClass = TestUtils.OpenSampleFeatureClass("Polygons");
			featureClass.GetFeatures(null, RecyclingPolicy.DoNotRecycle).WriteAuditInformations();
		}

		[Test]
		public void WriteAuditInformations_ForTwoMockedFeatures_IteratesOverTheseFeaturesAndWritesAuditInformations()
		{
			var selectionSet = _repository.StrictMock<ISelectionSet2>();
			var cursorAndIFeatureCursor = _repository.StrictMock<ICursorAndIFeatureCursor>();
			ICursor cursor = cursorAndIFeatureCursor;
			IFeatureCursor featureCursor = cursorAndIFeatureCursor;

			var featureOne = _repository.StrictMock<IFeature>();
			var featureTwo = _repository.StrictMock<IFeature>();

			var fields = _repository.DynamicMock<IFields>();

			SetupResult.For(featureOne.Fields).Return(fields);
			SetupResult.For(fields.FindField("Audit")).Return(6);

			using (_repository.Record())
			{
				Expect.Call(() => selectionSet.Search(null, false, out cursor)).OutRef(cursor);

				Expect.Call(featureCursor.NextFeature()).Return(featureOne);
				Expect.Call(() => featureOne.Value[6] = string.Empty).IgnoreArguments();
				Expect.Call(featureOne.Store);

				Expect.Call(featureCursor.NextFeature()).Return(featureTwo);
				Expect.Call(() => featureTwo.Value[6] = string.Empty).IgnoreArguments();
				Expect.Call(featureTwo.Store);

				Expect.Call(featureCursor.NextFeature()).Return(null);
			}

			using (_repository.Playback())
			{
				selectionSet.GetFeatures(null, RecyclingPolicy.DoNotRecycle).WriteAuditInformations();
			}
		}
	}
}
// ReSharper restore InconsistentNaming
