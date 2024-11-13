using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject highlight;
    public GameObject iconeItem;
    public GameObject quantidade;
    public Item item { get; private set; }
    private bool deletavel = false;
    public void AtivarH()
    {
        highlight.SetActive(true);
    }
    public void DesativarH()
    {
        highlight.SetActive(false);
    }
    public void AtualizarSlot(Item _item, int _quantidade)
    {
        item = _item;
        Debug.Log("??" + item.nome);
        quantidade.GetComponent<TextMeshProUGUI>().text = _quantidade.ToString();
        iconeItem.GetComponent<Image>().sprite = item.icone;
    }
    public void ConsumirItem()
    {
        if (item != null && item.Usar())
        {
            RemoverItem();
        }
    }
    public int GetQuantidade()
    {
        return int.Parse(quantidade.GetComponent<TextMeshProUGUI>().text);
    }
    public void SetQuantidade(int _quantidade)
    {
        quantidade.GetComponent<TextMeshProUGUI>().text = _quantidade.ToString();
    }
    public void RemoverItem()
    {
        if (item != null)
        {
            if (GetQuantidade() == 1)
            {
                item = null;
                iconeItem.GetComponent<Image>().sprite = null;
                iconeItem.SetActive(false);
                quantidade.SetActive(false);
                GetComponent<Image>().color = new Color32(90, 129, 210, 255);
            }
            SetQuantidade(GetQuantidade() - 1);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        deletavel = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        deletavel = false;
    }
    private void Update()
    {
        if (item && deletavel && Input.GetKeyDown(KeyCode.Q))
        {
            RemoverItem();
        }
    }
}
