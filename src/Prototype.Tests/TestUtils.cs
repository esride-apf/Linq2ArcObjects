using System;
using System.IO;
using System.Reflection;
using System.Text;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;

namespace EsriDE.Samples.LinqToArcObjects.Prototype.Tests
{
	internal static class TestUtils
	{
		private const string ShadowCopiedAssemblyInfoIni = "__AssemblyInfo__.ini";
		private const string TestGdbFilename = @"LinqToArcObjects.mdb";

		public static IFeatureClass OpenSampleFeatureClass(string featureClassName)
		{
			try
			{
				var workspace = GetWorkspace();
				var featureWorkspace = (IFeatureWorkspace) workspace;
				var featureClass = featureWorkspace.OpenFeatureClass(featureClassName);
				return featureClass;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
				throw;
			}
		}

		private static IWorkspace GetWorkspace()
		{
			try
			{
				var fullFilename = GetFullFilenameForTestMdb();

				var connectionString = string.Format("DATABASE={0}", fullFilename);

				var workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
				var workspace = workspaceFactory.OpenFromString(connectionString, 0);

				return workspace;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
				throw;
			}
		}

		private static string GetFullFilenameForTestMdb()
		{
			var result = GetFullFilenameIfNoShadowCopied();

			if (!File.Exists(result))
			{
				result = GetFullFilenameIfShadowCopied();
			}

			if (!File.Exists(result))
			{
				throw new FileNotFoundException(string.Format("Personal GDB '{0}' für Tests nicht gefunden.", TestGdbFilename));
			}

			return result;
		}

		private static string GetFullFilenameIfNoShadowCopied()
		{
			var currentAssemblyDir = GetPathFromExecutingAssembly();
			return string.Format(@"{0}\{1}", currentAssemblyDir, TestGdbFilename);
		}

		private static string GetFullFilenameIfShadowCopied()
		{
			var currentAssemblyDir = GetPathFromExecutingAssembly();
			var iniFilePath = Path.Combine(currentAssemblyDir, ShadowCopiedAssemblyInfoIni);

			var originalAssemblyPath = ExtractOriginalAssemblyPath(iniFilePath);
			var originalDir = Path.GetDirectoryName(originalAssemblyPath);

			return string.Format(@"{0}\{1}", originalDir, TestGdbFilename);
		}

		private static string ExtractOriginalAssemblyPath(string iniFilePath)
		{
			var iniFileContent = File.ReadAllText(iniFilePath, Encoding.Unicode);

			return iniFileContent
				.Remove(
					0,
					iniFileContent
						.IndexOf(
							"file:///",
							StringComparison.OrdinalIgnoreCase)
					+ "file:///".Length)
				.TrimEnd(' ', '\0');
		}

		private static string GetPathFromExecutingAssembly()
		{
			var assemblyLocation = GetAssemblyLocation();
			return Path.GetDirectoryName(assemblyLocation);
		}

		private static string GetAssemblyLocation()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var location = assembly.Location;
			return location;
		}
	}
}