using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerPushInJig : MonoBehaviour
{
    List<List<Vector3>> polygons;

    public TestPolygon thePolygon;

    public Color[] playerPolyColors;

    public float loopTicker, prevLoopTicker, loopRate = 2f, yPos, startingXPos, endXPos;

    // Start is called before the first frame update
    void Start()
    {
        polygons = new List<List<Vector3>>();
        Vector3[] shape1 = new Vector3[] {
            new Vector3 (0f, 0f, 0f),
            new Vector3 (0f, 1f, 0f),
            new Vector3 (1f, 1f, 0f),
            new Vector3 (1f, 0f, 0f)
        };
        polygons.Add(new List<Vector3>(shape1));
        Vector3[] shape2 = new Vector3[] {
            new Vector3 (0f, 0f, 0f),
            new Vector3 (1f, 2f, 0f),
            new Vector3 (1f, 0f, 0f),
            new Vector3 (0f, -1f, 0f)
        };
        polygons.Add(new List<Vector3>(shape2));
        Vector3[] shape3 = new Vector3[] {
            new Vector3 (0f, 0f, 0f),
            new Vector3 (1f, 1f, 0f),
            new Vector3 (2f, 1f, 0f),
            new Vector3 (1f, 0f, 0f)
        };
        polygons.Add(new List<Vector3>(shape3));
        Vector3[] shape4 = new Vector3[] {
            new Vector3 (-1f, 0f, 0f),
            new Vector3 (-.5f, 1f, 0f),
            new Vector3 (.5f, 1f, 0f),
            new Vector3 (1f, 0f, 0f)
        };

        //5 sided
        polygons.Add(new List<Vector3>(shape4));
        Vector3[] shape5 = new Vector3[] {
            new Vector3 (0f, -.5f, 0f),
            new Vector3 (-.5f, 0f, 0f),
            new Vector3 (0f, 1f, 0f),
            new Vector3 (1f, 0f, 0f),
            new Vector3(.5f, -.5f, 0f)
        };
        polygons.Add(new List<Vector3>(shape5));
        Vector3[] shape6 = new Vector3[] {
            new Vector3 (0f, -.5f, 0f),
            new Vector3 (-.5f, 0f, 0f),
            new Vector3 (-.5f, .5f, 0f),
            new Vector3 (0f, 1f, 0f),
            new Vector3(.5f, 0f, 0f)
        };
        polygons.Add(new List<Vector3>(shape6));
        Vector3[] shape7 = new Vector3[] {
            new Vector3 (0f, 0f, 0f),
            new Vector3 (-.5f, .5f, 0f),
            new Vector3 (.5f, 1f, 0f),
            new Vector3 (1f, .5f, 0f),
            new Vector3(.5f, 0f, 0f)
        };
        polygons.Add(new List<Vector3>(shape7));
        Vector3[] shape8 = new Vector3[] {
            new Vector3 (0f, -.5f, 0f),
            new Vector3 (-.5f, 0f, 0f),
            new Vector3 (0f, .5f, 0f),
            new Vector3 (1f, 0f, 0f),
            new Vector3(0f, 0f, 0f)
        };
        polygons.Add(new List<Vector3>(shape8));

        //6 sided
        Vector3[] shape9 = new Vector3[] {
            new Vector3 (0f, 0f, 0f),
            new Vector3 (-.5f, 0f, 0f),
            new Vector3 (0f, 1f, 0f),
            new Vector3 (1f, .5f, 0f),
            new Vector3(.5f, .5f, 0f),
            new Vector3(1f,-.5f,0f)
        };
        polygons.Add(new List<Vector3>(shape9));
        Vector3[] shape10 = new Vector3[] {
            new Vector3 (0f, -.5f, 0f),
            new Vector3 (-.5f, .5f, 0f),
            new Vector3 (0f, 1f, 0f),
            new Vector3 (.5f, 1f, 0f),
            new Vector3(1f, .5f, 0f),
            new Vector3(.5f,-.5f,0f)
        };
        polygons.Add(new List<Vector3>(shape10));
        Vector3[] shape11 = new Vector3[] {
            new Vector3 (-.5f, -0f, 0f),
            new Vector3 (0f, .5f, 0f),
            new Vector3 (0f, 1f, 0f),
            new Vector3 (.5f, 1f, 0f),
            new Vector3(.5f, 0f, 0f),
            new Vector3(1f,-.5f,0f)
        };
        polygons.Add(new List<Vector3>(shape11));
        Vector3[] shape12 = new Vector3[] {
            new Vector3 (-.5f, 0f, 0f),
            new Vector3 (-.5f, .5f, 0f),
            new Vector3 (0f, 1f, 0f),
            new Vector3 (1f, 1f, 0f),
            new Vector3(1f, -.5f, 0f),
            new Vector3(.5f,-.5f,0f)
        };
        polygons.Add(new List<Vector3>(shape12));

        thePolygon.Setup();
        ChangePolygon();
        StartCoroutine(PushIn());
    }

    // Update is called once per frame
    void Update()
    {
        loopTicker = Time.time % loopRate;
        if (loopTicker < prevLoopTicker)
        {
            //did a loop
            ChangePolygon();
        }
        prevLoopTicker = loopTicker;

    }

    IEnumerator PushIn()
    {
        float xPos = startingXPos;
        while (xPos > endXPos)
        {
            xPos += -.01f;
            loopRate -= .003f;
            thePolygon.transform.position = new Vector3(xPos, yPos, 0f);
            yield return null;
        }
        while (loopRate > 0f) { 
            loopRate -= .002f;
            yield return null;
        }
        Destroy(thePolygon.gameObject);
        yield return null;
    }

    void ChangePolygon()
    {
        //newVerts needs to be a new List of vertices, copied from the list in memory
        //polyRangeHigh should never exceed polygons.count
        List<Vector3> newVerts = new List<Vector3>(polygons[Random.Range(0, polygons.Count-1)]);
        thePolygon.verticesList = newVerts;
        //THIS LINE WILL CHANGE WHEN DIFFERENT REMOVAL NUMS FOR DIFFERENT POLYGONS IS A THING
        //thePolygon.numToRemove = currentLevel.BaseRemoveNum;
        thePolygon.ResetRemovedStack();
        thePolygon.vertices2D = System.Array.ConvertAll<Vector3, Vector2>(newVerts.ToArray(), v => v);
        thePolygon.color = playerPolyColors[UnityEngine.Random.Range(0, playerPolyColors.Length)];
        thePolygon.OutlineFadeInEffect();
    }
}
