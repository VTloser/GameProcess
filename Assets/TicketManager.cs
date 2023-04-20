using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class TicketManager : MonoBehaviour
{
    public GameObject Tag;
    void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BeginRot(()=> { Debug.Log(Tag.transform.eulerAngles.z); });
        }
    }
    /// <summary>
    /// ¿ªÊ¼Ðý×ª
    /// </summary>
    /// <param name="action"></param>
    private void BeginRot(UnityAction action = null)
    {
        Tag.transform.DOLocalRotate(Vector3.forward * UnityEngine.Random.Range(3240, 3601), 8)
            .SetEase(Ease.OutBack)
            .SetRelative()
            .OnComplete(() =>
            {
                action?.Invoke();
            });
    }
}
