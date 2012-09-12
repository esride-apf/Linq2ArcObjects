Abstract
========

Linq2ArcObjects contains a LINQ-provider for feature cursors and sets. It allows you to use LINQ-style and lambda syntax for accessing features.

One simple LINQ sample to get all features with an area above a threshold of 3000:

	var largeFeatures =
	    from feature in features
	    where (feature.GetValue("SHAPE_Area").ToDouble() > 3000)
	    select feature;
		
The same with a lambda expression:

	var largeFeatures =
		features.Where(feature => 
			(feature.GetValue("SHAPE_Area").ToDouble() > 3000));

This prototype has also some unit test to show you the usage of the constructs.

The whole documentation you could read in the wiki here.