using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image fillAreaImage;
    [SerializeField] private Image flashAreaImage;

    [Header("ColorPallete")]
    [SerializeField] private Color leftPrimaryColor;
    [SerializeField] private Color leftSecondaryColor;
    [SerializeField] private Color leftFlashColor;
    [SerializeField] private Color rightPrimaryColor;
    [SerializeField] private Color rightSecondaryColor;
    [SerializeField] private Color rightFlashColor;

    [Header("Bar Setup")]
    [SerializeField] private GameStateEnum startupPosition;

    [SerializeField] private float maxValue = 100;
    [SerializeField] private float currentValue = 50;
    
    [Header("Flash Setup")]
    [SerializeField] private float flashTimerDuration = 1;
    [SerializeField]  private float shrinkSpeed = 1f;
    private float flashTimer;

    private static HealthBar instance;
    private GameStateEnum gameState;

    public bool IsDead = false;
    public Animator anim;

    public static void InstantiateHealthBar(float maxHealthValue, GameStateEnum startingState, float currentHealth)
    {
        instance.ResetBarValues(maxHealthValue, startingState, currentHealth);
    }

    public static void RegisterDamage(float damageAmount)
    {
        instance.CalculateDamage(damageAmount);
    }

    public static void ChangeState(GameStateEnum newState)
    {
        instance.TryChangingBarState(newState);
    }

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        ChangeBarState(startupPosition);
    }

    public void FixedUpdate()
    {
        if (IsDead) anim.SetBool("IsDead", true);
        if (IsDead == false) anim.SetBool("IsDead", false);
    }

    private void Update()
    {

        flashTimer -= Time.deltaTime;

        if (flashTimer < 0)
        {
            if (fillAreaImage.fillAmount < flashAreaImage.fillAmount)
            {
                flashAreaImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }
    }

    public void CalculateDamage(float damageAmount)
    {
        currentValue -= damageAmount;

        if (currentValue < 0)
        {
            IsDead = true;
            currentValue = 0;
			StartCoroutine(endGame());
        }

        fillAreaImage.fillAmount = currentValue / maxValue;
        flashTimer = flashTimerDuration;
    }

	IEnumerator endGame()
	{
		try
		{
            this.GetComponent<AudioSource>().Play();
		}
		catch (System.Exception)
		{
			Debug.Log("no end sound");
		}

		PlayerController pl = GameManager.Instance.Player.GetComponent<PlayerController>();
		pl.movement = Vector2.zero;
		pl.isAlive = false;
		pl.enabled = false;
		pl.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GameManager.Instance.Player.GetComponent<BoxCollider2D>().enabled = false;
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("EndGame", LoadSceneMode.Single);
	}

	private void TryChangingBarState(GameStateEnum newState)
    {
        if (gameState == newState) return;

        ChangeBarState(newState);
    }

    private void ChangeBarState(GameStateEnum newState)
    {
        gameState = newState;
        currentValue = maxValue - currentValue;

        float fillAmount = currentValue / maxValue;
        fillAreaImage.fillAmount = fillAmount;
        flashAreaImage.fillAmount = fillAmount;

        if (gameState == GameStateEnum.Mater)
        {
            fillAreaImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            flashAreaImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            fillAreaImage.color = leftPrimaryColor;
            backgroundImage.color = rightSecondaryColor;
            flashAreaImage.color = leftFlashColor;
        }
        else
        {
            fillAreaImage.fillOrigin = (int)Image.OriginHorizontal.Right;
            flashAreaImage.fillOrigin = (int)Image.OriginHorizontal.Right;
            fillAreaImage.color = rightPrimaryColor;
            backgroundImage.color = leftSecondaryColor;
            flashAreaImage.color = rightFlashColor;
        }
    }

    private void ResetBarValues(float maxHealthValue, GameStateEnum startingState, float currentHealth)
    {
        maxValue = maxHealthValue;
        TryChangingBarState(startingState);
        currentValue = currentHealth;
    }
}