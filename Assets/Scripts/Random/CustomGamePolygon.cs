using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGamePolygon : MonoBehaviour
{
    [SerializeField]
    TestPolygon thePolygon;

    [SerializeField]
    List<Vector3> shapeVectorList;

    [SerializeField]
    Color[] playerPolyColors;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] shape1 = new Vector3[] {
            new Vector3 (0f, 0f, 0f),
            new Vector3 (0f, 1f, 0f),
            new Vector3 (1f, 1f, 0f),
            new Vector3 (1f, 0f, 0f)
        };
        shapeVectorList = new List<Vector3>(shape1);

        thePolygon.Setup();
        ChangePolygon(new int[] {1,4,4,1});

        NumberSelector.onNumberChanged += ChangeBasedOnNewNums;
    }

    private void OnDisable()
    {
        NumberSelector.onNumberChanged -= ChangeBasedOnNewNums;
    }

    void ChangePolygon(int[] values)
    {
        int i = 0;
        Vector3[] newVerts = new Vector3[4];
        foreach(int b in values)
        {
            float c = b * .5f;
            //bottom left
            if (i == 0) newVerts[0] = new Vector3((-1*c), (-1*c), 0f);
            //top left
            if (i == 1) newVerts[1] = new Vector3((-1 * c), c, 0f);
            //top right
            if (i == 2) newVerts[2] = new Vector3(c, c, 0f);
            //bottom right
            if (i == 3) newVerts[3] = new Vector3(c, (-1*c), 0f);

            i += 1;
        }
        shapeVectorList = new List<Vector3>(newVerts);
        thePolygon.verticesList = shapeVectorList;
        thePolygon.ResetRemovedStack();
        thePolygon.vertices2D = System.Array.ConvertAll<Vector3, Vector2>(shapeVectorList.ToArray(), v => v);
        thePolygon.color = playerPolyColors[UnityEngine.Random.Range(0, playerPolyColors.Length)];
        thePolygon.OutlineFadeInEffect();
    }

    void ChangeBasedOnNewNums()
    {
        int num1 = GameObject.Find("numshapes").GetComponentInChildren<NumberSelector>().current;
        int num2 = GameObject.Find("sideslow").GetComponentInChildren<NumberSelector>().current;
        int num3 = GameObject.Find("sideshigh").GetComponentInChildren<NumberSelector>().current;
        int num4 = GameObject.Find("removalsper").GetComponentInChildren<NumberSelector>().current;

        ChangePolygon(new int[] { num1, num2, num3, num4 });
    }

}
