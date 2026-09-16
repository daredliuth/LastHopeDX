using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using Unity.VisualScripting;
using System;


public class ControladorNinten : MonoBehaviour
{
    #region VariablesJugador
    //Enum con los estados posibles del jugador.
    private enum EstadoJugador{IDLE, CAMINANDO, CORRIENDO, SALTANDO, APUNTANDO, FIJANDO, ATACANDO, DEFENDIENDO, PAUSA}

    //[Header("Movimiento")]
    //[SerializeField]
    private float velocidadMovimiento = 0.25f;
    //[SerializeField]
    private float fuerzaSalto = 3.0f;
    private Vector2 entradaMovimiento;
    private Vector3 movimiento, forwardCamara, rightCamara;
    Quaternion rotacionNinten;
    private float velocidadRotacion = 5f;
    private EstadoJugador estado = EstadoJugador.IDLE;

    //[Header("Datos jugador")]
    //[SerializeField]
    private int vida = 10;
    private int vidaAnterior = 10;
    //[SerializeField]
    private int cantidadMagia = 100;
    //[SerializeField]
    private int puntuacion = 0;
    private int bufMagia = 2;
    private int ataque = 1;
    #endregion

    #region Banderas
    private bool interactuar = false;
    private bool magia = false;
    #endregion

    #region ObjetosExternos
    private AccionesNinten accionesEntrada;
    private Rigidbody rigidBodyNinten;
    private CinemachineCamera camaraSeguimiento, camaraOrbital, camaraDisparo;
    private CinemachineBrain cinemachineCerebro;
    GameObject dungeonMaster;
    GameObject enemigoFijado;
    Collider objetoInteractuable;
    private Animator animatorJugador;
    //private ControladorAudio controladorAudio;
    //private ControladorGUI controladorGUI;
    #endregion

    void Awake()
    {
        accionesEntrada = new AccionesNinten();
        rigidBodyNinten = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        accionesEntrada.Enable();
        //Suscripción de las funciones a su respectiva acción.
        //accionesEntrada.MapaAccionesNinten.Movimiento.performed += OnMovimiento;
        accionesEntrada.MapaAccionesNinten.SaltoInteraccionConfirmar.performed += OnSaltoInteraccionConfirmar;
        accionesEntrada.MapaAccionesNinten.DistanciaCancelar.performed += OnDistanciaCancelar;
        accionesEntrada.MapaAccionesNinten.Magia.performed += OnMagia;
        accionesEntrada.MapaAccionesNinten.Atacar.performed += OnAtacar;
        accionesEntrada.MapaAccionesNinten.Escudo.performed += OnEscudo;
        accionesEntrada.MapaAccionesNinten.FijarDisparar.performed += OnFijarDisparar;
        accionesEntrada.MapaAccionesNinten.Pausa.performed += OnPausa;
        //accionesEntrada.MapaAccionesNinten.Camara.performed += OnCamara;

        //Seleción de las cámaras.
        camaraSeguimiento = GameObject.FindGameObjectWithTag("CamaraSeguimiento").GetComponent<CinemachineCamera>();
        camaraOrbital = GameObject.FindGameObjectWithTag("CamaraOrbital").GetComponent<CinemachineCamera>();
        camaraDisparo = GameObject.FindGameObjectWithTag("CamaraDisparo").GetComponent<CinemachineCamera>();
        cinemachineCerebro = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();
        camaraOrbital.Prioritize();
    }

    void Start()
    {
        vidaAnterior = vida;
    }

    #region OnControlador
    void OnDisable()
    {
        //Suscripción de las funciones a su respectiva acción.
        //accionesEntrada.MapaAccionesNinten.Movimiento.performed -= OnMovimiento;
        accionesEntrada.MapaAccionesNinten.SaltoInteraccionConfirmar.performed -= OnSaltoInteraccionConfirmar;
        accionesEntrada.MapaAccionesNinten.DistanciaCancelar.performed -= OnDistanciaCancelar;
        accionesEntrada.MapaAccionesNinten.Magia.performed -= OnMagia;
        accionesEntrada.MapaAccionesNinten.Atacar.performed -= OnAtacar;
        accionesEntrada.MapaAccionesNinten.Escudo.performed -= OnEscudo;
        accionesEntrada.MapaAccionesNinten.FijarDisparar.performed -= OnFijarDisparar;
        accionesEntrada.MapaAccionesNinten.Pausa.performed -= OnPausa;
        //accionesEntrada.MapaAccionesNinten.Camara.performed -= OnCamara;
        accionesEntrada.Disable();
    }

    void Update()
    {
        entradaMovimiento = accionesEntrada.MapaAccionesNinten.Movimiento.ReadValue<Vector2>();

        //Selección de camara para el movimiento
        switch (estado)
        {
            case EstadoJugador.FIJANDO:
                forwardCamara = camaraSeguimiento.transform.forward;
                rightCamara = camaraSeguimiento.transform.right;
            break;
            case EstadoJugador.APUNTANDO:
                forwardCamara = new Vector3(0,0,0);
                rightCamara = forwardCamara;
            break;
            case EstadoJugador.PAUSA:
                forwardCamara = new Vector3(0,0,0);
                rightCamara = forwardCamara;
            break;
            default:
                forwardCamara = camaraOrbital.transform.forward;
                rightCamara = camaraOrbital.transform.right;
            break;
        }
        forwardCamara.y = 0;
        rightCamara.y = 0;
        forwardCamara.Normalize();
        rightCamara.Normalize();
        movimiento = (forwardCamara * entradaMovimiento.y + rightCamara * entradaMovimiento.x) * velocidadMovimiento;
        rigidBodyNinten.MovePosition(rigidBodyNinten.position + movimiento * Time.fixedDeltaTime);

        if(movimiento.sqrMagnitude > 0.001f)
        {
            rotacionNinten = Quaternion.LookRotation(movimiento, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionNinten, Time.deltaTime * velocidadRotacion);
        }
    }

    private void OnSaltoInteraccionConfirmar(InputAction.CallbackContext contexto)
    {
        if(estado == EstadoJugador.PAUSA)
        {
            //Estamos en pausa, confirmar la opción seleccionada.
        }
        else//No estamos en pausa.
        {
            if (interactuar)//Hay un objeto con el que podemos interactuar.
            {
                //animatorNinten.SetTrigger("Interactuar");
                if(objetoInteractuable != null)
                {
                    //Interactuamos con el objeto.
                    //StartCoroutine(objetoInteractuable.GetComponent<interaccionScript>().Interactuar(cinemachineCerebro, camaraOrbital));
                }
            }
            else if(rigidBodyNinten.linearVelocity.y == 0)//Saltamos si no estamos en movimiento en Y.
            {
                //animatorNinten.SetTrigger("Saltar");
                rigidBodyNinten.AddForce(new Vector3(0,fuerzaSalto,0), ForceMode.Impulse);
            }
        }
    }

    private void OnDistanciaCancelar(InputAction.CallbackContext contexto)
    {
        if(estado == EstadoJugador.PAUSA)
        {
            //Estamos en pausa, regresar o salir de la pausa.
        }
        else//No estamos en pausa.
        {
            if(estado == EstadoJugador.APUNTANDO)//Regresamos a la cámara orbital.
            {
                estado = EstadoJugador.IDLE;
                cinemachineCerebro.DefaultBlend.Time = 0.5f;
                camaraOrbital.Prioritize();
            }
            else//Cambiamos a la cámara de disparo.
            {
                estado = EstadoJugador.APUNTANDO;
                cinemachineCerebro.DefaultBlend.Time = 0;
                camaraDisparo.Prioritize();
            }
        }
    }

    private void OnMagia(InputAction.CallbackContext contexto)
    {
        if(estado != EstadoJugador.PAUSA)//Actuar solo si no estamos en pausa.
        {
            if (magia)
            {
                ataque /= bufMagia;
                //Desactivamos efecto especial (particulas).
            }
            else
            {
                ataque *= bufMagia;
                //Activamos efecto especial (particulas).
            }
            magia = !magia;
        }
    }

    private void OnAtacar(InputAction.CallbackContext contexto)
    {
        if(estado != EstadoJugador.PAUSA && PuedeAtacar())//Actuar solo si no estamos en pausa.
        {
            if(estado == EstadoJugador.FIJANDO)//No cambiamos el estado ya que debe seguir fijando.
            {
                //Animación de atacar.
                Debug.Log("Ataque fijado.");
            }
            else
            {
                //Animación de atacar.
                estado = EstadoJugador.ATACANDO;
                Debug.Log("Ataque.");
            }
        }
    }

    private void OnEscudo(InputAction.CallbackContext contexto)
    {
        if(estado != EstadoJugador.PAUSA)//Actuar solo si no estamos en pausa.
        {
            if(estado == EstadoJugador.DEFENDIENDO)
            {
                estado = EstadoJugador.IDLE;
                //Regresamos a Idle.
            }
            else
            {
                estado = EstadoJugador.DEFENDIENDO;
                //Animacion de defender.
            }
            
        }
    }

    private void OnFijarDisparar(InputAction.CallbackContext contexto)
    {
        if(estado != EstadoJugador.PAUSA)
        {
            if (estado == EstadoJugador.APUNTANDO){
                Debug.Log("Disparando.");
                //Crear la lógica del disparo.
            }
            else
            {
                if (estado == EstadoJugador.FIJANDO)
                {
                    estado = EstadoJugador.IDLE;
                    cinemachineCerebro.DefaultBlend.Time = 0.5f;
                    camaraOrbital.Prioritize();
                }
                else
                {
                    enemigoFijado = EnemigoMasCercano(GameObject.FindGameObjectsWithTag("Enemigo"));
                    if(enemigoFijado != null)
                    {
                        estado = EstadoJugador.FIJANDO;
                        //camaraSeguimiento.Follow = enemigoFijado.transform;
                        camaraSeguimiento.LookAt = enemigoFijado.transform;
                        cinemachineCerebro.DefaultBlend.Time = 1.5f;
                        camaraSeguimiento.Prioritize();
                    }
                }
            }
        }
    }

    private void OnPausa(InputAction.CallbackContext contexto)
    {
        if(estado == EstadoJugador.PAUSA)
        {
            //controladorGUI.ActivarHUD();
            //StartCoroutine(controladorAudio.FadeMixerVolume("MusicVolume", -25f, 1f));
            //StartCoroutine(controladorAudio.FadeMixerVolume("MusicVolume", -15f, 2f));
            //controladorAudio.ReproducirCancion(0);
            Time.timeScale = 1f;
            estado = EstadoJugador.IDLE;
        }
        else
        {
            //controladorGUI.ActivarPausa();
            //StartCoroutine(controladorAudio.FadeMixerVolume("MusicVolume", -25f, 1f));
            //StartCoroutine(controladorAudio.FadeMixerVolume("MusicVolume", 5f, 1f));
            //controladorAudio.ReproducirCancion(1);
            Time.timeScale = 0f;
            estado = EstadoJugador.PAUSA;
        }

    }
    #endregion

    #region FuncionesAuxiliares
    GameObject EnemigoMasCercano(GameObject[] enemigos)
    {
        int indiceCercano = 0;
        float distanciaMenor = 99999;
        float distancia = 0;
        int nEnemigos = enemigos.Length;

        if(nEnemigos == 0){return null;}
        else
        {
            for(int i=0; i < nEnemigos; i++)
            {
                Debug.Log($"Enemigo {i}");
                distancia = (transform.position - enemigos[i].transform.position).sqrMagnitude;
                if(distancia < distanciaMenor){
                    distanciaMenor = distancia;
                    indiceCercano = i;
                }
            }
            return enemigos[indiceCercano];
        }
    }

    private bool PuedeAtacar()
    {
        if(estado == EstadoJugador.IDLE || estado == EstadoJugador.CAMINANDO || estado == EstadoJugador.CORRIENDO || estado == EstadoJugador.FIJANDO || estado == EstadoJugador.ATACANDO){return true;}
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactuable")
        {
            interactuar = true;
            objetoInteractuable = other;
        }
    }

    private void OTriggerExit(Collider other)
    {
        if (other.tag == "Interactuable")
        {
            interactuar = false;
            objetoInteractuable = null;
        }
    }
    #endregion

    #region GetSet
    public int GetVida()
    {
        return vida;
    }

    public void SetVida(int vidaSetter)
    {
        vida = vidaSetter;
    }

    public int GetMagia()
    {
        return cantidadMagia;
    }

    public void SetMagia(int magiaSetter)
    {
        cantidadMagia = magiaSetter;
    }

    public int GetPuntuacion()
    {
        return puntuacion;
    }
    public void SetPuntuacion(int puntuacionGetter)
    {
        puntuacion = puntuacionGetter;
    }
    #endregion
}
