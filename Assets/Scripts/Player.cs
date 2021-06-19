using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    int playerSpeed;

    [SerializeField]
    int jumpHeight;

    public Coord playPosition;
    private new Rigidbody2D rigidbody;
    private bool playerIsTouchingGround;
    private int playerHasJumped = 0;
    public GameController gameController;
    public Animator animator;


    void Update()
    {

        if (Input.GetKey("a"))
            movePlayerXY(-1);
        if (Input.GetKey("d"))
            movePlayerXY(1);

        if (!Input.GetKey("a") && !Input.GetKey("d"))
            animator.SetFloat("Speed", 0);

        if (Input.GetKeyDown(KeyCode.Space))
            playerJump();

        playerReset();

        showPlayerPOS();
    }

    private void showPlayerPOS()
    {
        Coord pos = getPlayerPosition();
    }
    Coord getPlayerPosition()
    {
        Vector2 currentplayerPosition = gameObject.transform.position;
        float x = currentplayerPosition.x;
        float y = currentplayerPosition.y;
        this.playPosition = new Coord(x, y);
        return new Coord(x, y);
    }

    public void movePlayerXY(int direction)
    {
        animator.SetFloat("Speed", 1);
        transform.position += new Vector3(direction * Time.deltaTime * playerSpeed, 0, 0);
        rotatePlayer(direction);
    }

    private void rotatePlayer(int direction)

    {
        Debug.Log(direction);
        if (direction == 1)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        if (direction == -1)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void playerJump()
    {
        if (playerHasJumped <= 1)
        {
            rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            if (playerHasJumped == 1)
                playerSpin();

            playerHasJumped += 1;
        }
    }

    private void playerSpin()
    {

        transform.Rotate(0, 0, 5 * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerIsTouchingGround = true;
        playerHasJumped = 0;
        animator.SetBool("IsJumping", false);
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
