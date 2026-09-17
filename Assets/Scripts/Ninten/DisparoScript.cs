using UnityEngine;

public class DisparoScript : MonoBehaviour
{
    private float tiempoVida = 3f;

    void Start()
    {
        Destroy(this.gameObject, tiempoVida);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemigo")
        {
            ControladorNinten controladorNinten = GameObject.FindGameObjectWithTag("Ninten").GetComponent<ControladorNinten>();;
            other.GetComponent<EnemigoScript>().RecibirAtaque(controladorNinten.GetAtaqueDistancia());
            Destroy(this.gameObject);
        }
    }
}
