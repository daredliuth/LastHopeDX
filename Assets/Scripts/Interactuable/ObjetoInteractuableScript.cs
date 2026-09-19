using UnityEngine;

public class ObjetoInteractuableScript : MonoBehaviour
{
    public void Interactuar()
    {
        if(gameObject.tag == "Grada")
        {
            gameObject.GetComponent<GradaScript>().CurarSeguir();
        }
        else
        {
            
        }
    }
}
