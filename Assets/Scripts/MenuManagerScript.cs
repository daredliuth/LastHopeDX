using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerScript : MonoBehaviour
{
 [SerializeField] private Button btnHistoria;
    [SerializeField] private Button btnSupervivencia;
    [SerializeField] private Button btnSalir;

    private void Awake()
    {
        // Asignar eventos a los botones
        btnHistoria.onClick.AddListener(AbrirHistoria);
        btnSupervivencia.onClick.AddListener(AbrirSupervivencia);
        btnSalir.onClick.AddListener(SalirDelJuego);
    }

    private void AbrirHistoria()
    {
        SceneManager.LoadScene("HistoriaEscena");
    }

    private void AbrirSupervivencia()
    {
        SceneManager.LoadScene("SupervivenciaEscena");
    }

    private void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
