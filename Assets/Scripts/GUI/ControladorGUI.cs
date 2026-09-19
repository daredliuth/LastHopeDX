using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorGUI : MonoBehaviour
{
 private GameObject hud, pausa, fin, ninten;
    private Image barraMagia;
    private GameObject[] zanahorias = new GameObject[10];
    private int maxMagia = 100;
    private TMP_Text textoMagia;
    private Button btnRegresarPausa, btnOpcionesPausa, btnSalirPausa, btnReintentarFin, btnSalirFin;
    private void Awake()
    {
        //Obtenemos la referencia de cada canvas.
        hud = GameObject.FindGameObjectWithTag("HUD");
        pausa = GameObject.FindGameObjectWithTag("Pausa");
        fin = GameObject.FindGameObjectWithTag("Fin");

        //Obtenemos la referencia al jugador.
        ninten = GameObject.FindGameObjectWithTag("Ninten");

        //Obtenemos la referencia a los botones de los canvas.
        btnRegresarPausa = pausa.transform.GetChild(2).gameObject.GetComponent<Button>();
        btnSalirPausa = pausa.transform.GetChild(3).gameObject.GetComponent<Button>();
        btnReintentarFin = fin.transform.GetChild(3).gameObject.GetComponent<Button>();
        btnSalirFin = fin.transform.GetChild(4).gameObject.GetComponent<Button>();

        //Añadimos los eventos al dar click a los botones.
        btnRegresarPausa.onClick.AddListener(ActivarHUD);
        btnSalirPausa.onClick.AddListener(CerrarJuego);
        btnReintentarFin.onClick.AddListener(ReiniciarEscena);
        btnSalirFin.onClick.AddListener(CerrarJuego);

        for(int i=0; i<10; i++)
        {
            zanahorias[i] = GameObject.Find("Zanahoria" + (i+1));
            if(zanahorias[i] == null)
            {
                Debug.Log("No se encontro la xanahoria");
            }
        }

        barraMagia = GameObject.Find("RellenoMagia").GetComponent<Image>();
        textoMagia = GameObject.Find("TextoMagia").GetComponent<TMP_Text>();
    }

    private void Start() {
        ActivarHUD();
    }
    void Update()
    {
        if(ninten.GetComponent<ControladorNinten>().GetVida() <= 0)
        {
            ActivarFin();
        }
        barraMagia.fillAmount = (float)ninten.GetComponent<ControladorNinten>().GetMagia()/maxMagia;
        textoMagia.text = $"{ninten.GetComponent<ControladorNinten>().GetMagia()}/100";
    }

    public void DisminuirZanahorias(int zanahoria)
    {
        zanahorias[zanahoria].SetActive(false);
    }

    public void AumentarZanahorias(int zanahoria)
    {
        zanahorias[zanahoria-1].SetActive(true);
    }

    //Activa el HUD y desactiva las demás interfaces.
    public void ActivarHUD()
    {
        if(ninten.GetComponent<ControladorNinten>().GetVida() > 0)
        {
            hud.SetActive(true);
            pausa.SetActive(false);
            fin.SetActive(false);
            Time.timeScale = 1f;
        }

    }

    public void ActivarPausa()
    {
        if(ninten.GetComponent<ControladorNinten>().GetVida() > 0)
        {
            hud.SetActive(false);
            pausa.SetActive(true);
            fin.SetActive(false);
            Time.timeScale = 0f; 
        }
        
    }

    public void ActivarFin()
    {
        hud.SetActive(false);
        pausa.SetActive(false);
        fin.SetActive(true);
        Time.timeScale = 0f;
    }

    private void MostrarOpciones()
    {
        Debug.Log("Aquí modificamos el audio en la siguiente práctica.");
    }

    private void CerrarJuego()
    {
        Debug.Log("Cerrando el juego.");
        Application.Quit();
    }

    private void ReiniciarEscena()
    {
        Debug.Log("Reiniciando escena.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
