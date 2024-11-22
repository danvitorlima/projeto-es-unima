using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrulhaInimigo : MonoBehaviour
{
    public bool patrulha;
    Rigidbody2D rb;
    Vector3[] pontosDePatrulha;
    Vector3 ppAtual;
    int index;
    private double lastAttackTime;
    private NavMeshAgent agent;
    private bool fugindo;
    private bool ranged;

    void Start()
    {
        ranged = GetComponent<AtaqueDoInimigo>() != null;
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        lastAttackTime = -5;
        index = 1;
        pontosDePatrulha = EncontrarProximosPPs();
        ppAtual = pontosDePatrulha[index];
        rb = GetComponent<Rigidbody2D>();
        patrulha = true;
        agent.SetDestination(ppAtual);
        //agent.avoidancePriority = Random.Range(45, 50);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime >= 1)
            {
                collision.gameObject.GetComponent<SistemaVida>().ReceberDano(10);
                lastAttackTime = Time.time;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime >= 1)
            {
                collision.gameObject.GetComponent<SistemaVida>().ReceberDano(10);
                lastAttackTime = Time.time;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            patrulha = false;
            index = 0;
            agent.ResetPath();
            if (ranged)
            {
                agent.stoppingDistance = 2f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            fugindo = false;
            patrulha = true;
            pontosDePatrulha = EncontrarProximosPPs();
            ppAtual = pontosDePatrulha[index];
            if (agent.isActiveAndEnabled)
            {
                agent.SetDestination(ppAtual);
            }
            if (ranged)
            {
                agent.stoppingDistance = 0;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && agent.isActiveAndEnabled)
        {
            if (ranged)
            {
                if (Vector2.Distance(transform.position, collision.transform.position) > 1.5f)
                {
                    if (agent.velocity.Equals(Vector3.zero))
                    {
                        var direcao = collision.transform.position - transform.position;
                        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direcao.normalized);
                        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, targetRotation, Time.deltaTime * 15f);
                        fugindo = false;
                    }
                    agent.SetDestination(collision.transform.position);
                    if (agent.stoppingDistance == 0)
                    {
                        agent.stoppingDistance = 2f;
                    }
                }
                else
                {
                    fugindo = true;
                    var direcao = collision.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direcao.normalized);
                    agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, targetRotation, Time.deltaTime * 15f);
                    agent.SetDestination(2 * transform.position - collision.transform.position);
                    if (agent.stoppingDistance == 2)
                    {
                        agent.stoppingDistance = 0;
                    }
                }
            }
            else
            {
                agent.SetDestination(collision.transform.position);
            }
        }
    }


    private GameObject EncontrarMenorDistancia(GameObject objeto, GameObject[] lista)
    {
        float menorDistancia = Mathf.Infinity;
        GameObject objetoProximo = lista[0];
        foreach (var item in lista)
        {
            var distanciaAtual = Vector3.Distance(item.transform.position, gameObject.transform.position);
            if (distanciaAtual < menorDistancia)
            {
                menorDistancia = distanciaAtual;
                objetoProximo = item;
            }
        }
        return objetoProximo;
    }
    private Vector3[] EncontrarProximosPPs()
    {
        var pps = GameObject.FindGameObjectsWithTag("PP");
        var listaDePPs = pps.ToList().OrderBy(p => Vector3.Distance(p.transform.position, transform.position)).Take(4);
        return listaDePPs.Select(n => n.transform.position).ToArray();
        //Vector2 direcao = ppProximo.transform.position - gameObject.transform.position;
        //Debug.Log(direcao.magnitude);
        //rb.MovePosition(Vector3.MoveTowards(rb.position, ppProximo.transform.position, 2 * Time.deltaTime));
        //if (direcao.magnitude != 0)
        //{
        //    gameObject.transform.up = direcao.normalized;
        //}
    }
    private void Update()
    {
        Vector3 direction = agent.velocity.normalized;
        if (direction.magnitude > 0.1f && !fugindo)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            //suavizando rotação do inimigo
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, targetRotation, Time.deltaTime * 15f);
        }
    }
    void FixedUpdate()
    {
        if (patrulha)
        {
            if (agent.isActiveAndEnabled && !agent.pathPending && agent.remainingDistance <= 0.6f)
            {
                if (index != pontosDePatrulha.Length - 1)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
                ppAtual = pontosDePatrulha[index];
                agent.SetDestination(ppAtual);
            }
        }
    }
}
