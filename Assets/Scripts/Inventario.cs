using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    //private Dictionary<Item, int> itens;
    [SerializeField]
    private int slots;
    private int slotsDisponiveis;
    private GameObject jogador;
    [SerializeField]
    private GameObject telaDeInventario;
    [SerializeField]
    private Texture2D texturaCursor;
    private Vector2 cursorHotspot;
    [SerializeField]
    private AudioClip somDeErro;
    private GameObject[] slotsDoInventario;

    private void Start()
    {
        cursorHotspot = new Vector2(texturaCursor.width / 2, texturaCursor.height / 2);
        jogador = GameObject.FindGameObjectWithTag("Player");
        //itens = new Dictionary<Item, int>();
        slots = 4;
        slotsDisponiveis = slots;
        telaDeInventario.SetActive(true);
        slotsDoInventario = GameObject.FindGameObjectsWithTag("Slot");
        telaDeInventario.SetActive(false);
    }

    public bool AdicionarItem(Item item, int quantidade = 1)
    {
        slotsDisponiveis = slotsDoInventario.Take(slots).Where(a => a.GetComponent<Slot>().item == null).Count();
        //Debug.Log("slots disponiveis: " + slotsDisponiveis);
        float slotsNecessarios = (float)quantidade / item.stack;

        var slotsComItem = slotsDoInventario.Where(a => a.GetComponent<Slot>().item?.nome == item.nome
        && a.GetComponent<Slot>().GetQuantidade() < item.stack).Select(a => a.GetComponent<Slot>()).ToList();

        //Debug.Log(slotsComItem == null);

        float slotsResiduais = 0;

        //Debug.Log("Tentando adicionar " + quantidade);

        if (slotsComItem.Any())
        {
            slotsResiduais = slotsComItem.Select(a => (item.stack - (float)a.GetQuantidade()) / item.stack).Sum();
            //Debug.Log("Slots Residuais: " + slotsResiduais);
        }
        //se a quantidade de slots for suficiente
        if (slotsDisponiveis + slotsResiduais >= slotsNecessarios)
        {
            //caso o item não exista no inventario e seja menor que um stack
            if (quantidade <= item.stack && !slotsComItem.Any())
            {
                slotsDoInventario.FirstOrDefault(a => a.GetComponent<Slot>().item == null).GetComponent<Slot>().AtualizarSlot(item, quantidade);
            }
            //item maior do que o stack
            else
            {
                //caso o item exista no inventario
                foreach (var slot in slotsComItem)
                {
                    //caso a soma do ultimo slot com o item e a quantidade adicionada seja maior que um stack
                    if (quantidade + slot.GetQuantidade() > item.stack)
                    {
                        //Debug.Log(quantidade);
                        quantidade -= item.stack - slot.GetQuantidade();
                        slot.SetQuantidade(slot.GetQuantidade() + item.stack - slot.GetQuantidade());
                    }
                    else
                    {
                        //Debug.Log(quantidade);
                        slot.SetQuantidade(slot.GetQuantidade() + quantidade);
                        quantidade = 0;
                        return true;
                    }
                }
                while (quantidade != 0)
                {
                    //quantidade menor que o stack
                    if (quantidade < item.stack)
                    {
                        //Debug.Log(quantidade);
                        slotsDoInventario.FirstOrDefault(a => a.GetComponent<Slot>().item == null).GetComponent<Slot>().AtualizarSlot(item, quantidade);
                        quantidade = 0;
                    }
                    //quantidade maior que o stack
                    else
                    {
                        //Debug.Log(quantidade);
                        slotsDoInventario.FirstOrDefault(a => a.GetComponent<Slot>().item == null).GetComponent<Slot>().AtualizarSlot(item, item.stack);
                        quantidade -= item.stack;
                    }
                }
            }

            //slotsDisponiveis = (int)(slotsDisponiveis - slotsNecessarios + slotsResiduais);
            //inventarioModificado = true;
            return true;
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(somDeErro, 0.4f);
            return false;
        }
    }
    public void AtualizarInventario()
    {
        foreach (var slot in slotsDoInventario)
        {
            //slots desbloqueados pelo jogador
            if (Array.IndexOf(slotsDoInventario, slot) <= slots - 1)
            {
                if (slot.GetComponent<Slot>().item != null)
                {
                    var _item = slot.GetComponent<Slot>().item;
                    slot.GetComponent<Slot>().quantidade.SetActive(true);
                    slot.GetComponent<Slot>().iconeItem.SetActive(true);
                    switch (_item.tier)
                    {
                        case 0:
                            slot.GetComponent<Image>().color = new Color32(90, 129, 210, 255);
                            break;
                        case 1:
                            slot.GetComponent<Image>().color = new Color(0.275f, 0.375f, 1.0f);
                            break;
                        case 2:
                            slot.GetComponent<Image>().color = new Color(0.765f, 0.325f, 1.0f);
                            break;
                        case 3:
                            slot.GetComponent<Image>().color = new Color(1.0f, 0.769f, 0.0f);
                            break;
                    }
                }
                else
                {
                    slot.GetComponent<Image>().color = new Color32(90, 129, 210, 255);
                }
                slot.GetComponent<EventTrigger>().enabled = true;
            }
            else
            {
                //slots do inventario que não foram desbloqueados
                slot.GetComponent<Image>().color = Color.gray;
                slot.GetComponent<EventTrigger>().enabled = false;
            }

        }
    }
    public void AbrirInventario()
    {
        if (telaDeInventario.activeSelf)
        {
            var objetoNomeDoItem = slotsDoInventario[0].GetComponent<Slot>().campoNomeDoItem;
            foreach (var slot in slotsDoInventario)
            {
                slot.GetComponent<Slot>().DesativarH();
            }
            telaDeInventario.SetActive(false);
            jogador.GetComponent<AtaqueDoJogador>().enabled = true;
            jogador.GetComponent<PolygonCollider2D>().enabled = true;
            objetoNomeDoItem.SetActive(false);
            objetoNomeDoItem.GetComponent<TextMeshProUGUI>().text = null;
            Time.timeScale = 1;
        }
        else
        {
            telaDeInventario.SetActive(true);
            AtualizarInventario();
            jogador.GetComponent<AtaqueDoJogador>().enabled = false;
            jogador.GetComponent<PolygonCollider2D>().enabled = false;
            slotsDoInventario[0].GetComponent<Slot>().campoNomeDoItem.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void AumentarSlots(int quantidade = 1)
    {
        if (quantidade + slots <= 16)
        {
            Debug.Log("Aumentou o inventario");
            slots += quantidade;
        }
    }



    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab) && (!GameObject.FindGameObjectWithTag("Tela") || GameObject.FindGameObjectWithTag("Tela") == telaDeInventario))
        {
            AbrirInventario();
        }
    }
}
