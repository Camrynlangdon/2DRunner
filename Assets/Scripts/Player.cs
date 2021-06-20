using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    int playerSpeed;

    [SerializeField]
    int jumpStrength;

    [SerializeField]
    float maxPlayerHealth;

    public float playerHealth;
    public GameController gameController;
    public Animator animator;
    public Coord playPosition;
    public CameraController cameraController;
    public bool hasLanded = false;
    

    private new Rigidbody2D rigidbody;
    private bool playerIsTouchingGround;
    private int playerHasJumped = 0;

    private void Start()
    {
        playerHealth = maxPlayerHealth; 
    }

    private void Update()
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

        cameraController.changeCameraPosition(getPlayerPosition(), hasLanded);
    }


    private void FixedUpdate()
    {
        if(playerHealth <= 0) gameController.resetCurrentLevel();
    }

    public Coord getPlayerPosition()
    {
        Vector2 currentplayerPosition = gameObject.transform.position;
        float x = currentplayerPosition.x;
        float y = currentplayerPosition.y;
        this.playPosition = new Coord(x, y);
        return new Coord(x, y);
    }

    private void movePlayerXY(int direction)
    {
        if (!hasLanded) return;
        animator.SetFloat("Speed", 1);
        transform.position += new Vector3(direction * Time.deltaTime * playerSpeed, 0, 0);
        rotatePlayer(direction);
    }

    private void rotatePlayer(int direction)

    {
       
        if (direction == 1)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        if (direction == -1)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void playerJump()
    {
        if (playerHasJumped <= 1)
        {
            rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            playerHasJumped += 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerIsTouchingGround = true;
        playerHasJumped = 0;
        animator.SetBool("IsJumping", false);
        hasLanded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerIsTouchingGround = false;
    }

    private void playerReset()
    {

        if (gameObject.transform.position.y < 0)
            gameController.resetCurrentLevel();
    }

    public void ChangeHealth(float change)
    {
        float healthMultiplyer = change / 100;
        float healthModifier = maxPlayerHealth * healthMultiplyer;
        playerHealth = playerHealth - healthModifier;

    }

}
