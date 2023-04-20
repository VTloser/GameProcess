using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CamControl : MonoBehaviour
{

    public static CamControl Instance;
    Camera camera;

    private void Awake()
    {
        Instance = this;
        camera = this.GetComponent<Camera>();
    }

    public void Shake()
    {
        camera.DOShakePosition(0.2f, 1, 1000);
    }

}
