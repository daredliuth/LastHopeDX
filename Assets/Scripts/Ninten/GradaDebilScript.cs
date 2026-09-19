using UnityEngine;

public class GradaDebilScript : MonoBehaviour
{
    private GameObject ninten;
    private GameObject enemigo;
    private Vector3 posicion;
    private Quaternion rotacion;
    private Vector3 desfaseNinten = new Vector3(-0.5f,1,0);
    private Vector3 desfaseFijado = new Vector3(0,1.25f,0);
    private bool fijado = false;

    void OnEnable()
    {
        ninten = GameObject.FindGameObjectWithTag("Ninten");
        posicion = ninten.transform.position + desfaseNinten;
        rotacion = ninten.transform.rotation;
    }

    void Update()
    {
        if (fijado)
        {
            posicion = enemigo.transform.position + desfaseFijado;
            rotacion = enemigo.transform.rotation;
        }
        else
        {
            posicion = ninten.transform.position + desfaseNinten;
            rotacion = ninten.transform.rotation;
        }
        transform.position = posicion;
        transform.rotation = rotacion;
    }

    public void FijarEnemigo(GameObject _enemigo)
    {
        fijado = true;
        enemigo = _enemigo;
    }

    public void DesfijarEnemigo()
    {
        fijado = false;
        enemigo = null;
    }
}
