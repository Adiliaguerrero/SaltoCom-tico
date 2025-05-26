using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UnirFrasesInteractivo : MonoBehaviour
{

    public Paneles1 Paneles1;  

    public TMP_Text textoIzquierdo;
    public TMP_Text[] textosDerechos;
    public TMP_Text textoResultado;
    public Image imagenLinea;
    public Transform puntoInicioManual;
    public Transform[] puntosDerechos;

    private TMP_Text origenSeleccionado = null;
    private TMP_Text destinoSeleccionado = null;
    private bool arrastrando = false;
    private bool yaUnido = false;
    private Vector3 puntoInicio;

    private string fraseSinComas = "Mi papá compró plátano manzana uvas peras y pan";
    private string[] opcionesConComas = new string[] {
        "Mi papá compró plátano, manzana, uvas, peras y pan",
        "Mi papá compró plátano manzana uvas, peras y pan",
        "Mi papá compró plátano, manzana uvas peras, y pan"
    };
    private int indiceCorrecto = 0;

    void Start()
    {
        textoIzquierdo.text = fraseSinComas;
        for (int i = 0; i < textosDerechos.Length; i++)
        {
            textosDerechos[i].text = opcionesConComas[i];
        }

        imagenLinea.enabled = false;
        textoResultado.text = "";
    }

    void Update()
    {
        if (arrastrando)
        {
            DibujarLinea(puntoInicio, Input.mousePosition);

            if (Input.GetMouseButtonUp(0))
            {
                DetectarDestino();
            }
        }
    }

   public void IniciarArrastreDesdeTexto(GameObject textoGO)
{
    if (yaUnido || arrastrando) return;

    origenSeleccionado = textoGO.GetComponent<TMP_Text>();

    if (origenSeleccionado == textoIzquierdo && puntoInicioManual != null)
    {
        puntoInicio = puntoInicioManual.position;
    }
    else
    {
 
        for (int i = 0; i < textosDerechos.Length; i++)
        {
            if (origenSeleccionado == textosDerechos[i] && i < puntosDerechos.Length && puntosDerechos[i] != null)
            {
                puntoInicio = puntosDerechos[i].position;
                break;
            }
            else
            {
      
                puntoInicio = ObtenerCentro(origenSeleccionado.rectTransform);
            }
        }
    }

    arrastrando = true;
    imagenLinea.enabled = true;
}


    void DetectarDestino()
    {
        arrastrando = false;

        TMP_Text posibleDestino = DetectarTextoDebajoDelMouse();
        if (posibleDestino != null && posibleDestino != origenSeleccionado)
        {
            destinoSeleccionado = posibleDestino;
            Vector3 puntoFin = ObtenerPuntoDeDestino(destinoSeleccionado);
            DibujarLinea(puntoInicio, puntoFin);
            VerificarRespuesta(destinoSeleccionado);

            if (destinoSeleccionado == textoIzquierdo && puntoInicioManual != null)
            {
                Vector3 nuevoDestinoVisual = puntoInicioManual.position;
                DibujarLinea(puntoInicio, nuevoDestinoVisual);
            }

            yaUnido = true;
        }
        else
        {
            imagenLinea.enabled = true;
        }
    }

    TMP_Text DetectarTextoDebajoDelMouse()
    {
        PointerEventData data = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var resultados = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(data, resultados);

        foreach (var r in resultados)
        {
            TMP_Text t = r.gameObject.GetComponent<TMP_Text>();
            if (t != null) return t;
        }

        return null;
    }

    void VerificarRespuesta(TMP_Text destino)
    {
        for (int i = 0; i < textosDerechos.Length; i++)
        {
            if (destino == textosDerechos[i])
            {
                if (i == indiceCorrecto)
                {
                    textoResultado.text = "¡Correcto!";
                    IncrementarPuntajeBasicoCorrecto();
                    Invoke("PasarASiguienteTrivia", 2f);
                }
                else
                {
                    textoResultado.text = "Incorrecto.";
                    IncrementarPuntajeBasicoIncorrecto();
                    Invoke("PasarASiguienteTrivia", 2f);
                }
                return;
            }
        }

        if (origenSeleccionado == textosDerechos[indiceCorrecto] && destino == textoIzquierdo)
        {
            textoResultado.text = "¡Correcto!";
            IncrementarPuntajeBasicoCorrecto();
            Invoke("PasarASiguienteTrivia", 2f);
        }
        else
        {
            textoResultado.text = "Incorrecto.";
            IncrementarPuntajeBasicoIncorrecto();
            Invoke("PasarASiguienteTrivia", 2f);
        }
    }

 void PasarASiguienteTrivia()
{
    Paneles1.SiguienteTrivia();
}


    void DibujarLinea(Vector3 inicio, Vector3 fin)
    {
        Vector3 localInicio = imagenLinea.rectTransform.parent.InverseTransformPoint(inicio);
        Vector3 localFin = imagenLinea.rectTransform.parent.InverseTransformPoint(fin);
        Vector3 medio = (localInicio + localFin) / 2f;

        imagenLinea.rectTransform.localPosition = medio;

        float distancia = Vector3.Distance(localInicio, localFin);
        imagenLinea.rectTransform.sizeDelta = new Vector2(distancia, 5f);

        float angulo = Mathf.Atan2(localFin.y - localInicio.y, localFin.x - localInicio.x) * Mathf.Rad2Deg;
        imagenLinea.rectTransform.localRotation = Quaternion.Euler(0, 0, angulo);
    }

    Vector3 ObtenerCentro(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        return (corners[0] + corners[1]) / 2f;
    }

    Vector3 ObtenerPuntoDeDestino(TMP_Text destino)
    {
        for (int i = 0; i < textosDerechos.Length; i++)
        {
            if (destino == textosDerechos[i] && i < puntosDerechos.Length && puntosDerechos[i] != null)
            {
                return puntosDerechos[i].position;
            }
        }

        return ObtenerCentro(destino.rectTransform);
    }

    
    void IncrementarPuntajeBasicoCorrecto()
    {
        int puntaje = PlayerPrefs.GetInt("PuntajeCorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeCorrectoBasico", ++puntaje);
    }

    void IncrementarPuntajeBasicoIncorrecto()
    {
        int puntaje = PlayerPrefs.GetInt("PuntajeIncorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoBasico", ++puntaje);
    }
}


