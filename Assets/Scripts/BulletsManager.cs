using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;

public class BulletsManager : MonoBehaviour
{
    /// <summary> 单例 </summary>
    public static BulletsManager Instance;

    /// <summary> 子弹材质球 </summary>
    public Material[] Bulletsmaterials;

    /// <summary> 子弹材质球 </summary>
    public Material[] Cubematerials;

    /// <summary> Bullet </summary>
    public Bullets Bullet;

    /// <summary> Bullet </summary>
    public GameObject Bullet_Init;

    /// <summary> ColorCube </summary>
    public GameObject ColorCube;

    /// <summary> 射线检测层 </summary>
    public LayerMask layerMask;

    /// <summary> 速度 </summary>
    public float Speed = 5;

    /// <summary> 哨塔 </summary>
    public List<TurrentItem> Turrent = new List<TurrentItem>();

    /// <summary> 哨塔 </summary>
    public List<GameObject> Turrent_Init = new List<GameObject>();

    /// <summary> Bullet集合 </summary>
    public List<Bullets> bullets = new List<Bullets>();

    /// <summary> 对象池 </summary>
    Pool<Bullets> pool;

    /// <summary> ColorCubes </summary>
    public List<GameObject> ColorCubes = new List<GameObject>();


    public void Awake()
    {
        Instance = this;
        pool = new Pool<Bullets>(Bullet, 20);
        //Turrent = FindObjectsOfType<TurrentItem>(true).ToList();

        foreach (var item in Turrent)
        {
            TurrentControl(item.gameObject);
        }

        INIT();
    }


    void INIT()
    {
        StartCoroutine(Init());
    }


    IEnumerator Init()
    {

        GenertBullet();
        GenertBulletColorCube();
        yield return new WaitForSeconds(3);
        GenerateTurrent();

    }

    /// <summary>
    /// 生成防御塔
    /// </summary>
    private void GenerateTurrent()
    {
        StartCoroutine("TurrentGenerate");
    }


    IEnumerator TurrentGenerate()
    {
        int i = 0;
        while (i < Turrent_Init.Count)
        {
            for (i = 0; i < Turrent_Init.Count; i++)
            {
                CamControl.Instance.Shake();
                Turrent_Init[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                Turrent[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.4f);
            }
        }
        foreach (var item in Turrent)
        {
            item.count = 500;
        }
    }

    /// <summary>
    /// 生成场地颜色Cube
    /// </summary>
    private void GenertBulletColorCube()
    {
        StartCoroutine("BulletColorCubeGenert");

        //for (int i = -10; i < 10; i++)
        //{
        //    for (int j = -10; j < 10; j++)
        //    {
        //        GameObject.Instantiate(ColorCube, new Vector3(i, 0, j), Quaternion.identity, this.transform);
        //    }
        //    yield return new WaitForSeconds(0);
        //}
    }

    IEnumerator BulletColorCubeGenert()
    {
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                GameObject.Instantiate(ColorCube, new Vector3(i, 0, j), Quaternion.identity, this.transform);
            }
            yield return new WaitForSeconds(0.1f);
        }
        Bullet_Init.gameObject.SetActive(true);
    }

    public void GenertBullet()
    {
        foreach (var item in Turrent)
        {
            StartCoroutine(Generate(item));
        }
    }


    void Update()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i] != null)
                BulletControl(bullets[i]);
        }
    }

    private void TurrentControl(GameObject tag)
    {
        RotateControl(tag);
    }
    private void RotateControl(GameObject tag)
    {
        tag.transform.DORotate(new Vector3(0, 90, 0), 2)
            .SetEase(Ease.Linear)
            .SetRelative(true)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void BulletControl(Bullets Tag)
    {
        if (!Tag.gameObject.activeInHierarchy)
            return;

        CollControl(Tag);
        MoveControl(Tag.gameObject);
    }

    IEnumerator Generate()
    {
        for (int i = 0; i < 100; i++)
        {
            var t = pool.GetObject();
            t.transform.rotation = Turrent[0].transform.rotation;
            t.transform.position = Turrent[0].transform.position;
            bullets.Add(t);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Generate(TurrentItem turrent)
    {
        while (true)
        {
            if (turrent.count > 0)
            {
                turrent.count--;
                var t = pool.GetObject();
                t.GetComponent<Bullets>().Init(turrent);
                bullets.Add(t);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 移动控制
    /// </summary>
    /// <param name="Tag"></param>
    private void MoveControl(GameObject Tag)
    {
        Tag.transform.Translate(Vector3.forward * Time.deltaTime * Speed);
    }


    /// <summary>
    /// 碰撞控制
    /// </summary>
    private void CollControl(Bullets Tag)
    {
        var bullet = Tag.GetComponent<Bullets>();

        bullet.CureentPos = Tag.transform.position;
        if (Physics.Linecast(bullet.LaterPos, bullet.CureentPos, out bullet.hitInfo))
        {
            if (bullet.hitInfo.collider.CompareTag("Wall"))
            {
                Coll_Wall(Tag.gameObject, bullet.CureentPos - bullet.LaterPos, bullet.hitInfo.normal);
            }
            if (bullet.hitInfo.collider.CompareTag("ColorCub"))
            {
                Coll_ColorCub(Tag, bullet);
            }
        }
        bullet.LaterPos = Tag.transform.position;
    }

    /// <summary>
    /// 碰撞到墙体
    /// </summary>
    /// <param name="Tag"></param>
    /// <param name="Dir"></param>
    /// <param name="normal"></param>
    private void Coll_Wall(GameObject Tag, Vector3 Dir, Vector3 normal)
    {
        Tag.transform.forward = Vector3.Reflect(Dir, normal);
    }

    /// <summary>
    /// 碰撞到颜色Cube
    /// </summary>
    /// <param name="Tag"></param>
    private void Coll_ColorCub(Bullets Tag, Bullets bullet)
    {
        if (bullet.hitInfo.collider.gameObject.GetComponent<ColorCube>(). ColorType == Tag.GetComponent<Bullets>().colorType)
        {
            // 如果颜色已经一样了就传过去
        }
        else
        {
            GameObject Temp = bullet.hitInfo.collider.gameObject;
            Temp.transform.DOScale(Vector3.one * 1f, 0.2f).SetEase(Ease.InOutBack).OnComplete(() => { Temp.transform.DOScale(Vector3.one * 0.7f, 0.1f); });
            Temp.GetComponent<ColorCube>().ColorType = Tag.GetComponent<Bullets>().colorType;
            //Todo 销毁物体
            BulletDestory(Tag);
        }
    }

    /// <summary>
    /// 销毁
    /// </summary>
    /// <param name="DesTag"></param>
    public void BulletDestory(Bullets DesTag)
    {
        bullets.Remove(DesTag);
        pool.DestObject(DesTag);
    }

}

public enum ColorType
{
    Null,
    red,
    blue,
    yellow,
    green,
}