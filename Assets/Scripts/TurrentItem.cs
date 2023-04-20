using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentItem : MonoBehaviour
{
    public ColorType colorType;

    /// <summary>    ×Óµ¯ÊýÁ¿     /// </summary>
    public int count;

    private void Start()
    {
        this.GetComponent<MeshRenderer>().material = BulletsManager.Instance.Cubematerials[(int)colorType];
    }
}
