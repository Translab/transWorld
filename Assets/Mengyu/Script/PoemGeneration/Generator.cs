using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

	public float height = 40;
    public GameObject[] buildings;
    public int mapWidth = 10;
    public int mapHeight = 10;
	public int skip_step = 2; 
    public int separation_dist = 6;
	private GameObject placeholder;
	private GameObject clone;

	// Use this for initialization
	void Start () {
        //float seed = 42;
        float seed = Random.Range(0,100);
		placeholder = new GameObject ();
		placeholder.name = "City Components";
		for (int h = 0; h < mapHeight; h+= skip_step)
        {
			for (int w = 0; w < mapWidth; w+= skip_step)
            {
                int result = (int)(Mathf.PerlinNoise(w/50.0f + seed, h/50.0f + seed) * 10);
				Vector3 pos = new Vector3( transform.position.x - mapWidth / 2  * separation_dist + w * separation_dist , Random.Range( transform.position.y - height / 2, transform.position.y + height / 2), transform.position.z - mapHeight / 2 * separation_dist + h * separation_dist);
                if (result < 2) {
                    clone = Instantiate(buildings[0], pos, Quaternion.identity);
                    //Instantiate(buildings[5], poshigh, Quaternion.identity);
                }
                else if (result < 4) {
                    clone = Instantiate(buildings[1], pos, Quaternion.identity);
                    //Instantiate(buildings[6], poshigh, Quaternion.identity);
                }
                else if (result < 6) {
                    clone = Instantiate(buildings[2], pos, Quaternion.identity);
                    //Instantiate(buildings[7], poshigh, Quaternion.identity);
                }
                else if (result < 8) {
                    clone = Instantiate(buildings[3], pos, Quaternion.identity);
                    //Instantiate(buildings[8], poshigh, Quaternion.identity);
                }
                else if (result < 10) {
                    clone = Instantiate(buildings[4], pos, Quaternion.identity);
                    //Instantiate(buildings[9], poshigh, Quaternion.identity);
                }
				clone.transform.SetParent (placeholder.transform);
                //int n = Random.Range(0, buildings.Length);
                //Instantiate(buildings[n], pos, Quaternion.identity);
            }
        }	
	}
	
	
}
