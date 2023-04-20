using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public ColorType colorType;
    [HideInInspector]
    public Vector3 LaterPos;
    [HideInInspector]
    public Vector3 CureentPos;
    [HideInInspector]
    public RaycastHit hitInfo;
    public float Speed;
    public void Init(TurrentItem Tag)
    {
        LaterPos = Tag.transform.position;
        CureentPos = Tag.transform.position;
        this.colorType = Tag.colorType;
        this.transform.rotation = Tag.transform.rotation;
        this.transform.position = Tag.transform.position;

        this.GetComponent<Renderer>().material = BulletsManager.Instance.Bulletsmaterials[(int)colorType];
    }
}
