 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    
    public GameObject Player2;
 

   
   void Update()
{
    if (Player2 != null) // Verifica que Player2 no sea null
    {
        transform.position = new Vector3(Player2.transform.position.x, Player2.transform.position.y, transform.position.z);
    }
}

}