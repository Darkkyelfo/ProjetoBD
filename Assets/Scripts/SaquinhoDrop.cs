using UnityEngine;
using System.Collections;

public class SaquinhoDrop : MonoBehaviour {
    public SpriteRenderer iconePegar;
    private ArrayList itensDropados = new ArrayList();
    private bool exibirItens = false;
    private Inventario inventarioPlayer;
    private float lastClick = 0;

    // Use this for initialization
    void Start () {
        this.iconePegar.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (this.exibirItens) {
                this.exibirItens = false;
            }
            else {
                this.exibirItens = true;
            }
        }
        if (this.itensDropados.Count == 0) {
            this.exibirItens = false;
            Destroy(gameObject);
        }
	}


    void OnGUI()
    {
        if (this.exibirItens && this.iconePegar.enabled == true) {
            Rect janelaItensDropados = GUI.Window(0, new Rect(300, 150, 200, 200), GUIitensDropados, "Drops");
        }

    }
        void GUIitensDropados(int windowID)
    {
        GUI.BeginGroup(new Rect(0, 20, 200, 200));
        GUI.Box(new Rect(0, 0, 200, 200), "");
        byte cont = 1;
        foreach (GameObject i in this.itensDropados) {
            SpriteRenderer render = i.GetComponent("SpriteRenderer") as SpriteRenderer;
            Texture2D imagemItem = render.sprite.texture;//a partir do render ele obtem a imagem do prefab(Sprite)
            if (GUI.Button(new Rect(20, 40 * cont, 40, 40), imagemItem)) {
                if (Time.time - lastClick < 0.3) {
                    this.inventarioPlayer.addItem(i);
                    this.itensDropados.Remove(i);
                }
                }
            cont++;
            lastClick = Time.time;
        }
        GUI.EndGroup();
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "colisaoItem") {
            this.iconePegar.enabled = true;
            this.inventarioPlayer = coll.gameObject.GetComponentInParent<Inventario>() as Inventario;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "colisaoItem") {
            this.iconePegar.enabled = false;
            this.inventarioPlayer = null;
        }
    }
    //recebe o GameObject que foi dropado do mob
    public void pegarGameObject(Itens item)
    {
        GameObject itemObject = (GameObject)Resources.Load("Prefabs/" + item.getTipo() + "/" + item.getNome());//carrega o item da ser dropado
        this.itensDropados.Add(itemObject);//adiciona item a lista de itens para serem exibidos na GUI.

    }
}
