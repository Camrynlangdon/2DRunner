using System.Collections;
using Unity.Mathematics;
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

    [SerializeField]
    float healthRegenSpeed;

    [SerializeField]
    float playerHealthRegenDelay;

    [SerializeField]
    float healthRegenCap;

    [SerializeField]
    public float maxPlayerArmor;

    [SerializeField]
    public float staggerLength;

    public float playerHealth;
    public float playerArmor;
    public GameController gameController;
    public Animator animator;
    public Coord playerPosition;
    public CameraController cameraController;
    public bool hasLanded = false;
    public GameObject healthBar;
    public GameObject armorBar;
    public Rigidbody2D rigidbody2D;
    public bool playerControlLock = false;

    private new Rigidbody2D rigidbody;
    private bool playerIsTouchingGround;
    private int playerHasJumped = 0;
    private bool playerHealthRegenInProgress = false;
    private float lastTimePlayerWasHurt;
    private SpriteRenderer healthBarSprite;

    private void Start()
    {
        playerHealth = maxPlayerHealth;
        healthBarSprite = healthBar.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!playerControlLock)
        {
            if (Input.GetKey("a"))
                movePlayerX(-1);
            if (Input.GetKey("d"))
                movePlayerX(1);
            if (Input.GetKeyDown(KeyCode.Space))
                playerJump();
        }
        if (!Input.GetKey("a") && !Input.GetKey("d") || playerControlLock)
            animator.SetFloat("Speed", 0);

        if (Input.GetKeyDown(KeyCode.Space))
            playerJump();

        if (Input.GetKey("r"))
            gameController.resetCurrentLevel();

        cameraController.changeCameraPosition(getPlayerPosition(), hasLanded);
    }
    private void FixedUpdate()
    {
        StartCoroutine(regenHealth());
    }

    public Coord getPlayerPosition()
    {
        Vector2 currentplayerPosition = gameObject.transform.position;
        float x = currentplayerPosition.x;
        float y = currentplayerPosition.y;
        this.playerPosition = new Coord(x, y);
        return new Coord(x, y);
    }

    private void movePlayerX(int direction)
    {
        if (!hasLanded) return;
        animator.SetFloat("Speed", 1);
        rigidbody2D.velocity = new Vector2(direction * playerSpeed, rigidbody2D.velocity.y);
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
        if (playerHasJumped <= 2)
        {
            rigidbody2D.velocity = Vector2.up * jumpStrength * 2;
            animator.SetBool("IsJumping", true);
            playerHasJumped += 1;
        }
    }

    public void staggerPlayer(float seconds)
    {
        StartCoroutine(lockPlayerContols(seconds));
    }
    public IEnumerator lockPlayerContols(float seconds)
    {
        playerControlLock = true;
        yield return new WaitForSeconds(seconds);
        playerControlLock = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerIsTouchingGround = true;
        playerHasJumped = 0;
        animator.SetBool("IsJumping", false);
        hasLanded = true;

        if (collision.gameObject.tag == "Enemy")
        {
            staggerPlayer(staggerLength); ;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerIsTouchingGround = false;
    }

    IEnumerator regenHealth()
    {
        if (playerHealth < healthRegenCap && !playerHealthRegenInProgress)
        {
            if (Time.time - lastTimePlayerWasHurt > playerHealthRegenDelay)
            {
                playerHealthRegenInProgress = true;
                yield return new WaitForSeconds(healthRegenSpeed);
                ChangeHealth(1);
                playerHealthRegenInProgress = false;
            }
        }
    }

    public void ChangeHealth(float change)
    {
        if (playerArmor > 0 && change < 0)
        {
            changeArmor(change);
            return;
        }
        float newPlayerHealth = playerHealth - maxPlayerHealth * change / 100 * -1;

        if (newPlayerHealth > 100) newPlayerHealth = 100;
        if (newPlayerHealth < 0)
        {
            newPlayerHealth = 0;
            gameController.resetCurrentLevel();
        }
        playerHealth = newPlayerHealth;
        ChangeHealthBar();
    }

    public void changeArmor(float change)
    {
        float newPlayerArmor = playerArmor - maxPlayerArmor * change / 100 * -1;

        if (newPlayerArmor > 100) newPlayerArmor = 100;
        if (newPlayerArmor < 0)
        {
            ChangeHealth(playerArmor - change);
            newPlayerArmor = 0;
        }
        playerArmor = newPlayerArmor;
        changeArmorBar();
    }

    private void changeArmorBar()
    {
        float currentXScale = armorBar.transform.localScale.x;
        float newBarXScale = playerArmor / maxPlayerArmor;
        armorBar.transform.localScale = new Vector2(newBarXScale, 0.5f);
    }

    private void ChangeHealthBar()
    {
        float currentXScale = healthBar.transform.localScale.x;
        float newBarXScale = playerHealth / maxPlayerHealth;

        healthBar.transform.localScale = new Vector2(newBarXScale, 1f);
        handleHealthBarColor();

        if (newBarXScale < currentXScale)
        {
            animator.SetTrigger("PlayerHurt");
            lastTimePlayerWasHurt = Time.time;
        }
    }

    private void handleHealthBarColor()
    {
        if (playerHealth <= 35)
        {
            healthBarSprite.color = new Color(255, 0, 0, 1);
            return;
        }
        if (playerHealth <= 79)
        {
            healthBarSprite.color = new Color(255, 255, 0, 1);
            return;
        }
        if (playerHealth >= 80)
        {
            healthBarSprite.color = new Color(0, 255, 0, 1);
            return;
        }
    }



}
