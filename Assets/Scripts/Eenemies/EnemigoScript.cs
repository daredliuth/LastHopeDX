using UnityEngine;

public class EnemigoScript : MonoBehaviour
{
    public enum EstadoEnemigo{IDLE, PATRULLANDO, PERSIGUIENDO, ATACANDO}
    [SerializeField] private int vida;
    [SerializeField] private int rangoAtaque;
    [SerializeField] private int rangoPerseguir;
    [SerializeField] private EstadoEnemigo estado;
    [SerializeField] private int ataque = 1;
    [SerializeField] private float velocidad;
    [SerializeField] private float velocidadRotacion;

    public int RecibirAtaque(int ataque)
    {
        vida -= ataque;
        return vida;
    }

    #region GetSet
    public void SetVida(int _vida)
    {
        vida = _vida;
    }

    public void SetRangoAtaque(int rango)
    {
        rangoAtaque = rango;
    }

    public int GetRangoAtaque()
    {
        return rangoAtaque;
    }

    public void SetRangoPerseguir(int rango)
    {
        rangoPerseguir = rango;
    }

    public int GetRangoPerseguir()
    {
        return rangoPerseguir;
    }

    public void SetEstado(string _estado)
    {
        switch (_estado)
        {
            case "IDLE":
                estado = EstadoEnemigo.IDLE;
            break;
            case "PATRULLANDO":
                estado = EstadoEnemigo.PATRULLANDO;
            break;
            case "PERSIGUIENDO":
                estado = EstadoEnemigo.PERSIGUIENDO;
            break;
            case "ATACANDO":
                estado = EstadoEnemigo.ATACANDO;
            break;
            default:
                estado = EstadoEnemigo.IDLE;
            break;
        }
    }

    public void SetVelocidad(float _velocidad)
    {
        velocidad = _velocidad;
    }

    public float GetVelocidad()
    {
        return velocidad;
    }

    public void SetVelocidadRotacion(float _velocidad)
    {
        velocidadRotacion = _velocidad;
    }

    public float GetVelocidadRotacion()
    {
        return velocidadRotacion;
    }

    public float DistanciaJugador(GameObject jugador)
    {
        float distancia = (jugador.transform.position - this.transform.position).sqrMagnitude;
        //Debug.Log($"Distancia a Ninten: {distancia}");
        return distancia;
    }
    #endregion
}
