using UnityEngine;

public class GiiguScript : MonoBehaviour
{
EnemigoScript enemigoScript;
    GameObject ninten;
    private Rigidbody rigidBodyEnemigo;
    private Animator animatorEnemigo;
    bool perseguir = false;
    private Vector3 movimiento;
    private Quaternion rotacion;
    bool muerto = false;

    void Awake()
    {
        enemigoScript = gameObject.GetComponent<EnemigoScript>();
        enemigoScript.SetVida(32);
        enemigoScript.SetRangoAtaque(1);
        enemigoScript.SetRangoPerseguir(12);
        enemigoScript.SetEstado("IDLE");
        enemigoScript.SetVelocidad(1.75f);
        enemigoScript.SetVelocidadRotacion(8f);
        enemigoScript.SetCooldownAtaque(4f);
        ninten = GameObject.FindGameObjectWithTag("Ninten");
        rigidBodyEnemigo = GetComponent<Rigidbody>();
        animatorEnemigo = GetComponent<Animator>();
    }

    void Update()
    {
        if (!muerto)
        {
            enemigoScript.SetTimerAtaque(enemigoScript.GetTimerAtaque() + Time.deltaTime);
            if(enemigoScript.DistanciaJugador(ninten) <= enemigoScript.GetRangoAtaque())
            {
                if(enemigoScript.GetTimerAtaque() >= enemigoScript.GetCooldownAtaque())
                {
                    enemigoScript.SetTimerAtaque(0);
                    enemigoScript.SetEstado("ATACANDO");
                    animatorEnemigo.SetBool("movimiento", false);
                    animatorEnemigo.SetTrigger("atacar");
                    perseguir = true;
                }
            }
            else if(enemigoScript.DistanciaJugador(ninten) <= enemigoScript.GetRangoPerseguir())
            {
                enemigoScript.SetEstado("PERSIGUIENDO");
                animatorEnemigo.SetBool("movimiento", true);
                perseguir = true;
            }
            else
            {
                enemigoScript.SetEstado("IDLE");
                animatorEnemigo.SetBool("movimiento", false);
                perseguir = false;
            }

            if (perseguir)
            {
                movimiento = (ninten.transform.position - this.transform.position).normalized * enemigoScript.GetVelocidad();
                movimiento.y = 0;
                rigidBodyEnemigo.MovePosition(this.transform.position + movimiento * Time.fixedDeltaTime);
                if(movimiento.sqrMagnitude > 0.001f)
                {
                    rotacion = Quaternion.LookRotation(movimiento, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * enemigoScript.GetVelocidadRotacion());
                }
            }

            if(enemigoScript.GetVida() <= 0)
            {
                muerto = true;
                ninten.GetComponent<ControladorNinten>().AumentarPuntuacion(enemigoScript.Morir(500));
                animatorEnemigo.SetTrigger("morir");
                //Debug.Log("Enemigo morido.");
            }
        }
    }
}
