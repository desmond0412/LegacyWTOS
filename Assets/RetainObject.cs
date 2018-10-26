using UnityEngine;
using System.Collections;

public class RetainObject : MonoBehaviour {

	// Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
