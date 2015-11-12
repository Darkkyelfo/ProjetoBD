using UnityEngine;
using System.Collections;

public class ControleDrop : MonoBehaviour {

    /*Essa classe é responsavel por realizar o sorteio, avaliando quais itens vão ser dropados
    ao realizar o sortei a classe cria um objeto do tipo "saquinho" onde os itens sorteidos são
    armazenados para o player coletar*/
    private ArrayList listaItens= new ArrayList();//array de itens que vai armazenar os itens que podem ser dropados
    private ArrayList probabilidades = new ArrayList();/*array que armazena a probabilidade de dropar os itens do array acima
    ex: probabilidades[0]==0.2, indica que o listaItens[0] tem 20% de chance de dropar*/
    int[] selecionados;
    public int numItensDropados=1;//Determina quantos itens vão ser dropados
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void droparItem(Vector3 posicaoDoDrop)
    {
        this.selecionados = new int[this.numItensDropados];//armazena os itens que vão ser dropados
        int cont= 0;
        while (cont < this.numItensDropados){
            float numAleatorio = Random.Range(0, 1.0f);//gera um número aleatorio
            foreach (float i in this.probabilidades) {
                if (numAleatorio <= i) {//se o número aleatorio for menor ou igual a probablidade,o item será dropado 
                    selecionados[cont] = this.probabilidades.IndexOf(i);
                }
            }
            cont++;
        }
        this.instanciarNaCena(posicaoDoDrop);
        }

    //função responsavel por instanciar o drop na cena.
    //recebe o nome do item, seu tipo(arma,escudo e etc)
    //a posicao é o local onde o item vai ser instanciado(vai ser a posição do mob)  
    private void instanciarNaCena(Vector3 posicaoDoDrop)
    {
        GameObject saquinhoDropObj= (GameObject)Resources.Load("Prefabs/Outros/drop");
        posicaoDoDrop = new Vector3(posicaoDoDrop.x, posicaoDoDrop.y - 2, posicaoDoDrop.z);
        GameObject saquinhoNaCena = Instantiate(saquinhoDropObj, posicaoDoDrop, new Quaternion(0, 0, 0, 0)) as GameObject;//coloca o saquinho na cena
        SaquinhoDrop saquinho = saquinhoNaCena.GetComponent("SaquinhoDrop") as SaquinhoDrop;

        foreach (int i in this.selecionados) {//instancia na cena os itens dropados
            Itens item = (Itens)this.listaItens[i];
            saquinho.pegarGameObject(item);
        }
    }

    public ArrayList getListaItens()
    {
        return this.listaItens;
    }
    public ArrayList getProbabilidades()
    {
        return this.probabilidades;
    }
}
