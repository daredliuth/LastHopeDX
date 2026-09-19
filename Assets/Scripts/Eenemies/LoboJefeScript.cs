using UnityEngine;

public class LoboJefeScript : MonoBehaviour
{
    EnemigoScript enemigoScript;
    GameObject ninten;
    private Rigidbody rigidBodyEnemigo;
    private Animator animatorEnemigo;
    bool perseguir = false;
    private Vector3 movimiento;
    private Quaternion rotacion;
    bool muerto = false;
    float timerInvocar = 0;
    float cooldownInvocar = 30;
    [SerializeField] GameObject lobo;

    void Awake()
    {
        enemigoScript = gameObject.GetComponent<EnemigoScript>();
        enemigoScript.SetVida(32);
        enemigoScript.SetRangoAtaque(1);
        enemigoScript.SetRangoPerseguir(10);
        enemigoScript.SetEstado("IDLE");
        enemigoScript.SetVelocidad(1.2f);
        enemigoScript.SetVelocidadRotacion(7f);
        enemigoScript.SetCooldownAtaque(7f);
        ninten = GameObject.FindGameObjectWithTag("Ninten");
        rigidBodyEnemigo = GetComponent<Rigidbody>();
        animatorEnemigo = GetComponent<Animator>();
    }

    void Update()
    {
        if (!muerto)
        {
            enemigoScript.SetTimerAtaque(enemigoScript.GetTimerAtaque() + Time.deltaTime);
            if(enemigoScript.DistanciaJugador(ninten) <= enemigoScript.GetRangoAtaque() && !animatorEnemigo.GetCurrentAnimatorStateInfo(0).IsName("Lobo_Jefe_Invocar"))
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
            else if(enemigoScript.DistanciaJugador(ninten) <= enemigoScript.GetRangoPerseguir() && !animatorEnemigo.GetCurrentAnimatorStateInfo(0).IsName("Lobo_Jefe_Invocar"))
            {
                enemigoScript.SetEstado("PERSIGUIENDO");
                animatorEnemigo.SetBool("movimiento", true);
                perseguir = true;
            }
            else if(!animatorEnemigo.GetCurrentAnimatorStateInfo(0).IsName("Lobo_Jefe_Invocar"))
            {
                enemigoScript.SetEstado("IDLE");
                animatorEnemigo.SetBool("movimiento", false);
                perseguir = false;
            }

            if (perseguir && !animatorEnemigo.GetCurrentAnimatorStateInfo(0).IsName("Lobo_Jefe_Invocar"))
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
                ninten.GetComponent<ControladorNinten>().AumentarPuntuacion(enemigoScript.Morir(2500));
                animatorEnemigo.SetTrigger("morir");
                //Debug.Log("Enemigo morido.");
            }

            timerInvocar += Time.deltaTime;
            if(timerInvocar > cooldownInvocar)
            {
                timerInvocar = 0;
                int pInvocar = Random.Range(0,10);
                //Debug.Log(pInvocar);
                if(pInvocar >= 7 && enemigoScript.GetEstado() != "IDLE")
                {
                    //Debug.Log("Invocar");
                    Invocar();
                }
            }
        }
    }

    private void Invocar()
    {
        animatorEnemigo.SetTrigger("invocar");
        Instantiate(lobo, transform.position, transform.rotation);
    }
}
