using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogoController : MonoBehaviour
{
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;

    [TextArea(2, 4)]
    public List<string> lineasDialogo; 

    public float tiempoEntreLineas = 2f; 

    public void MostrarDialogo(System.Action alTerminar)
    {
        panelDialogo.SetActive(true);
        StartCoroutine(MostrarLineas(alTerminar));
    }

    private IEnumerator MostrarLineas(System.Action callback)
    {
        foreach (string linea in lineasDialogo)
        {
            textoDialogo.text = linea;
            yield return new WaitForSeconds(tiempoEntreLineas);
        }

        textoDialogo.text = "";
        panelDialogo.SetActive(false);

        callback?.Invoke();
    }
}

