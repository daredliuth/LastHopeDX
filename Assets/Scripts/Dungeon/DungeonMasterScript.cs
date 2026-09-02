using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Sala
{
    /*Enum para definir el tipo de sala que se creará.*/
    public enum TipoSala
    {
        INICIAL,
        MIXTA,
        ACERTIJO,
        COMBATE,
        JEFE,
        GRADA
    }
    public Vector3 tamPared;
    public Vector3 tamArco;
    public Vector3 tamPiso;
    public Vector2 tam;
    public Vector2 origen;
    public TipoSala tipo;
    public int nivel;
    public int[] puertas = {0,0,0,0};

    public Sala(Vector3 _tamPared, Vector3 _tamArco, Vector3 _tamPiso, Vector2 _tam, Vector2 _origen, TipoSala _tipo, int _nivel, int[] _puertas)
    {
        tamPared = _tamPared;
        tamArco = _tamArco;
        tamPiso = _tamPiso;
        tam = _tam;
        origen = _origen;
        tipo = _tipo;
        nivel = _nivel;
        puertas = _puertas;
    }

    public Sala()
    {
        tamPared = new Vector3(2.5f,2.5f,0.5f);
        tamArco = new Vector3(2.5f,2.5f,0.5f);
        tamPiso = new Vector3(2.5f,0.5f,2.5f);
        tam = new Vector2(12.5f,12.5f);
        origen = new Vector2(0,0);
        tipo = TipoSala.MIXTA;
        nivel = 1;
        for(int i=0; i<puertas.Length; i++)
        {
            puertas[i] = 1;
        }
    }

    public Sala(Vector2 _origen, TipoSala _tipo, int[] _puertas, int _nivel)
    {
        tamPared = new Vector3(2.5f,2.5f,0.5f);
        tamArco = new Vector3(2.5f,2.5f,0.5f);
        tamPiso = new Vector3(2.5f,0.5f,2.5f);
        tam = new Vector2(12.5f,12.5f);
        origen = _origen;
        tipo = _tipo;
        nivel = _nivel;
        puertas = _puertas;
    }


}

public class DungeonMasterScript : MonoBehaviour
{
    private List<Sala> listaSalas = new List<Sala>();
    [SerializeField] GameObject pared;
    [SerializeField] GameObject piso;
    [SerializeField] GameObject arco;

    void Start()
    {
        int[] arregloAux = {0,0,0,1};
        listaSalas.Add(new Sala());
        listaSalas.Add(new Sala(new Vector2(12.5f,0), Sala.TipoSala.COMBATE, arregloAux, 1));

        for(int i=0; i<listaSalas.Count; i++)
        {
            CrearSala(listaSalas[i]);
        }
    }

    private void CrearSala(Sala salaNueva)
    {
        //Creación del piso.
        for (int i=0; i<(salaNueva.tam.x/salaNueva.tamPiso.x); i++)
        {
            for(int j=0; j<(salaNueva.tam.y/salaNueva.tamPiso.z); j++)
            {
                Instantiate(piso, new Vector3(salaNueva.origen.x + (salaNueva.tamPiso.x/2) + (salaNueva.tamPiso.x*j), 0f, salaNueva.origen.y + (salaNueva.tamPiso.z*i)), Quaternion.Euler(0,0,0));
            }
        }

        //Creación de las paredes y arcos.
        //Sur
        for (int i=0; i<(salaNueva.tam.x/salaNueva.tamPared.x); i ++)
        {
            if (salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i) == salaNueva.tam.x/2 && salaNueva.puertas[0] == 1)
            {
                Instantiate(arco, new Vector3(salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i), 1.25f, salaNueva.origen.y), Quaternion.Euler(0,0,0));
            }
            else
            {
                Instantiate(pared, new Vector3(salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i), 1.25f, salaNueva.origen.y), Quaternion.Euler(0,0,0));
            }
        }
        //Este
        for (int i=0; i<(salaNueva.tam.y/salaNueva.tamPared.y); i ++)
        {
            if (salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i) == salaNueva.tam.y/2 && salaNueva.puertas[1] == 1)
            {
                Instantiate(arco, new Vector3(salaNueva.origen.x + salaNueva.tam.x, 1.25f, salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i)), Quaternion.Euler(0,-90,0));
            }
            else
            {
                Instantiate(pared, new Vector3(salaNueva.origen.x + salaNueva.tam.x, 1.25f, salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i)), Quaternion.Euler(0,-90,0));
            }
        }
        //Norte
        for (int i=0; i<(salaNueva.tam.x/salaNueva.tamPared.x); i ++)
        {
            if (salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i) == salaNueva.tam.x/2 && salaNueva.puertas[2] == 1)
            {
                Instantiate(arco, new Vector3(salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i), 1.25f, salaNueva.origen.y+salaNueva.tam.y), Quaternion.Euler(0,180,0));
            }
            else
            {
                Instantiate(pared, new Vector3(salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i), 1.25f, salaNueva.origen.y+salaNueva.tam.y), Quaternion.Euler(0,180,0));
            }
        }
        //Oeste
        for (int i=0; i<(salaNueva.tam.y/salaNueva.tamPared.y); i ++)
        {
            if (salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i) == salaNueva.tam.y/2 && salaNueva.puertas[3] == 1)
            {
                Instantiate(arco, new Vector3(salaNueva.origen.x, 1.25f, salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i)), Quaternion.Euler(0,90,0));
            }
            else
            {
                Instantiate(pared, new Vector3(salaNueva.origen.x, 1.25f, salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i)), Quaternion.Euler(0,90,0));
            }
        }
    }
}
