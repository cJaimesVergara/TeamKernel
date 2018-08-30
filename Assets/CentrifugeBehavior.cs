using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentrifugeBehavior : MonoBehaviour {

    float posOrigX ;
    float posOrigZ ;
    float posOrigY;
    bool isOn = true;   
    public LookCamera messageGO;

    // Use this for initialization
    void Start () {
        posOrigX = this.gameObject.transform.position.x;
        posOrigY = this.gameObject.transform.position.y;
        posOrigZ = this.gameObject.transform.position.z;
    }
	
	// Update is called once per frame
	void Update () {
        if (isOn) {
            float rndX = Random.Range(0.0f, 0.02f);
            float rndZ = Random.Range(0.0f, 0.02f);
            transform.position = new Vector3(posOrigX + rndX, posOrigY, posOrigZ + rndZ);
        }    
    }

    public void shutDown() {
        if (isOn) {
            transform.position = new Vector3(posOrigX, posOrigY, posOrigZ);
            isOn = false;
            messageGO.gameObject.SetActive(false);
            messageGO.Mostrar=false;
        }
    }
}
