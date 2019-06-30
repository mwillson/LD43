using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    //is the game paused?
    public bool paused;

    public int numToRemove = 1, score;

    public bool cameraWait = false;

    public bool bpsChanged = false;
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

    public abstract bool VerticesAreSame(TestPolygon p1, TestPolygon p2);
    public abstract void UpdateRemoveText();
    public abstract void WallCycle();
    public abstract void ChangeTexture1(Texture newTex);
}
