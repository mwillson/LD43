using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager : MonoBehaviour
{

    //current player polygon(s)
    protected List<TestPolygon> currentPolys, currentHoles;

    //prefab for player polygon
    public GameObject polyPrefab, GameOverUI;

    //is the game paused?
    public bool paused;

    public int numToRemove = 1, score;

    public bool cameraWait = false;

    public bool bpsChanged = false;

    public int currentWallSlows = 0;

    //placeholder variable for when bps changes. we have to 
    //hold it somewhere until it is on time with a beat
    public float newBps;

    [SerializeField]
    public Texture world1Texture;

    [SerializeField]
    protected Texture world2Texture;

    [SerializeField]
    protected Texture world3Texture;

    public Color world1Text, world1Outline;
    public Color world2Text, world2Outline;
    public Color world3Text, world3Outline;

    [SerializeField]
    protected GameObject bonusSelector;

    public TestPolygon wallPoly;

    public PointerController pointerController;

    public abstract bool VerticesAreSame(TestPolygon p1, TestPolygon p2);
    public abstract void UpdateRemoveText();
    public abstract void WallCycle();
    public abstract void ChangeTexture1(Texture newTex);
}
