using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Greg
{
    public class ParameterManagerGreg : MonoBehaviour
    {

        public static ParameterManagerGreg instance = null;
        EntityManager em;

        private Vector3 worldOrigin;
        private int ringsPerSystem;
        private int spawnsPerRing;
        private int cubesPerRing;

        public float radialDistance;
        public Vector3 cartesianDistance;
        private float d1Radius = 1;
        private float d2Radius = 1;
        private float d3Radius = 1;

        private float d1LerpTarget = 1;
        private float d2LerpTarget = 1;
        private float d3LerpTarget = 1;

        private float d1LerpStart = 1;
        private float d2LerpStart = 1;
        private float d3LerpStart = 1;

        private float d1LerpTime = 1;
        private float d2LerpTime = 1;
        private float d3LerpTime = 1;

        // public GameObject worldPrefab; use to grab parameters for proper mapping
        public GameObject playerObj;
        private Transform playerT;


        void Awake()
        {
            //singleton stuffs
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            //grab the entity manager
            //em = World.Active.GetOrCreateManager<EntityManager>();

            //Sets this to not be destroyed when reloading scene
            //DontDestroyOnLoad(gameObject);

        }

        void Start()
        {
            worldOrigin = GameManagerGreg.instance.worldOrigin; //get the world origin for radius calc
            playerT = playerObj.transform;

            //grab stuff needed

        }

        public float GetRadialDistanceFromOrigin (Vector3 player ) {
            return (player - worldOrigin).magnitude;
        }

        public Vector3 GetCartesianDistanceFromOrigin(Vector3 player)
        {
            return (player - worldOrigin);
        }

        
        //TODO: NOTE THAT CHANGING BOTH THE RANDOM INIT VALUES AND THE INITIAL SPEED RATIOS
        //      IS IMPORTANT. Do more testing tomorrow.



        //public float[] 

        //scale the radial distance from 0 to 2
        public float D1BaseSpeedMultiplyer()
        {
            //return 0;
            return math.clamp(1 - (radialDistance / 800), .0001f, 1);
           // return math.clamp((.1f * radialDistance / 80), 0, .1f);
            //what about opposite direction
            //what is max radial distance
        }

        //scale the radial distance from 0 to 2
        public float D2BaseSpeedMultiplyer()
        {
            return math.clamp(2 - (cartesianDistance.y / 2000), 0, 2);
            //return math.clamp((.1f * radialDistance / 600), 0, 2);
            //what about opposite direction
            //what is max radial distance
        }

        //scale the radial distance from 0 to 2
        public float D3BaseSpeedMultiplyer()
        {
            //return 1;
            return math.clamp(2 - (cartesianDistance.x / 2000), 0, 2);

            //return math.clamp((.1f * radialDistance / 80), 0, 2);
            //what about opposite direction
            //what is max radial distance
        }


        public float blackHoleParticleScale()
        {
            //scale with radial distance
            //TODO : fix
            return math.clamp((20 - radialDistance / 800), 1, 20);

        }


        public float blackHoleAcceleration()
        {
            //set with audio parameters
            return math.clamp((10 * cartesianDistance.z / 100), 0, 10);
        }


        public float D1Harmonicity()
        {
            //sclae player 
            return math.clamp(((cartesianDistance.z + 1000) / 2000), 0, 1);
        }


        public float D2Harmonicity()
        {
            // scale player x coordinate to 0 1.
            return math.clamp(((cartesianDistance.x + 1000) / 2000), 0, 1);
        }

        public float D3Harmonicity()
        {
            //scale y to between 0 and 1
            return math.clamp(((cartesianDistance.y + 1000) / 2000), 0, 1);

        }

        public float getD1Radius()
        {
            //set with audio parameters
            return d1Radius;
        }

        public void setD1Radius(float newRadius)
        {
            d1Radius = newRadius;
        }

        public float getD2Radius()
        {
            //set with audio parameters
            return d2Radius;
        }

        public void setD2Radius(float radius)
        {
            d2Radius = radius;
        }

        public float getD3Radius()
        {
            //set with audio parameters
            return d3Radius;
        }

        public void setD3Radius(float newRadius)
        {
            //pass in new radius. store old radius.

            //d3Radius = math.lerp()
            d3Radius = newRadius;
        }

        private void Update()
        {
            // on update grab the player pos 
            radialDistance = GetRadialDistanceFromOrigin(playerT.position);
            cartesianDistance = GetCartesianDistanceFromOrigin(playerT.position);

            //job systems should grab from ParameterManagerGreg.instance
            Debug.Log(radialDistance);
            Debug.Log(cartesianDistance);

            


        }

        public IEnumerator D1RadiusLerp(object[] d1params)
        {
            float t = 0;
            while (t < 1)
            {
                d1Radius = Mathf.Lerp((float) d1params[0], (float) d1params[1], t);
                t = t + (Time.deltaTime * (float) d1params[2]);
                yield return null;
            }
            yield return null;
        }

        public IEnumerator D2RadiusLerp(object[] d2params)
        {
            float t = 0;
            while (t < 1)
            {
                d2Radius = Mathf.Lerp((float) d2params[0], (float) d2params[1], t);
                t = t + (Time.deltaTime * (float) d2params[2]);
                yield return null;
            }
            yield return null;
            //StopCoroutine("D2RadiusLerp");
        }

        public IEnumerator D3RadiusLerp(object[] d3params)
        {
            float t = 0;
            while (t < 1)
            {
                d3Radius = Mathf.Lerp((float) d3params[0], (float)d3params[1], t);
                t = t + (Time.deltaTime * (float) d3params[2]);
                yield return null;
            }
            yield return null;

        }



    }
}