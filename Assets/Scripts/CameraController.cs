using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    


    public void changeCameraPosition(Coord playerPosition, bool hasLanded)
    {
        if (!hasLanded) return; 
        float x = playerPosition.x;
        float y = playerPosition.y;

        cam.transform.position = new Vector3(x, y, -1); 
    }

}

// x = 056    y = 1.076   z = -1