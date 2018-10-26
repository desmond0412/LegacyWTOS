using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Artoncode.Core;




public class PuzzleStateManager : Singleton<PuzzleStateManager> {

    Hashtable puzzleStates;

    public PuzzleStateManager()
    {
        Load();
    }

    public void Reset()
    {
        puzzleStates = new Hashtable();
        Save();
    }

    void Load()
    {
        puzzleStates = GameDataManager.shared().PlayerPuzzleStates;
        if(puzzleStates == null)
            puzzleStates = new Hashtable();
        
    }

    public void Save()
    {
        GameDataManager.shared().PlayerPuzzleStates = puzzleStates;
    }


    public void SetPuzzleState(PuzzleStateModel newState)
    {
        if(!puzzleStates.Contains(newState.state))
        {
            puzzleStates.Add(newState.state,newState);
        }
        puzzleStates[newState.state] = newState;
        Save();

    }

    public void SetPuzzleState(PuzzleStateModel[] newStates)
    {
        foreach (var state in newStates)
            SetPuzzleState(state);
    }


    public bool CompareWithState(PuzzleStateModel state)
    {
        if(state == null)
        {
            Debug.LogError("STATE IS NULL");
            return false;
        }
            
        if (puzzleStates.Contains(state.state)) {
            PuzzleStateModel currentState =  puzzleStates[state.state] as PuzzleStateModel;
            return (currentState.value == state.value);
        }

        return !state.value;
    }

    public bool CompareWithState(PuzzleStateModel[] states)
    {
        foreach (var state in states)
            if(!CompareWithState(state))
                return false;
        
        return true;
    }

    public override string ToString()
    {
        string str = "";
        str += "---PUZZLE STATE---\n";
        foreach (DictionaryEntry state in puzzleStates) {
            str += state.Value.ToString ();
        }
        return str;
    }


}


