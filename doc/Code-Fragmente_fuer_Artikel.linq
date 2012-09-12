<Query Kind="Statements" />

var largeFeatures =
	from feature in features
	where (feature.GetValue("SHAPE_Area").ToDouble() > 3000)
	select feature;

var largeFeatures =
	features.Where(feature => 
		(feature.GetValue("SHAPE_Area").ToDouble() > 3000));

public static IEnumerable<IFeature> GetFeatures(this IFeatureClass featureClass,
	IQueryFilter queryFilter, RecyclingPolicy policy)
{
	IFeatureCursor featureCursor =
	featureClass.Search(queryFilter, RecyclingPolicy.Recycle == policy);
 
	IFeature feature;
	while (null != (feature = featureCursor.NextFeature()))
	{
		yield return feature;
	}
 
	//this is skipped in unit tests with cursor-mock
	if (Marshal.IsComObject(featureCursor))
	{
		Marshal.ReleaseComObject(featureCursor);
	}
}

IEnumerable<IFeature> features = 
	_featureClass.GetFeatures(RecyclingPolicy.DoNotRecycle);


largeFeatures.ToList().
	ForEach(feature => Debug.WriteLine(feature.OID));


new FeatureEnumerator<IFeatureclass>(_featureClass,
	featureClass => featureClass.Search(_filter, isRecyclingCursor));

public void Reset() 
{
	_featureCursor = _resetCursor(_t); 
}