/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 22-06-2026 00:06:22
 * @modify date 22-06-2026 00:06:22
 * @desc [description]
 */
using System.Collections;
using UnityEngine;
using TMPro;

public class CompanionDialogue : MonoBehaviour
{
    [Header("UI del Globo")]
    [SerializeField] public GameObject bubbleCanvas; 
    [SerializeField] public TextMeshProUGUI bubbleText; 
    
    [Header("Configuración")]
    public float typingSpeed = 0.03f;
    public float tiempoVisible = 5f; // Segundos que el texto se queda en pantalla

    private string[] currentSentences;
    private int index;
    private bool isTyping;
    private Coroutine autoCloseCoroutine; // Guardamos el temporizador acá

    void Start()
    {
        bubbleCanvas.SetActive(false);
    }

    void Update()
    {
        // Mantenemos la opción de saltar el diálogo con Espacio/E si el jugador está apurado
        if (bubbleCanvas.activeInHierarchy && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E)))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                bubbleText.text = currentSentences[index];
                isTyping = false;
                
                // Si el jugador saltó la animación, iniciamos el temporizador de cierre desde ahora
                if (autoCloseCoroutine != null) StopCoroutine(autoCloseCoroutine);
                autoCloseCoroutine = StartCoroutine(CerrarDialogoAutomatico());
            }
            else
            {
                NextSentence();
            }
        }
    }

    public void ShowDialogue(string[] sentences)
    {
        StopAllCoroutines(); // Detenemos cualquier escritura o temporizador viejo
        currentSentences = sentences;
        index = 0;
        bubbleCanvas.SetActive(true);
        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        bubbleText.text = "";
        
        foreach (char letter in currentSentences[index].ToCharArray())
        {
            bubbleText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        isTyping = false;
        
        // ¡Terminó de escribir! Arrancamos la cuenta regresiva de 5 segundos
        autoCloseCoroutine = StartCoroutine(CerrarDialogoAutomatico());
    }

    IEnumerator CerrarDialogoAutomatico()
    {
        // Espera los 5 segundos (o lo que se configure en el Inspector)
        yield return new WaitForSeconds(tiempoVisible);
        
        // Ejecuta la misma función de cierre que si hubieras llegado al final
        NextSentence();
    }

    public void NextSentence()
    {
        if (index < currentSentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence());
        }
        else
        {
            // Apaga el globo de diálogo
            bubbleCanvas.SetActive(false);
            bubbleText.text = "";
        }
    }
}