using UnityEngine;
using System.Collections;

public class PuzzleStateTrigger : MonoBehaviour {


    public delegate void PuzzleStateTriggerDelegate (GameObject sender);
    public event PuzzleStateTriggerDelegate OnStateMeet;

    public PuzzleStateModel[] stateRequirement;
    public PuzzleStateModel[] stateToApply;

    public void Trigger(bool force = false)
    {
        if(force || stateRequirement.Length == 0 || PuzzleStateManager.shared().CompareWithState(stateRequirement))
        {
            PuzzleStateManager.shared().SetPuzzleState(stateToApply);
            if(OnStateMeet!=null)
                OnStateMeet(gameObject);
//            print(PuzzleStateManager.shared().ToString());

        }
    }
}
