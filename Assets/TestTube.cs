using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTube : MonoBehaviour {

    public GameObject putInObject;
    public Vector3 position;
    public Vector3 rotation;

    public void PutInObject()
    {
        this.transform.SetParent(putInObject.transform);
        this.transform.eulerAngles = rotation;
        this.transform.localPosition = position;
    }
}
