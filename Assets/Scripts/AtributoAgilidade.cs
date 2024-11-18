using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtributoAgilidade : Atributo
{
    public override void EvoluirAtributo()
    {
        base.EvoluirAtributo();
        GameObject.FindGameObjectWithTag("Player").GetComponent<AtaqueDoJogador>().AumentarVelocidadeDeAtaque(1.3f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<MovimentacaoPersonagem>().AumentarVelocidade(1.09f);
    }
}
