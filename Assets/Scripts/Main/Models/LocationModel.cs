using UnityEngine;
using System.Collections;

[System.Serializable]
public class LocationModel
{
	public string sceneName;
	public LocationType location;
//    public LocationModel()
//    {
//        
//    }
//
    public LocationModel(string sceneName, LocationType location)
    {
        this.sceneName = sceneName;
        this.location = location;
    }

}