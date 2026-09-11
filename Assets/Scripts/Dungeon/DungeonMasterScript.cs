using UnityEngine;
using System.Collections.Generic;

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
    public List<GameObject> listaPisos = new List<GameObject>();
    public List<GameObject> listaParedes = new List<GameObject>();
    public List<GameObject> listaArcos = new List<GameObject>();

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
    private int maxSalas = 6;
    [SerializeField] GameObject pared;
    [SerializeField] GameObject piso;
    [SerializeField] GameObject arco;

    void Start()
    {
        //Variables auxiliares para la generación de las salas del nivel.
        Vector2 posicionSalaActual = new Vector2(0,0);
        int[] arcosSalaAnterior = {0,0,0,0};
        int nivel = 1;
        int actualSalas = 0;
        
        for(int i=0; i<maxSalas; i++)
        {
            if(i == 0)//Caso inicial.
            {
                listaSalas.Add(new Sala());
                arcosSalaAnterior = listaSalas[0].puertas;
            }
            else if(i == 6) //Sala de jefe (y sala de Grada).
            {
                //arregloFinal[i] = arreglo1[i] | arreglo2[i];
                listaSalas.Add(new Sala(posicionSalaActual, DeterminarTipoSala(5), DeterminarArcos(Random.Range(2,4), arcosSalaAnterior), nivel));
                listaSalas.Add(new Sala(posicionSalaActual, DeterminarTipoSala(6), DeterminarArcos(Random.Range(2,4), arcosSalaAnterior), nivel));
            }
            else //Cualquier sala.
            {
                //por cada arco en la sala anterior
                    //Crear sala de cada arco.
                    listaSalas.Add(new Sala(posicionSalaActual, DeterminarTipoSala(Random.Range(2,5)), DeterminarArcos(Random.Range(1,4), arcosSalaAnterior), nivel));
            }
        }
        
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
                salaNueva.listaPisos.Add(Instantiate(piso, new Vector3(salaNueva.origen.x + (salaNueva.tamPiso.x/2) + (salaNueva.tamPiso.x*j), 0f, salaNueva.origen.y + (salaNueva.tamPiso.z*i)), Quaternion.Euler(0,0,0)));
            }
        }

        //Creación de las paredes y arcos.
        //Sur
        for (int i=0; i<(salaNueva.tam.x/salaNueva.tamPared.x); i ++)
        {
            if (salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i) == salaNueva.tam.x/2 && salaNueva.puertas[0] == 1)
            {
                salaNueva.listaArcos.Add(Instantiate(arco, new Vector3(salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i), 1.25f, salaNueva.origen.y), Quaternion.Euler(0,0,0)));
            }
            else
            {
                salaNueva.listaParedes.Add(Instantiate(pared, new Vector3(salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i), 1.25f, salaNueva.origen.y), Quaternion.Euler(0,0,0)));
            }
        }
        //Este
        for (int i=0; i<(salaNueva.tam.y/salaNueva.tamPared.y); i ++)
        {
            if (salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i) == salaNueva.tam.y/2 && salaNueva.puertas[1] == 1)
            {
                salaNueva.listaArcos.Add(Instantiate(arco, new Vector3(salaNueva.origen.x + salaNueva.tam.x, 1.25f, salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i)), Quaternion.Euler(0,-90,0)));
            }
            else
            {
                salaNueva.listaParedes.Add(Instantiate(pared, new Vector3(salaNueva.origen.x + salaNueva.tam.x, 1.25f, salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i)), Quaternion.Euler(0,-90,0)));
            }
        }
        //Norte
        for (int i=0; i<(salaNueva.tam.x/salaNueva.tamPared.x); i ++)
        {
            if (salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i) == salaNueva.tam.x/2 && salaNueva.puertas[2] == 1)
            {
                salaNueva.listaArcos.Add(Instantiate(arco, new Vector3(salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i), 1.25f, salaNueva.origen.y+salaNueva.tam.y), Quaternion.Euler(0,180,0)));
            }
            else
            {
                salaNueva.listaParedes.Add(Instantiate(pared, new Vector3(salaNueva.origen.x + (salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i), 1.25f, salaNueva.origen.y+salaNueva.tam.y), Quaternion.Euler(0,180,0)));
            }
        }
        //Oeste
        for (int i=0; i<(salaNueva.tam.y/salaNueva.tamPared.y); i ++)
        {
            if (salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i) == salaNueva.tam.y/2 && salaNueva.puertas[3] == 1)
            {
                salaNueva.listaArcos.Add(Instantiate(arco, new Vector3(salaNueva.origen.x, 1.25f, salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i)), Quaternion.Euler(0,90,0)));
            }
            else
            {
                salaNueva.listaParedes.Add(Instantiate(pared, new Vector3(salaNueva.origen.x, 1.25f, salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i)), Quaternion.Euler(0,90,0)));
            }
        }
    }

    private int[] DeterminarArcos(int nArcos, int[] arcosAnterior)
    {
        int[] arreglo = arcosAnterior;
        int indiceActual = 0;
        int nActual = 0;

        for(int i=0; i<4; i++)
        {
            if(arreglo[i] == 1)
            {
                nActual++;
            }
        }

        while(nActual <= nArcos)
        {
            Debug.Log("Buscando arcos");
            if(Random.Range(0,2) == 1 && arreglo[indiceActual] != 1)
            {
                Debug.Log($"Creando arco en {indiceActual}");
                arreglo[indiceActual] = 1;
                nActual++;
            }
            indiceActual++;
            if(indiceActual > 3)
            {
                indiceActual = 0;
            }
        }
        return arreglo;
    }

    private Sala.TipoSala DeterminarTipoSala(int tipo)
    {
        Debug.Log($"Tipo de la sala: {tipo}");
        switch (tipo)
        {
            case 1: return Sala.TipoSala.INICIAL;
            case 2: return Sala.TipoSala.MIXTA;
            case 3: return Sala.TipoSala.ACERTIJO;
            case 4: return Sala.TipoSala.COMBATE;
            case 5: return Sala.TipoSala.JEFE;
            case 6: return Sala.TipoSala.GRADA;
            default: return Sala.TipoSala.MIXTA;
        }
    }


}
