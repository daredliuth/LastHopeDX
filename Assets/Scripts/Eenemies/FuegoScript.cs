using UnityEngine;

public class FuegoScript : MonoBehaviour
{
    private float tiempoVida = 3f;

    void Start()
    {
        Destroy(this.gameObject, tiempoVida);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ninten")
        {
            other.GetComponent<ControladorNinten>().RecibirAtaque(1);
            Destroy(this.gameObject);
        }
    }
}
