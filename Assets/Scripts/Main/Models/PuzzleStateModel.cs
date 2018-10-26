using UnityEngine;
using System.Collections;

public enum PuzzleStateType
{
    //# = XYYZZ
    /*
     * X = Area
     * YY = Puzzle Stage
     * 0Z = Save point 
     * 1Z = other state 
    */

    A1P01_Start = 10100,
    A1P02_Start = 10200,
    A1P05_Start = 10500,
    A1P07_Start = 10700,
    A1P09_Start = 10900,
    A1P10_Start = 11000,
    A1P16_Start = 11600,
    A1P17_Start = 11700,
    A1P18_Start = 11800,
    A1P19_Start = 11900,
    A1P20_Start = 12000,
    A1P21_Start = 12100,
    A1P22_Start = 12200,
    A1P23_Start = 12300,
    A1P24_Start = 12400,


}

[System.Serializable]
public class PuzzleStateModel{

    public PuzzleStateType state;
    public bool value;

    public override string ToString()
    {
        return (value?"[V]":"[ ]") + "\t" + state.ToString() + "\n";
    }
}
