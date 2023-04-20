using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCube : MonoBehaviour
{
    private ColorType colorType;

    public ColorType ColorType
    {
        get
        {
            return colorType;
        }
        set
        {
            colorType = value;
            ChangeColor();
        }
    }

    public void ChangeColor()
    {
        this.GetComponent<MeshRenderer>().material = BulletsManager.Instance.Cubematerials[(int)colorType];
    }
}
