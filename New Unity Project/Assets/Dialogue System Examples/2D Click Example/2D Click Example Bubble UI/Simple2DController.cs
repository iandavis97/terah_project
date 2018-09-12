using UnityEngine;

public class Simple2DController : MonoBehaviour
{

    public float speed = 10;

    private Rigidbody2D rb { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * speed, 0));
    }
}
