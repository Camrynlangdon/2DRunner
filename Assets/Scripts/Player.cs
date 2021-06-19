using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    int playerSpeed;

    [SerializeField]
    int jumpHeight;

    private new Rigidbody2D rigidbody;
    private bool playerIsTouchingGround;
    private int playerHasJumped = 0;
    public GameController gameController;

    void Update()
    {
        if (Input.GetKey("d"))
            movePlayerXY(1);
        if (Input.GetKey("a"))
            movePlayerXY(-1);
        if (Input.GetKeyDown(KeyCode.Space))
            playerJump();

        playerReset();
    }

    public void movePlayerXY(int direction)
    {
        transform.position += new Vector3(direction * Time.deltaTime * playerSpeed, 0, 0);
    }

    public void playerJump()
    {
        if (playerHasJumped <= 1)
        {
            rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            playerHasJumped += 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerIsTouchingGround = true;
        playerHasJumped = 0;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        playerIsTouchingGround = false;
    }

    private void playerReset()
    {

        if (gameObject.transform.position.y < 0)
            gameController.resetCurrentLevel();
    }
}
