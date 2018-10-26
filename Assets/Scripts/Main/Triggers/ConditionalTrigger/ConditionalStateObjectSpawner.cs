using UnityEngine;
using System.Collections;

public class ConditionalStateObjectSpawner : MonoBehaviour {

    public PuzzleStateTrigger puzzleStateTrigger;
//    public bool isTriggerOnStart = false;

	void Awake () {
        Trigger();
	}

    void Trigger()
    {
        if(puzzleStateTrigger.stateRequirement.Length == 0 || PuzzleStateManager.shared().CompareWithState(puzzleStateTrigger.stateRequirement))
        {
            this.gameObject.SetActive (true);
        } else {
            this.gameObject.SetActive (false);
        }
    }

}
