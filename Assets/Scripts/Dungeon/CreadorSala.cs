using UnityEngine;
/*Summary

--Cosas importantes--
Las paredes son creadeas de 0 a x (Horizontal) y luego de 0 a Z (Vertical).
*/
public class CreadorSala : MonoBehaviour
{
    [SerializeField] GameObject pared;
    [SerializeField] GameObject piso;
    [SerializeField] GameObject arco;
    private Vector3 tamPared = new Vector3(2.5f,2.5f,0.5f);
    private Vector3 tamArco = new Vector3(2.5f,2.5f,0.5f);
    private Vector3 tamPiso = new Vector3(2.5f,0.5f,2.5f);
    [SerializeField] private Vector2 tamSala = new Vector3(12.5f,12.5f);
    private Vector2 origenSala = new Vector2(0,0);
    
    void Start()
    {

        //Instancias del piso
        for (int i=0; i<(tamSala.x/tamPiso.x); i++)
        {
            for(int j=0; j<(tamSala.y/tamPiso.z); j++)
            {
                Debug.Log($"Piso: {i},{j}\nPos: ({origenSala.x + (tamPiso.x/2) + (tamPiso.x*j)},{0},{origenSala.y + (tamPiso.z*i)})");
                Instantiate(piso, new Vector3(origenSala.x + (tamPiso.x/2) + (tamPiso.x*j), 0f, origenSala.y + (tamPiso.z*i)), Quaternion.Euler(0,0,0));
            }
        }

        //Horizontal.
        for (int i=0; i<(tamSala.x/tamPared.x); i ++)
        {
            if (origenSala.x + (tamPared.x/2) + (tamPared.x*i) == tamSala.x/2)
            {
                Debug.Log($"Arco: {i}\nPos: ({origenSala.x + (tamPared.x/2) + (tamPared.x*i)},{1.25},{origenSala.y})");
                Instantiate(arco, new Vector3(origenSala.x + (tamPared.x/2) + (tamPared.x*i), 1.25f, origenSala.y), Quaternion.Euler(0,0,0));
            }
            else
            {
                Debug.Log($"Paredes: {i}\nPos: ({origenSala.x + (tamPared.x/2) + (tamPared.x*i)},{1.25},{origenSala.y})");
                Instantiate(pared, new Vector3(origenSala.x + (tamPared.x/2) + (tamPared.x*i), 1.25f, origenSala.y), Quaternion.Euler(0,0,0));
                Instantiate(pared, new Vector3(origenSala.x + (tamPared.x/2) + (tamPared.x*i), 1.25f, origenSala.y+tamSala.y-(tamPared.z/2)), Quaternion.Euler(0,0,0));
            }
        }

        //Vertical
        for (int i=0; i<(tamSala.y/tamPared.y); i ++)
        {
            if (origenSala.y + (tamPared.y/2) + (tamPared.y*i) == tamSala.y/2)
            {
                Instantiate(arco, new Vector3(origenSala.x , 1.25f, origenSala.y + (tamPared.y/2) + (tamPared.y*i)), Quaternion.Euler(0,90,0));
            }
            else
            {
                Instantiate(pared, new Vector3(origenSala.x , 1.25f, origenSala.y + (tamPared.y/2) + (tamPared.y*i)), Quaternion.Euler(0,90,0));
                Instantiate(pared, new Vector3(origenSala.x + tamSala.x-(tamPared.z/2), 1.25f, origenSala.y + (tamPared.y/2) + (tamPared.y*i)), Quaternion.Euler(0,90,0));
            }
        }
    }
}
