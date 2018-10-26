using UnityEngine;
using System.Collections;

public class ConditionalSavePointTrigger : SavePointTrigger
{

    public PuzzleStateTrigger puzzleStateTrigger;

    public override void Awake()
    {
        base.Awake();
        if (puzzleStateTrigger != null)
            puzzleStateTrigger.OnStateMeet += PuzzleStateTrigger_OnStateMeet;
    }

    void PuzzleStateTrigger_OnStateMeet(GameObject sender)
    {
        TriggerSave();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (puzzleStateTrigger != null)
            {
                puzzleStateTrigger.Trigger(); 
            }
            else
            {
                TriggerSave();    
            }
        }
    }

    void OnDestroy()
    {
        if (puzzleStateTrigger != null)
            puzzleStateTrigger.OnStateMeet -= PuzzleStateTrigger_OnStateMeet;
    }
}
