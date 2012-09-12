using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;

namespace EsriDE.Samples.LinqToArcObjects.Prototype
{
	public static class FeaturesExtensions
	{
		public static void WriteAuditInformations(this IEnumerable<IFeature> features)
		{
			var timestamp = DateTime.Now;
			var information = timestamp.ToShortTimeString();

			int? auditFieldIndex = null;
			foreach (var feature in features)
			{
				auditFieldIndex = auditFieldIndex ?? GetAuditFieldIndex(feature);
				if (0 == auditFieldIndex)
				{
					return;
				}

				feature.Value[auditFieldIndex.Value] = information;
				feature.Store();
			}
		}

		private static int GetAuditFieldIndex(IFeature feature)
		{
			var fields = feature.Fields;
			var index = fields.FindField("Audit");
			return index;
		}
	}
}