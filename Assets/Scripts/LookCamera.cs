using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LookCamera: MonoBehaviour
{
    Camera referenceCamera;

    public enum Axis { up, down, left, right, forward, back };
    public bool reverseFace = false;
    public Axis axis = Axis.up;
    public string whatToDo;
    private bool mostrar = true;

    public bool Mostrar
    {
        get
        {
            return mostrar;
        }

        set
        {
            mostrar = value;
        }
    }

    public Vector3 GetAxis(Axis refAxis)
    {
        switch (refAxis)
        {
            case Axis.down:
                return Vector3.down;
            case Axis.forward:
                return Vector3.forward;
            case Axis.back:
                return Vector3.back;
            case Axis.left:
                return Vector3.left;
            case Axis.right:
                return Vector3.right;
        }
        return Vector3.up;
    }

    void Awake()
    {
        if (!referenceCamera)
            referenceCamera = Camera.main;
    }

    void Update()
    {
        Vector3 targetPos = transform.position + referenceCamera.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back);
        Vector3 targetOrientation = referenceCamera.transform.rotation * GetAxis(axis);
        transform.LookAt(targetPos, targetOrientation);
    }

    public void ShowMessage() {
        if (mostrar)
        {
            this.gameObject.SetActive(true);
            this.GetComponentInChildren<Text>().text = whatToDo;
        }
        else {
            this.gameObject.SetActive(false);
        }
    }

    public void HideMessage() {
        this.gameObject.SetActive(false);
    }
}