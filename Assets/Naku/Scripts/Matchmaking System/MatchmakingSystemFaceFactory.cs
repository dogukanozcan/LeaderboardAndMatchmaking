using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchmakingSystemFaceFactory : MonoBehaviour
{

    public List<Sprite> faces = new List<Sprite>();

    public Sprite GetNextRandomFace()
    {
        return faces[UnityEngine.Random.Range(0,faces.Count)];
    }
}
