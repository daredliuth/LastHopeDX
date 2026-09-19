using System.Collections;
using UnityEngine;

public class GradaScript : MonoBehaviour
{
    private GameObject ninten;
    private GameObject dungeonMaster;
    private Animator animatorGrada;
    private Vector3 rotacion;
    void Awake()
    {
            animatorGrada = GetComponent<Animator>();
    }
    
    void OnEnable()
    {
        ninten = GameObject.FindGameObjectWithTag("Ninten");
        dungeonMaster = GameObject.FindGameObjectWithTag("DungeonMaster");
    }

    void Update()
    {
        rotacion = ninten.transform.position - transform.position;
        rotacion.y = 0;
        transform.rotation = Quaternion.LookRotation(rotacion);
    }

    public void CurarSeguir()
    {
        ControladorNinten controladorNinten = ninten.GetComponent<ControladorNinten>();
        while(controladorNinten.GetVida() < 10)
        {
            controladorNinten.AumentarVida();
        }
        controladorNinten.SetMagia(100);
        StartCoroutine(EsperarCurar());
    }

    IEnumerator EsperarCurar()
    {
        AnimatorStateInfo estadoPrevio = animatorGrada.GetCurrentAnimatorStateInfo(0);
        animatorGrada.SetTrigger("curar");
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo estadoActual = animatorGrada.GetCurrentAnimatorStateInfo(0);
            return estadoActual.fullPathHash != estadoPrevio.fullPathHash;
        });

        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo estadoActual = animatorGrada.GetCurrentAnimatorStateInfo(0);
            return estadoActual.normalizedTime >= 1f;
        });
        dungeonMaster.GetComponent<DungeonMasterScript>().AumentarNivel();

        //Esperamos a efectuar una animación de transición
        //Activar y desactivar GUI.
    }
}
