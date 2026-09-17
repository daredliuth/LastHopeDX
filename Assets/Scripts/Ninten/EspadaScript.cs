using UnityEngine;

public class EspadaScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemigo") //&& animatorcontrollerisplayingAtaque
        {
            ControladorNinten controladorNinten = GameObject.FindGameObjectWithTag("Ninten").GetComponent<ControladorNinten>();
            if (controladorNinten.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Ninten_Atacar"))
            {
                int vidaEnemigo = other.GetComponent<EnemigoScript>().RecibirAtaque(controladorNinten.GetAtaque());
                if(vidaEnemigo <= 0)
                {
                    //Desfijamos el objeto
                    controladorNinten.SetEenemigoFijado(null);
                    other.GetComponent<EnemigoScript>().Morir();
                }
            }
        }
    }
}
