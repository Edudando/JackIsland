/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 27-06-2026 23:58:26
 * @modify date 27-06-2026 23:58:26
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
    
    [Header("Animación")]
    public Animator jackAnimator;
    public PlayerMovement playerMovement;

    [Header("Configuración")]
    public float typingSpeed = 0.03f;
    public float tiempoVisible = 5f;

    private string[] currentSentences;
    private int index;
    private bool isTyping;
    private Coroutine autoCloseCoroutine;

    void Start()
    {
        bubbleCanvas.SetActive(false);
    }

    void Update()
    {
        if (bubbleCanvas.activeInHierarchy && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E)))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                bubbleText.text = currentSentences[index];
                isTyping = false;
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
        StopAllCoroutines();
        currentSentences = sentences;
        index = 0;
        bubbleCanvas.SetActive(true);

        if (jackAnimator != null)
            jackAnimator.SetBool("Talk", true);

        if (playerMovement != null)
            playerMovement.estaHablando = true;

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
        autoCloseCoroutine = StartCoroutine(CerrarDialogoAutomatico());
    }

    IEnumerator CerrarDialogoAutomatico()
    {
        yield return new WaitForSeconds(tiempoVisible);
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
            bubbleCanvas.SetActive(false);
            bubbleText.text = "";

            if (jackAnimator != null)
                jackAnimator.SetBool("Talk", false);

            if (playerMovement != null)
                playerMovement.estaHablando = false;
        }
    }
}