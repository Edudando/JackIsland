/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 22-06-2026 00:14:36
 * @modify date 22-06-2026 00:14:36
 * @desc [description]
 */
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity; // 1. Agrega la librería de FMOD aquí

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    public CompanionDialogue dialogoDelLoro;

    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        dialogoDelLoro = GetComponent<CompanionDialogue>();         
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * MoveSpeed;
    }

    void Update()
    {
        if (animator != null)
        {
            animator.SetFloat("Velocity", moveInput.magnitude);

            if (moveInput.x > 0)
            {
                spriteRenderer.flipX = false; 
            }
            else if (moveInput.x < 0)
            {
                spriteRenderer.flipX = true; 
            }

            if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
            {
                string[] frasesDePrueba = { 
                    "¡Squawk! ¡Cuidado por donde pisas, Jack!", 
                    "Esa calavera de piedra no me da buena espina." 
                };
                
                dialogoDelLoro.ShowDialogue(frasesDePrueba);
            }
        }
    }

    // 2. Agrega esta función al final de tu clase
    public void ReproducirPaso()
    {
        // RuntimeManager.PlayOneShot dispara el sonido una sola vez en la posición del jugador
        RuntimeManager.PlayOneShot("event:/PasosJack", transform.position);
    }
}