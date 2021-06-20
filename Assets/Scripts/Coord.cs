using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Coord
{


    public float x = -1;
    public float y = -1;
    public float z = -1;

    public Coord()
    {
        this.x = -1;
        this.y = -1;
        this.z = -1;
    }

    public Coord(float x, float y, float z = -1)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

}

