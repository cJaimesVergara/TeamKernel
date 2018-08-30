using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour {

    public GameObject putInObject;
    public Vector3 position;


    public void PutInObject()
    {
        this.transform.SetParent(putInObject.transform);
        this.transform.localPosition = position;
    }
}
