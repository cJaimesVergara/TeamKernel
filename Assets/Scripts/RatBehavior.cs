using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBehavior : MonoBehaviour {

    public GameObject putInObject;
    public GameObject ratInCage;


    public void ToCage() {
        ratInCage.SetActive(true);
        GameObject.Destroy(this.gameObject);
    }

    public void makenoise() {
        Debug.Log("NOISE");
    }
	
}
