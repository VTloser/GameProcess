using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Object = UnityEngine.Object;
using UnityEngine.Events;

public class Pool<T> : MonoBehaviour where T : Object
{
    /// <summary>
    ///  构造器 初始化时产生多少物体
    /// </summary>
    /// <param name="Tempt"></param>
    /// <param name="trans"></param>
    /// <param name="Size"></param>
    public Pool(T Tempt, Transform parent, int Size)
    {
        t = Tempt;
        MaxSize = Size;
        Items = new T[MaxSize];
        Parent = parent;
        //首先初始化出MaxSize个
        for (int i = 0; i < MaxSize; i++)
        {
            Items[i] = Instantiate(t, parent.position, parent.rotation);
            (Items[i] as GameObject).SetActive(false);
        }
    }
    public Pool(T Tempt, int Size)
    {
        t = Tempt;
        MaxSize = Size;
        Items = new T[MaxSize];
        //首先初始化出MaxSize个
        for (int i = 0; i < MaxSize; i++)
        {
            Items[i] = Instantiate(t);
            (Items[i] as Bullets).gameObject.SetActive(false);
        }
    }


    /// <summary>   位置   </summary>
    private Transform Parent;
    /// <summary>   类型   </summary>
    private T t;
    /// <summary>   对象池最大容量   </summary>
    private int MaxSize;
    /// <summary>   对象池   </summary>
    private T[] Items;

    //创建对象
    public T GetObject()
    {
        for (int i = 0; i < MaxSize; i++)
        {
            if (!(Items[i] as Bullets).gameObject.activeInHierarchy)
            {
                (Items[i] as Bullets).gameObject.SetActive(true);
                return Items[i];
            }
        }
        Debug.Log(132);
        //遍历数组后没有不再使用的
        return DynamicAddSize();
    }
    public T GetObject(UnityAction<Object> action)
    {
        for (int i = 0; i < MaxSize; i++)
        {
            if (!(Items[i] as GameObject).activeInHierarchy)
            {
                action?.Invoke(Items[i]);
                (Items[i] as GameObject).SetActive(true);
                return Items[i];
            }
        }
        Debug.Log(132);
        //遍历数组后没有不再使用的
        return DynamicAddSize();
    }
    //销毁对象
    public void DestObject(T t)
    {
        for (int i = 0; i < MaxSize; i++)
        {
            if (Items[i] == t)
            {
                (Items[i] as Bullets).gameObject.SetActive(false);
                return;
            }
        }
    }

    /// <summary>
    /// 动态调整数组长度
    /// </summary>
    public T DynamicAddSize()
    {
        Debug.Log($"数组长度不足");
        int RecordNum;
        int n = 1;
        while (n <= MaxSize) n *= 2;
        T[] Temp = new T[n];
        Array.Copy(Items, 0, Temp, 0, Items.Length);
        Items = Temp;
        for (int i = MaxSize; i < n; i++)
        {
            Items[i] = Instantiate(t);
            (Items[i] as Bullets).gameObject.SetActive(false);
        }
        RecordNum = MaxSize;
        MaxSize = n;
        Debug.Log($"数组长度不足，动态调整数组长度,调整后长度{ MaxSize }");

        (Items[RecordNum] as Bullets).gameObject.SetActive(true);
        return Items[RecordNum];
    }

    //批量式初始化
    public void DynamicAddSize(int size)
    {
        Debug.Log("数组长度不足，动态调整数组长度");
        int n = 1;
        while (n <= size) n *= 2;
        T[] Temp = new T[n];
        Array.Copy(Items, 0, Temp, 0, Items.Length);
        Items = Temp;
        for (int i = MaxSize; i < n; i++)
        {
            Items[i] = Instantiate(t);
            (Items[i] as GameObject).SetActive(false);
        }
        MaxSize = n;
        Debug.Log($"调整后长度{ MaxSize }");
    }


    //TODO 暂时不知道怎么处理
    public void DynamciReduceSize()
    {
        Debug.Log("数组长度过长，动态调整数组长度");
        int n = MaxSize / 2;
        T[] Temp = new T[n];
        Array.Copy(Items, 0, Temp, 0, Items.Length);
        Items = Temp;
        for (int i = n; i < MaxSize; i++)
        {
            Destroy(Items[i]);
        }
        MaxSize = n;
        Debug.Log($"调整后长度{ MaxSize }");
    }

}
