using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Greg
{
    public struct GeneratePoints
    {
        static public void RandomPointsOnSphere(float3 center, float radius, ref NativeArray<float3> points)
        {
            //var radiusSquared = radius * radius;
            var pointsFound = 0;
            var count = points.Length;
            while (pointsFound < count)
            {
                var p = new Vector3
                {
                    x = UnityEngine.Random.Range(-1f, 1f),
                    y = UnityEngine.Random.Range(-1f, 1f),
                    z = UnityEngine.Random.Range(-1f, 1f)
                };
                if (math.lengthsq(p) < 1)
                {
                    p = (p.normalized) * radius;

                    float3 point = (float3) p;
                    points[pointsFound] = point;

                    //transform to onto circle with radius r


                    pointsFound++;
                }
            }
        }

        static public float3[] RandomPointsOnSphereFloat3(float3 center, float radius, float3[] points)
        {
            //var radiusSquared = radius * radius;
            var pointsFound = 0;
            var count = points.Length;
            while (pointsFound < count)
            {
                var p = new Vector3
                {
                    x = UnityEngine.Random.Range(-1f, 1f),
                    y = UnityEngine.Random.Range(-1f, 1f),
                    z = UnityEngine.Random.Range(-1f, 1f)
                };
                if (math.lengthsq(p) < 1)
                {
                    p = (p.normalized) * radius;

                    float3 point = (float3)p;
                    points[pointsFound] = point;

                    //transform to onto circle with radius r


                    pointsFound++;
                }
            }
            return points;
        }


        static public float3 RandomPointOnSphere()
        {
            var pointFound = false;
            float3 newPos = new float3(0, 0, 0);
            //var count = points.Length;
            
            while (pointFound == false)
            {
                var p = new Vector3
                {
                    x = UnityEngine.Random.Range(-1f, 1f),
                    y = UnityEngine.Random.Range(-1f, 1f),
                    z = UnityEngine.Random.Range(-1f, 1f)
                };
                if (math.lengthsq(p) < 1)
                {
                    p = (p.normalized);

                    newPos = (float3) p;
                    pointFound = true;
                    //points[pointsFound] = point;

                    //transform to onto circle with radius r


                    //pointsFound++;
                }
            }
            return newPos;       
        }



        static public void GenerateRing(float3 center, float radius, float zRot , ref NativeArray<float3> points)
        {

            var count = points.Length;
            float increment = Mathf.PI * 2.0f / count; //increment is 2*pi / numPartitions
            float angle = 0;

            //float increment = Mathf.PI * 2.0f / count; //increment is 2*pi / numPartitions
            //float zRot = 0;


            //var rotationVec = new Vector3(0, 0, 1);  //unit vector in z direction
            //rotationVect = Quaternion.Euler(0, -45, angle) * rotationVec;

            //rotationVec.
            //var radiusSquared = radius * radius;
            //var pointsFound = 0;


            //float angleOffset = 0;

            for (int i = 0; i < count; i++)
            {

                points[i] = center + new float3
                {
                    x = math.sin(angle) * radius,
                    y = math.cos(angle) * radius,
                    z = 0
                };

                angle = angle + increment;
            }


        }

        static public void GenerateWorldRing(float3 center, float radius, Quaternion ringRotation, ref NativeArray<float3> points)
        {
            var count = points.Length;
            float increment = Mathf.PI * 2.0f / count; //increment is 2*pi / numPartitions
            float angle = 0;

            for (int i = 0; i < count; i++)
            {
                points[i] = new float3
                {
                    x = math.sin(angle) * radius,
                    y = 0,
                    z = math.cos(angle) * radius
                };

                points[i] = ringRotation * points[i];
                angle = angle + increment;
            }
        }

    }
}

