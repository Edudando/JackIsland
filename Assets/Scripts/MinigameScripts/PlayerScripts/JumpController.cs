/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Script que pretende manejar sólo el salto del personaje sin modificar el script de movimiento principal]
 */

using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    
    [Header("Salto")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector2 valoresCaja;
    [SerializeField] private Vector2 dimensionesCaja;
    [SerializeField] private LayerMask capasSalto;

    private bool enSuelo;
    private bool entradaSalto;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            entradaSalto = true;
        }

        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, valoresCaja, 0f, capasSalto);
    }

    private void FixedUpdate()
    {
        ControlarSalto();
    }

    private void ControlarSalto()
    {
        if (!entradaSalto)
        {
            return;
        }

        if (!enSuelo)
        {
            return;
        }

        Saltar();
    }

    private void Saltar()
    {
        entradaSalto = false;
        rb2D.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }
}
