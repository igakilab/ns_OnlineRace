using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class ReSpawnPoint : MonoBehaviour
{

    Vector2[] positions = new Vector2[6] {
        new Vector2(0, 0),
        new Vector2(40, 2.5f),
        new Vector2(63, -3),
        new Vector2(133, -3),
        new Vector2(195, 4.4f),
        new Vector2(255, -3.5f)
    };

    public Vector2 getPosition(Vector2 pos)
    {
        int index = 0;
        for (int i = 1; i < positions.Length; i++)
        {
            if (positions[i].x < pos.x && pos.x - positions[index].x >= pos.x - positions[i].x)
            {
                index = i;
            }
        }
        return positions[index];
    }
}
