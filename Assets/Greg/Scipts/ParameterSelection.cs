using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterSelection : MonoBehaviour {

    private LSystemGen gen;
    private Renderer[] rends;

	// Use this for initialization
	void Awake () {
        gen = GetComponent<LSystemGen>();
        gen.angle = Random.Range(80, 100f);
        gen.length = Random.Range(1f, 3f);
        gen.randomness = Random.Range(0f, .2f);
        gen.tropism = new Vector3(Random.Range(80,100), 0, 0);
	}

    private void Start()
    {
        rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer dummyRend in rends)
        {
            //Debug.Log("child rends");
            dummyRend.material.SetFloat("_MovementAmplitude", Random.Range(20f, 40f));
            dummyRend.material.SetFloat("_Frequencyx", Random.Range(.05f, 0.15f));
            dummyRend.material.SetFloat("_Frequencyy", Random.Range(0.07f, 0.014f));
            dummyRend.material.SetFloat("_Frequencyz", Random.Range(0.1f, 0.2f));


        }
    }

    // Update is called once per frame
    void Update () {


		
	}
}
