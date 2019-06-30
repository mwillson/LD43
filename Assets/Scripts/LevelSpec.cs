using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelSpec", menuName = "Level Spec", order = 51)]
public class LevelSpec : ScriptableObject
{
    [SerializeField]
    private int levelNum;
    [SerializeField]
    private int baseRemoveNum;
    [SerializeField]
    private float wallSpeed;
    [SerializeField]
    private float bps;
    [SerializeField]
    private int polyRangeLow;
    [SerializeField]
    private int polyRangeHigh;
    [SerializeField]
    private int nextLevelThreshold;

    public int LevelNum
    {
        get { return levelNum; }
        set { levelNum = value; }
    }

    public int BaseRemoveNum
    {
        get { return baseRemoveNum; }
        set { baseRemoveNum = value; }
    }

    public float WallSpeed
    {
        get { return wallSpeed; }
        set { wallSpeed = value; }
    }

    public float BeatsPerSecond
    {
        get { return bps; }
        set { bps = value; }
    }

    public int PolyRangeLow
    {
        get { return polyRangeLow; }
        set { polyRangeLow = value; }
    }

    public int PolyRangeHigh
    {
        get { return polyRangeHigh; }
        set { polyRangeHigh = value; }
    }

    public int NextLevelThreshold
    {
        get { return nextLevelThreshold; }
        set { nextLevelThreshold = value; }
    }

}
