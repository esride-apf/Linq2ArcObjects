using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;
using NUnit.Framework;

namespace EsriDE.Samples.LinqToArcObjects.Prototype.Tests
{
	public abstract class ArcObjectsBaseFixture
	{
		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			BindProductCode();
			InitializeLicense();
		}

		protected virtual void BindProductCode()
		{
			RuntimeManager.Bind(ProductCode.EngineOrDesktop);
		}

		protected virtual void InitializeLicense()
		{
			InitializeEngineLicense();
		}

		private static void InitializeEngineLicense()
		{
			AoInitialize aoInitialize = new AoInitializeClass();

			const esriLicenseProductCode productCode = esriLicenseProductCode.esriLicenseProductCodeArcInfo;
			if (aoInitialize.IsProductCodeAvailable(productCode) == esriLicenseStatus.esriLicenseAvailable)
			{
				aoInitialize.Initialize(productCode);
			}
		}
	}
}