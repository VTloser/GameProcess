using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rote : MonoBehaviour
{
    //指定角度
    private float Angle;
    public AnimationCurve Curve;
    void Awake()
    {
        //Keyframe[] ks = new Keyframe[3];
        //ks[0] = new Keyframe(0, 0);
        //ks[0].value = 0;
        //ks[1] = new Keyframe(4, 0);
        //ks[1].value = 1;
        //ks[2] = new Keyframe(8, 0);
        //ks[2].value = 2;
        //Curve = new AnimationCurve(ks);





        //BeginRot();
    }



    //private void BeginRot()
    //{
    //    this.transform.DORotate(new Vector3(3500, 0, 0), 5,RotateMode.FastBeyond360).SetEase(Curve);
    //}


    float X = 0;
    float a = 0;
    private void Update()
    {
        Angle = this.transform.eulerAngles.x;
        Debug.Log(Angle);

        //X += Time.deltaTime;
        //double Y;
        float A = 1;
        float W = 1;
        //Y = A * Math.Sin(Math.PI * W * X);
        //var Dt = new Vector3(this.transform.position.x + X, (float)(this.transform.position.y + Y), this.transform.position.z);
        //this.transform.position = Dt;

        X += Time.deltaTime * 10;
        var Y = A * Mathf.Sin(Mathf.PI / 2 * X );
        //var y = Mathf.Sin(Mathf.PI / 2 * 3) * 1f;
        //Debug.Log(y);
        var Dt = new Vector3(X, Y, this.transform.position.z);
        this.transform.position = Dt;
    }
}
