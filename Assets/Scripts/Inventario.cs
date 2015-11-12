using UnityEngine;
using System.Collections;



public class Inventario : MonoBehaviour {
	private GameObject [,] inventario = new GameObject[5,5];//matriz com os itens do heroi
	public GameObject armaAtual;
	public GameObject posicaoArma;
    public GameObject escudoAtual;
    public GameObject posicaoEscudo;
	public GameObject player;
	public GameObject objEstatus;//pega os status do player
	private bool estaComArma=false;
	private float lastClick = 0;//conta o intervalo entre os clicks do mouse
	//variaveis da GUI do inventario
	private Rect janelaInventario;
	private string statusText;
	private int tamJanelaX = 400;
	private int tamJanlelaY = 220;
	private int espacoBotaoX=20;
	private int espacoBotaoY=0;
	private bool gerarInv = false;//variavel de controle para exibir a inventario.
    private byte numMaxItens = 25;
    private byte numAtualDeItens = 0;

	void OnGUI() {
		if (gerarInv) {
			janelaInventario = GUI.Window (0, new Rect (300, 150, tamJanelaX, tamJanlelaY), GUIinventario, "Inventario");
		}
		}

	// Use this for initialization
	void Start () {
		//Acredito que aqui ficara a parte do banco, os itens serao carregados para o invetario do personagem
		inventario[0,0] = (GameObject)Resources.Load ("Prefabs/Armas/EspadaBasica", typeof(GameObject));
        this.numAtualDeItens++;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.I)) {
			if(gerarInv){
				gerarInv=false;
			}else{
				gerarInv=true;
			}
		}
	
	}

	void GUIinventario(int windowID){//funçao que cria a GUI do inventario
		GUI.BeginGroup (new Rect(0,20, 200, 200));
		GUI.Box(new Rect(0, 0, 200, 200), "");
		for (int i=0; i<5; i++) {
			for(int j=0;j<5;j++){
				if(inventario[i,j]!=null){
					//captura a imagem do item
					SpriteRenderer render = inventario[i,j].GetComponent("SpriteRenderer") as SpriteRenderer;//captura o render do prefab
					Texture2D imagemItem = render.sprite.texture;//a partir do render ele obtem a imagem do prefab(Sprite) 
					if(GUI.Button (new Rect (40*j, 40*i, 40, 40),imagemItem)){
						//aqui fica o código resposanvel por equipar os itens 
						//ao personagem
						if(Time.time-lastClick<0.3){//só usa o item ao se dar um "double click"
                            if(this.inventario[i, j].tag == "arma") {
                                this.equiparArma(inventario[i, j]);
                            }
                            else if(this.inventario[i, j].tag == "escudo") {
                                this.equiparEscudo(this.inventario[i, j]);
                            }
                            else if(this.inventario[i, j].tag == "pocao") {
                                this.usarPocao(inventario[i, j],i,j);
                            }

						}
						else{//um click exibe o status do item
                            if (this.inventario[i, j].tag == "arma") {
                                Arma armaSelecionada = inventario[i, j].GetComponent("Arma") as Arma;
                                Arma arma = armaSelecionada.GetComponent("Arma") as Arma;
                                arma.setPortador(objEstatus);
                                statusText = "Dano:" + armaSelecionada.getDanoBase()+"\n"+"Quantidade:"+armaSelecionada.getQuantidade();
                            }
                            else if(this.inventario[i, j].tag == "pocao") {
                                Pocao pocaoUsada = inventario[i, j].GetComponent("Pocao") as Pocao;
                                statusText = "Recupera:" + pocaoUsada.recuperacao + "\n" + "Quantidade:" + pocaoUsada.getQuantidade();
                            }
						}
						lastClick = Time.time;
					}
				}else{
					if(GUI.Button (new Rect (40*j, 40*i, 40, 40),""+i+""+j)){
						//caso nao tenha itens o slot ficara default
					}
				}
			}
		}
		GUI.EndGroup();
		GUI.Box(new Rect(200, 20, 200, 200), statusText);//box de informaçao dos itens

	}

	void equiparArma(GameObject item){
		//aqui ficara o cogido resposanvel por equipar os itens 
		//ao personagem
		if (armaAtual != null) {//caso ja tenha uma arma equipada ela sera destruida, para que outra a substitua
            Arma armaAserTrocada = armaAtual.GetComponent("Arma") as Arma;
            armaAserTrocada.removerBuffs();//retira os pontos extras trazidos por armas
            Destroy(armaAtual);
		}
		Vector3 posicao = posicaoArma.transform.position;//posicao da arma(precisa ser ajustada)
		//Coloca o player como portador da arma
		armaAtual = Instantiate(item,posicao,posicaoArma.transform.rotation) as GameObject;//instancia a arma na mao do player
		Arma arma = armaAtual.GetComponent ("Arma") as Arma;
        arma.portador = player;
		arma.setPortador (objEstatus);
		armaAtual.transform.parent=posicaoArma.transform;//transforma a arma em "filha" do player(assim ela se movera junto com ele)
		armaAtual.transform.localPosition = new Vector3 (0, 0, 0);
		armaAtual.transform.localScale = new Vector3 (1, 1, 1);

	}
    private void equiparEscudo(GameObject escudo)
    {
        //aqui ficara o cogido resposanvel por equipar os itens 
        //ao personagem
        if (this.escudoAtual != null) {//caso ja tenha uma arma equipada ela sera destruida, para que outra a substitua
            Escudo escudoAserTrocada = armaAtual.GetComponent("Escudo") as Escudo;
            escudoAserTrocada.removerBuffs();//retira os pontos extras trazidos por armas
            Destroy(armaAtual);
        }
        Vector3 posicao = this.posicaoEscudo.transform.position;//posicao da arma(precisa ser ajustada)
                                                         //Coloca o player como portador da arma
        armaAtual = Instantiate(escudo, posicao, this.posicaoEscudo.transform.rotation) as GameObject;//instancia a arma na mao do player
        Escudo escudoNovo = this.escudoAtual.GetComponent("Escudo") as Escudo;
        escudoNovo.portador = player;
        this.escudoAtual.transform.parent = this.posicaoEscudo.transform;//transforma a arma em "filha" do player(assim ela se movera junto com ele)
        this.escudoAtual.transform.localPosition = new Vector3(0, 0, 0);
        this.escudoAtual.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    private void usarPocao(GameObject pocao,int i,int j)
    {
        Pocao pocaoUsada = pocao.GetComponent("Pocao") as Pocao;
        pocaoUsada.portador = this.player;
        pocaoUsada.recuperarPlayer();
        Debug.Log(pocaoUsada.getQuantidade());
        pocaoUsada.setQuantidade(pocaoUsada.getQuantidade() - 1);
        if (pocaoUsada.getQuantidade() == 0) {//caso acabem as poções elas são removidas do inventário 
            this.removerItem(i, j);
        }


    }

    private void removerItem(int i,int j)
    {
        this.inventario[i, j] = null;
        this.numAtualDeItens++;
    }
    //Adiciona um item ao inventario
    public void addItem(GameObject novoItem)
    {
        if (this.numAtualDeItens <= this.numMaxItens) {
            bool pararLoop = false;
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 5; j++) {
                    if(this.inventario[i, j] != null) {
                        if(this.addItemRepetido(this.inventario[i, j], novoItem)) {
                            pararLoop = true;
                            break;
                        }
                    }
                    else if (this.inventario[i,j] == null) {
                        this.inventario[i, j] = novoItem;
                        this.numAtualDeItens--;
                        pararLoop = true;
                        break;
                    }
                }
                if (pararLoop) {
                    break;
                }
            }
        }
        else {
            Debug.Log("inventario cheio");
        }
    }
    //Verifica se o item é repetido(com o mesmo nome e status) e caso seja adiciona ao inventario 
    //como um incremento na quantidade atual de itens
    private bool addItemRepetido(GameObject itemAtual,GameObject itemAdicionado) {
        bool resultado = false;
        if(itemAtual.name == itemAdicionado.name) {
            if (itemAtual.tag == "arma") {
                Arma armaAtual = itemAtual.GetComponent("Arma") as Arma;
                Arma armaAserAdd = itemAdicionado.GetComponent("Arma") as Arma;
                if (armaAtual.status.ehIgual(armaAserAdd.status)) {
                    armaAtual.setQuantidade(armaAtual.getQuantidade() + 1);
                    resultado = true;
                }
            }
            else if(itemAtual.tag == "escudo") {
                Escudo escudoAtual = itemAtual.GetComponent("Escudo") as Escudo;
                Escudo escudoAserAdd = itemAdicionado.GetComponent("Escudo") as Escudo;
                if (escudoAtual.status.ehIgual(escudoAserAdd.status)) {
                    escudoAtual.setQuantidade(escudoAserAdd.getQuantidade() + 1);
                    resultado = true;
                }
            }
            else if(itemAtual.tag == "pocao") {
                Pocao pocaoAtual = itemAtual.GetComponent("Pocao") as Pocao;
                Pocao pocaoAserAdd = itemAdicionado.GetComponent("Pocao") as Pocao;
                if (pocaoAtual.status.ehIgual(pocaoAserAdd.status)) {
                    pocaoAtual.setQuantidade(pocaoAtual.getQuantidade() + 1);
                    resultado = true;
                }
            }
        }
        return resultado;

        }

}
