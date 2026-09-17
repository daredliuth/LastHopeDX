using UnityEngine;

public class AtaqueScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ninten") //&& animatorcontrollerisplayingAtaque
        {
            EnemigoScript enemigo = GetComponentInParent<EnemigoScript>();
            //EnemigoScript enemigo =  this.gameObject.GetComponentInParent<EnemigoScript>();
            if(enemigo.PuedeAtacar())
            {
                int vidaNinten = other.GetComponent<ControladorNinten>().RecibirAtaque(enemigo.GetAtaque());
                if(vidaNinten <= 0)
                {
                    //Desfijamos el objeto
                    other.GetComponent<ControladorNinten>().Morir();
                }
            }
        }
    }
}
