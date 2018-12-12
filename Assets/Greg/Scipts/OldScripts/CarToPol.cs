using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarToPol : MonoBehaviour {

    //convert polar (azimuth,elevation) to cartesian
    public Vector3 PolarToCartesian(Vector2 polarAngle)
    {
        Vector3 origin = new Vector3(0, 0, 1); //unit vector to rotate
        Quaternion rotation = Quaternion.Euler(-polarAngle.y, polarAngle.x, 0); //convert euler to rotation
        Vector3 point = rotation * origin; //calculate cartesian coordinate of speaker
        return point; //return a Vector3
    }
}
