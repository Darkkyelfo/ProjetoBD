using UnityEngine;
using System.Collections;

public class Arma : Itens {
	public int danoBase;//coloquei público para visualizar o valor em tempo de execução
	public float chanceDeCritico;//1 -> 100%, 0->0% 
    public int multDeCritico;//Define por quanto o ataque vai ser multiplicado caso seja crítico
    // Use this for initialization
    void Start () {
        this.buffPortador();
        Status estatusPortador = portador.GetComponentInChildren <Status>() as Status;
        this.chanceDeCritico = this.chanceDeCritico +(int)( estatusPortador.inteligencia + (int)estatusPortador.forca / 2)/10;//chance de crítico aumenta em função da inteligencia e forca
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	public int getDanoBase(){
		return danoBase;
	}
	public void setPortador(GameObject portador){
		Status estatusPortador = portador.GetComponent("Status") as Status;
		danoBase = status.ataque * estatusPortador.forca;
	}
    
    private bool critico()
    {
        bool ehCritico = false;
        float numAleatorio =Random.Range(0, 1.01f);
        if (numAleatorio <= this.chanceDeCritico) {
            ehCritico=true;
            Debug.Log("foi crítico");
        }
        return ehCritico;


    }

	void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player" && portador.tag=="inimigo") {
            EnemyBehavior inimigo = portador.GetComponent("EnemyBehavior") as EnemyBehavior;
            PlayerBehavior player = coll.gameObject.GetComponent("PlayerBehavior") as PlayerBehavior;
            //O player só recebe dano se o inimigo estiver no estado de ataque e se não estiver defendendo(o dano é redirecionado pro escudo)
            if (inimigo.getEstado() == "attack" && player.getDefendendo()==false)
            {
                inimigo.incrementarNumGolpes();//conta o número de golpes que inimigo realiza
                int dano = getDanoBase();
                if (this.critico()) {
                    dano = this.danoBase * this.multDeCritico;
                }
                player.tomarDano(dano);
            }
		}

		if (coll.gameObject.tag == "inimigo" && portador.tag=="Player") {
            EnemyBehavior inimigo = coll.gameObject.GetComponent("EnemyBehavior") as EnemyBehavior;
            PlayerBehavior player = portador.GetComponent("PlayerBehavior") as PlayerBehavior;
            if (player.getEstado() == "attack") {
                int dano = getDanoBase();
                if (this.critico()) {
                    dano = this.danoBase * this.multDeCritico;
                }
                inimigo.tomarDano(dano);
                player.setEstado(Estado_Do_Player.idle);
            }
		}
		
	}


}