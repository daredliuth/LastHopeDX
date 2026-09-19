using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using Unity.IntegerTime;
using Unity.VisualScripting;


public class ControladorNinten : MonoBehaviour
{
    #region VariablesJugador
    //Enum con los estados posibles del jugador.
    private enum EstadoJugador{IDLE, CAMINANDO, CORRIENDO, SALTANDO, APUNTANDO, FIJANDO, ATACANDO, DEFENDIENDO, PAUSA, MUERTO}

    //[Header("Movimiento")]
    //[SerializeField]
    private float velocidadMovimiento = 0.35f;
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
    [SerializeField] private int puntuacion = 0;
    private int bufMagia = 2;
    private int ataque = 4;
    private int ataqueDistancia = 2;
    private float cooldownDistancia = 2f;
    private float timerDistancia = 0f;
    private float cooldownMagia = 1f;
    private float timerMagia = 0f;
    private float timerInvencible = 0f;
    private float coolDownInvencible = 3f;
    #endregion

    #region Banderas
    private bool interactuar = false;
    private bool magia = false;
    private bool puedeDisparar;
    private bool muerto = false;
    private bool invencible = false;
    #endregion

    #region ObjetosExternos
    private AccionesNinten accionesEntrada;
    private Rigidbody rigidBodyNinten;
    private CinemachineCamera camaraSeguimiento, camaraOrbital, camaraDisparo;
    private CinemachineBrain cinemachineCerebro;
    private GameObject dungeonMaster;
    private GameObject enemigoFijado;
    private GameObject grada;
    [SerializeField] private GameObject municionPrefab;

    private GameObject objetoInteractuable;
    private Animator animatorJugador;
    //private ControladorAudio controladorAudio;
    //private ControladorGUI controladorGUI;
    #endregion

    void Awake()
    {
        accionesEntrada = new AccionesNinten();
        rigidBodyNinten = GetComponent<Rigidbody>();
        animatorJugador = GetComponent<Animator>();
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

        //Obtenemos a Grada
        grada = GameObject.FindGameObjectWithTag("GradaDebil");
    }

    void Start()
    {
        camaraOrbital.Prioritize();
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
        //Debug.Log($"{estado}");
        switch (estado)
        {
            case EstadoJugador.FIJANDO:
                forwardCamara = camaraSeguimiento.transform.forward;
                rightCamara = camaraSeguimiento.transform.right;
            break;
            case EstadoJugador.APUNTANDO:
                //Debug.Log($"Estado: {estado}");
                forwardCamara = new Vector3(0,0,0);
                rightCamara = forwardCamara;
            break;
            case EstadoJugador.PAUSA:
                forwardCamara = new Vector3(0,0,0);
                rightCamara = forwardCamara;
            break;
            case EstadoJugador.DEFENDIENDO:
                forwardCamara = new Vector3(0,0,0);
                rightCamara = forwardCamara;
            break;
            case EstadoJugador.MUERTO:
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
        if(estado != EstadoJugador.APUNTANDO && estado != EstadoJugador.MUERTO)
        {
            animatorJugador.SetFloat("velocidad", entradaMovimiento.magnitude);
        }
        else
        {
            animatorJugador.SetFloat("velocidad", 0);
        }
        
        if(movimiento.sqrMagnitude > 0.001f)
        {
            rotacionNinten = Quaternion.LookRotation(movimiento, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionNinten, Time.deltaTime * velocidadRotacion);
        }

        timerDistancia += Time.deltaTime;
        

        if (magia && estado != EstadoJugador.MUERTO)
        {
            timerMagia += Time.deltaTime;
            if(timerMagia >= cooldownMagia)
            {
                timerMagia = 0;
                cantidadMagia --;
                //Debug.Log($"Magia: {cantidadMagia}");
                if(cantidadMagia == 0)
                {
                    ataque /= bufMagia;
                    ataqueDistancia /= bufMagia;
                    magia = false;
                    //Debug.Log($"{ataque}, {ataqueDistancia}");
                }
            }
        }

        if (invencible)
        {
            timerInvencible += Time.deltaTime;
            if(timerInvencible >= coolDownInvencible)
            {
                timerInvencible = 0;
                invencible = false;
            }
        }
    }

    private void OnSaltoInteraccionConfirmar(InputAction.CallbackContext contexto)
    {
        if(estado == EstadoJugador.PAUSA)
        {
            //Estamos en pausa, confirmar la opción seleccionada.
        }
        else if(estado != EstadoJugador.MUERTO)//No estamos en pausa.
        {
            if (interactuar)//Hay un objeto con el que podemos interactuar.
            {
                animatorJugador.SetTrigger("interactuar");
                if(objetoInteractuable != null)
                {
                    //Debug.Log($"Voy a interactuar con: {objetoInteractuable.name}");
                    objetoInteractuable.GetComponent<ObjetoInteractuableScript>().Interactuar();
                }
            }
            else if(rigidBodyNinten.linearVelocity.y <= 0.0001 && !animatorJugador.GetCurrentAnimatorStateInfo(0).IsName("Ninten_Saltar"))//Saltamos si no estamos en movimiento en Y.
            {
                //Debug.Log("saltar.");
                animatorJugador.SetTrigger("saltar");
                rigidBodyNinten.AddForce(new Vector3(0,fuerzaSalto,0), ForceMode.Impulse);
            }
        }
    }

    private void OnDistanciaCancelar(InputAction.CallbackContext contexto)
    {
        Debug.Log($"Ondistanciacancelar ele estado es: {estado}");
        if(estado == EstadoJugador.PAUSA)
        {
            //Estamos en pausa, regresar o salir de la pausa.
        }
        else if(estado != EstadoJugador.MUERTO)//No estamos en pausa.
        {
            if(estado == EstadoJugador.APUNTANDO)//Regresamos a la cámara orbital.
            {
                Debug.Log("Distancia cancelar lo mandé a aIDLE");
                estado = EstadoJugador.IDLE;
                cinemachineCerebro.DefaultBlend.Time = 0.5f;
                camaraOrbital.Prioritize();
            }
            else if(estado != EstadoJugador.DEFENDIENDO)
            {
                Debug.Log("Vamo a apuntar");
                estado = EstadoJugador.APUNTANDO;
                SetEnemigoFijado(null);
                cinemachineCerebro.DefaultBlend.Time = 0;
                camaraDisparo.Prioritize();
            }
        }
    }

    private void OnMagia(InputAction.CallbackContext contexto)
    {
        if(estado != EstadoJugador.PAUSA && estado != EstadoJugador.MUERTO)//Actuar solo si no estamos en pausa.
        {
            if (magia)
            {
                ataque /= bufMagia;
                ataqueDistancia /= bufMagia;
                magia = false;
                //Desactivamos efecto especial (particulas).
            }
            else if(cantidadMagia > 0)
            {
                ataque *= bufMagia;
                ataqueDistancia *= bufMagia;
                magia = true;
                //Activamos efecto especial (particulas).
            }
        }
    }

    private void OnAtacar(InputAction.CallbackContext contexto)
    {
        if(estado != EstadoJugador.PAUSA && PuedeAtacar() &&  estado != EstadoJugador.MUERTO)//Actuar solo si no estamos en pausa.
        {
            if(estado == EstadoJugador.FIJANDO)//No cambiamos el estado ya que debe seguir fijando.
            {
                animatorJugador.SetTrigger("atacar");
                //Debug.Log("Ataque fijado.");
            }
            else
            {
                animatorJugador.SetTrigger("atacar");
                estado = EstadoJugador.ATACANDO;
                //Debug.Log("Ataque.");
            }
        }
    }

    private void OnEscudo(InputAction.CallbackContext contexto)
    {
        if(estado != EstadoJugador.PAUSA  && estado != EstadoJugador.MUERTO)//Actuar solo si no estamos en pausa.
        {
            if(estado == EstadoJugador.DEFENDIENDO)
            {
                Debug.Log("Escudo lo mandé a IDLE");
                estado = EstadoJugador.IDLE;
                //Regresamos a Idle.
                animatorJugador.SetBool("defender", false);
            }
            else
            {
                estado = EstadoJugador.DEFENDIENDO;
                animatorJugador.SetBool("defender", true);
            }
        }
    }

    private void OnFijarDisparar(InputAction.CallbackContext contexto)
    {
        if(estado != EstadoJugador.PAUSA && estado != EstadoJugador.MUERTO)
        {
            Debug.Log($"En fijar/Disaparar el estado es: {estado}");
            if (estado == EstadoJugador.APUNTANDO){
                Debug.Log("Toy apuntadnod");
                if(timerDistancia > cooldownDistancia)
                {
                    Debug.Log("Disparé");
                    timerDistancia = 0;
                    //disparamos
                    GameObject municion = Instantiate(municionPrefab, camaraDisparo.transform.position, camaraDisparo.transform.rotation);
                    Rigidbody rb = municion.GetComponent<Rigidbody>();
                    rb.AddForce(municion.transform.forward * 20, ForceMode.Impulse);
                }
            }
            else
            {
                if (estado == EstadoJugador.FIJANDO)
                {
                    Debug.Log("Fijando lo mandé a OIDELÑ");
                    estado = EstadoJugador.IDLE;
                    cinemachineCerebro.DefaultBlend.Time = 0.5f;
                    camaraOrbital.Prioritize();
                    //Grada a Ninten.
                    grada.GetComponent<GradaDebilScript>().DesfijarEnemigo();
                }
                else if(estado != EstadoJugador.DEFENDIENDO)
                {
                    SetEnemigoFijado(EnemigoMasCercano(GameObject.FindGameObjectsWithTag("Enemigo")));
                    if(enemigoFijado != null)
                    {
                        estado = EstadoJugador.FIJANDO;
                        //camaraSeguimiento.Follow = enemigoFijado.transform;
                        camaraSeguimiento.LookAt = enemigoFijado.transform;
                        cinemachineCerebro.DefaultBlend.Time = 1.5f;
                        camaraSeguimiento.Prioritize();
                        //Grada a Enemigo
                        grada.GetComponent<GradaDebilScript>().FijarEnemigo(enemigoFijado);
                    }
                }
            }
        }
    }

    private void OnPausa(InputAction.CallbackContext contexto)
    {
        if(estado != EstadoJugador.MUERTO)
        {
            if(estado == EstadoJugador.PAUSA)
            {
                //controladorGUI.ActivarHUD();
                //StartCoroutine(controladorAudio.FadeMixerVolume("MusicVolume", -25f, 1f));
                //StartCoroutine(controladorAudio.FadeMixerVolume("MusicVolume", -15f, 2f));
                //controladorAudio.ReproducirCancion(0);
                Time.timeScale = 1f;
                Debug.Log("Pausa lo mande a IDLE");
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
                //Debug.Log($"Enemigo {i}");
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
        if (other.tag == "Interactuable" || other.tag == "Grada")
        {
            interactuar = true;
            objetoInteractuable = other.gameObject;
            //Debug.Log($"Entré a un objeto interactuable: {objetoInteractuable.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactuable" || other.tag == "Grada")
        {
            interactuar = false;
            objetoInteractuable = null;
            //Debug.Log("Salí de un objeto interactuable.");
        }
    }

    public int RecibirAtaque(int _ataque)
    {
        if(estado != EstadoJugador.DEFENDIENDO && !invencible)
        {
            invencible = true;
            vida -= _ataque;
        }
        //Modificar GUI.
        Debug.Log($"Vida Ninten: {vida}");
        return vida;
    }

    public int AumentarVida()
    {
        vida++;
        //Modificar GUI.
        Debug.Log($"Vida Ninten ++: {vida}");
        return vida;
    }

    public void Morir()
    {
        if (!muerto)
        {
            estado = EstadoJugador.MUERTO;
            muerto = true;
            invencible = false;
            //Animación muerte.
            animatorJugador.SetTrigger("morir");
            //Mostrar pantalla muerte.
        }
    }

    public void AumentarPuntuacion(int puntos)
    {
        puntuacion += puntos;
        //Modificar GUI.
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
        //Modificar GUI.
    }

    public int GetPuntuacion()
    {
        return puntuacion;
    }
    public void SetPuntuacion(int puntuacionGetter)
    {
        puntuacion = puntuacionGetter;
    }

    public int GetAtaque()
    {
        return ataque;
    }

    public int GetAtaqueDistancia()
    {
        return ataqueDistancia;
    }

    public void SetEnemigoFijado(GameObject enemigo)
    {
        enemigoFijado = enemigo;
        if (enemigoFijado == null)//Quitamos la fijación si no hay enemigo.
        {
            if(estado != EstadoJugador.APUNTANDO){ estado = EstadoJugador.IDLE;Debug.Log("setenemigo fijado lo mandé a IDLE"); }
            
            cinemachineCerebro.DefaultBlend.Time = 0.5f;
            camaraOrbital.Prioritize();
            grada.GetComponent<GradaDebilScript>().DesfijarEnemigo();
        }
    }
    #endregion
}
