using System;
using UnityEngine;

public class EnemigoScript : MonoBehaviour
{
    public enum EstadoEnemigo{IDLE, PATRULLANDO, PERSIGUIENDO, ATACANDO, MUERTO}
    [SerializeField] private int vida;
    [SerializeField] private int rangoAtaque;
    [SerializeField] private int rangoPerseguir;
    [SerializeField] private EstadoEnemigo estado;
    [SerializeField] private int ataque = 1;
    [SerializeField] private float velocidad;
    [SerializeField] private float velocidadRotacion;
    [SerializeField] private float timerAtaque = 0;
    [SerializeField] private float cooldownAtaque;

    #region GetSet
    public void SetVida(int _vida)
    {
        vida = _vida;
    }

    public int GetVida()
    {
        return vida;
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
            case "MUERTO":
                estado = EstadoEnemigo.MUERTO;
            break;
            default:
                estado = EstadoEnemigo.IDLE;
            break;
        }
    }

    public String GetEstado()
    {
        switch (estado)
        {
            case EstadoEnemigo.IDLE: return "IDLE";
            case EstadoEnemigo.PATRULLANDO: return "PATRULLANDO";
            case EstadoEnemigo.PERSIGUIENDO: return "PERSIGUIENDO";
            case EstadoEnemigo.ATACANDO: return "ATACANDO";
            case EstadoEnemigo.MUERTO: return "MUERTO";
            default: return "IDLE";
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

    public int GetAtaque()
    {
        return ataque;
    }

    public void SetTimerAtaque(float _timerAtaque)
    {
        timerAtaque = _timerAtaque;
    }

    public float GetTimerAtaque()
    {
        return timerAtaque;
    }

    public void SetCooldownAtaque(float cooldown)
    {
        cooldownAtaque = cooldown;
    }

    public float GetCooldownAtaque()
    {
        return cooldownAtaque;
    }
    #endregion

    #region Funciones auxiliares.
    public int RecibirAtaque(int _ataque)
    {
        vida -= _ataque;
        Debug.Log($"Vida enemigo: {vida}");
        return vida;
    }

    public bool PuedeAtacar()
    {
        return (estado == EstadoEnemigo.ATACANDO);
    }

    public float DistanciaJugador(GameObject jugador)
    {
        float distancia = (jugador.transform.position - this.transform.position).sqrMagnitude;
        //Debug.Log($"Distancia a Ninten: {distancia}");
        return distancia;
    }

    public int Morir(int puntos)
    {
        //La animación para morir se hace directamente en el Script de cada enemigo a través de una comprobación de la vida en cada update.
        estado = EstadoEnemigo.MUERTO;
        Destroy(this.gameObject, 10f);
        return puntos;//Puntos por derrotar enemigos.
    }
    #endregion
}
