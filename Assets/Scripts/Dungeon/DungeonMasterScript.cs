using UnityEngine;
using System.Collections.Generic;

public class Sala
{
    /*Enum para definir el tipo de sala que se creará.*/
    public enum TipoSala{INICIAL, MIXTA, ACERTIJO, COMBATE, JEFE, GRADA}
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
    int maxMapa = 11;
    int nivel = 1;
    [SerializeField] GameObject pared;
    [SerializeField] GameObject piso;
    [SerializeField] GameObject arco;
    int[,] mapa;

    void Start()
    {
        
        //Variables auxiliares para la generación de las salas del nivel.
        Vector2 posicionSalaActual = new Vector2(0,0);
        int[] arcosSalaAnterior = {0,0,0,0};
        mapa = new int[maxMapa,maxMapa];

        
        //Generación del mapa inicial.
        mapa = GenerarMapa(maxMapa, maxSalas);

        //Recorrido del mapa para crear las salas.
        for(int i=0; i < maxMapa; i++)
        {
            for(int j=0; j<maxMapa; j++)
            {
                //Creamos una sala.
                if(mapa[i,j] != 0)
                {
                    //Determinamos la posicion de la sala.
                    posicionSalaActual = DeterminarPosicion(i,j);
                    //Aañadimos la sala a la lista.
                    listaSalas.Add( new Sala(posicionSalaActual, DeterminarTipoSala(mapa[i,j]), DeterminarArcos(i,j,mapa), nivel) );
                    //Debug.Log($"Sala creada [{i},{j}]\nx:{posicionSalaActual.x}, y:{posicionSalaActual.y}\nTipo: {mapa[i,j]}");
                }
            }
        }
        
        for(int i=0; i<listaSalas.Count; i++)
        {
            //Instanciamos los elementos de la sala.
            Debug.Log($"Sala ({listaSalas[i].origen.x},{listaSalas[i].origen.y})\nTipo: {listaSalas[i].tipo}\nPuertas: [{listaSalas[i].puertas[0]}, {listaSalas[i].puertas[1]}, {listaSalas[i].puertas[2]}, {listaSalas[i].puertas[3]}]");
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
            if ((salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i) == salaNueva.tam.x/2 && salaNueva.puertas[0] == 1)
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
            if ((salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i) == salaNueva.tam.y/2 && salaNueva.puertas[1] == 1)
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
            if ((salaNueva.tamPared.x/2) + (salaNueva.tamPared.x*i) == salaNueva.tam.x/2 && salaNueva.puertas[2] == 1)
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
            if ((salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i) == salaNueva.tam.y/2 && salaNueva.puertas[3] == 1)
            {
                salaNueva.listaArcos.Add(Instantiate(arco, new Vector3(salaNueva.origen.x, 1.25f, salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i)), Quaternion.Euler(0,90,0)));
            }
            else
            {
                salaNueva.listaParedes.Add(Instantiate(pared, new Vector3(salaNueva.origen.x, 1.25f, salaNueva.origen.y + (salaNueva.tamPared.y/2) + (salaNueva.tamPared.y*i)), Quaternion.Euler(0,90,0)));
            }
        }
    }

    private int[,] GenerarMapa(int tamMapa, int salas)
    {
        int[,] mapaActual = new int[tamMapa,tamMapa];
        int x = 5;
        int xAnterior = 5;
        int y = 5;
        int yAnterior = 5;
        int direccion = 0;
        int i=2;

        mapaActual[x,y] = 1;

        while (i < salas)
        {
            direccion = Random.Range(0,3);
            //Debug.Log($"Dirección: {direccion}");
            switch (direccion)
            {
                case 0: y++; break;
                case 1: x++; break;
                case 2: y--; break;
                case 3: x--; break;
                default: x++; break;
            }

            //En caso de salir de la cuadrícula.
            if(x > maxMapa){ x = maxMapa; }
            if(x < 0){ x = 0; }
            if(y > maxMapa){ y = maxMapa; }
            if(y < 0){ x = 0; }

            if(mapaActual[x,y] != 1)
            {
                //Debug.Log($"Sala {i}\n[x: {x}, y: {y}]");
                if (i < salas-1){ mapaActual[x,y] = 1; }
                else
                {
                    //Debug.Log("Sala Jefe");
                    mapaActual[x,y] = 2;
                    //Definimos la ubicación de la sala de grada.
                    if((x - xAnterior) < 0){ x--; }
                    else if((x - xAnterior) > 0){ x++; }
                    
                    if((y - yAnterior) < 0){ y--; }
                    else if((y - yAnterior) > 0){ y++; }
                    //Debug.Log($"Sala grada ({x},{y})");
                    mapaActual[x,y] = 3;
                }
                xAnterior = x;
                yAnterior = y;
                i++;
            }
            else
            {
                x = xAnterior;
                y = yAnterior;
            }
        }
        return mapaActual;
    }
    
    private int[] DeterminarArcos(int x, int y, int[,] mapaActual)
    {
        int[] arreglo = {0,0,0,0};
        if(x == maxMapa)
        {
            if(mapaActual[x,y+1] != 0){ arreglo[0] = 1; }
            if(mapaActual[x,y-1] != 0){ arreglo[2] = 1; }
            if(mapaActual[x-1,y] != 0){ arreglo[3] = 1; }
            //Debug.Log($"Arcos creados (x:max): [{arreglo[0]},{arreglo[1]}, {arreglo[2]}, {arreglo[3]}]");
            return arreglo;
        }

        else if(x == 0)
        {
            if(mapaActual[x,y+1] != 0){ arreglo[0] = 1; }
            if(mapaActual[x+1,y] != 0){ arreglo[1] = 1; }
            if(mapaActual[x,y-1] != 0){ arreglo[2] = 1; }
            //Debug.Log($"Arcos creados (x:0): [{arreglo[0]},{arreglo[1]}, {arreglo[2]}, {arreglo[3]}]");
            return arreglo;
        }

        else if(y == maxMapa)
        {
            if(mapaActual[x+1,y] != 0){ arreglo[1] = 1; }
            if(mapaActual[x,y-1] != 0){ arreglo[2] = 1; }
            if(mapaActual[x-1,y] != 0){ arreglo[3] = 1; }
            //Debug.Log($"Arcos creados (y:max): [{arreglo[0]},{arreglo[1]}, {arreglo[2]}, {arreglo[3]}]");
            return arreglo;
        }
        else if(y == 0)
        {
            if(mapaActual[x,y+1] != 0){ arreglo[0] = 1; }
            if(mapaActual[x+1,y] != 0){ arreglo[1] = 1; }
            if(mapaActual[x-1,y] != 0){ arreglo[3] = 1; }
            //Debug.Log($"Arcos creados (y:0): [{arreglo[0]},{arreglo[1]}, {arreglo[2]}, {arreglo[3]}]");
            return arreglo;
        }
        else{
            if(mapaActual[x,y+1] != 0){ arreglo[0] = 1; }
            if(mapaActual[x+1,y] != 0){ arreglo[1] = 1; }
            if(mapaActual[x,y-1] != 0){ arreglo[2] = 1; }
            if(mapaActual[x-1,y] != 0){ arreglo[3] = 1; }
            //Debug.Log($"Arcos creados: [{arreglo[0]},{arreglo[1]}, {arreglo[2]}, {arreglo[3]}]");
            return arreglo;
        }
    }

    private Vector2 DeterminarPosicion(int x, int y)
    {
        Vector2 posicion;
        posicion.x = ( x - Mathf.Floor(maxMapa/2) ) * (float)12.5;
        posicion.y = ( Mathf.Floor(maxMapa/2) - y ) * (float)12.5;
        return posicion;
    }
    
    private Sala.TipoSala DeterminarTipoSala(int tipo)
    {
        if(tipo == 2){ return Sala.TipoSala.JEFE; }
        else if(tipo == 3){ return Sala.TipoSala.GRADA; }
        else
        {
            tipo = Random.Range(1,3);
            switch (tipo)
            {
                case 1: return Sala.TipoSala.MIXTA;
                case 2: return Sala.TipoSala.ACERTIJO;
                case 3: return Sala.TipoSala.COMBATE;
                default: return Sala.TipoSala.MIXTA;
            }
        }
    }
}
