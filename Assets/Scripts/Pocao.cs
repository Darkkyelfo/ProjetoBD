using UnityEngine;
using System.Collections;

public class Pocao : Itens {
    public byte tipoDePocao;//indica se é poção de cura(1) ou de mana(2)
    public int recuperacao;//determina a quantidade a ser restaurada
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void recuperarPlayer()
    {
        Status statusPortador = portador.GetComponentInChildren<Status>() as Status;
        if (this.tipoDePocao == 1) {//pocao de cura
            if(statusPortador.hpAtual+ recuperacao <= statusPortador.hp) {
                statusPortador.hpAtual = statusPortador.hpAtual + recuperacao;
            }
            else {
                statusPortador.hpAtual = statusPortador.hp;
            }
        }
        else if (this.tipoDePocao == 2) {
            if (statusPortador.mpAtual + this.recuperacao <= statusPortador.mp) {
                statusPortador.mpAtual = statusPortador.mpAtual + this.recuperacao;
            }
            else {
                statusPortador.mpAtual = statusPortador.mp;
            }
        }
    }

}
