using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BotoesMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI tmp;

    private void Start()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        tmp.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tmp.color = new Color(0.9529412f, 0.7019608f, 0.4117647f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmp.color = Color.white;
    }

}
