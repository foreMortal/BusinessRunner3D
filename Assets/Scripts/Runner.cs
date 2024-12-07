using System;
using YG;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using UnityEngine.Rendering.Universal;

public class Runner : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private ManagerObject manager;
    [SerializeField] private AudioSource source;
    [SerializeField] private Vector3 direction = Vector3.forward;
    [SerializeField] private float speed, audioTime;

    private CapitalBar hud;
    private GameEndedScreen gameEnded;
    private Animator animator;
    private CapitalManager capitalManager;
    private Vector3 distVector;
    private Quaternion distRotation;
    private float t = 2, idleTimer, audioTimer;
    private float stamina, maxStamina, distance;
    private bool finished, audioActive;

    public bool Active { get;  set; }

    public UnityEvent gameEnd = new();
    public float Distance { get { return distance; } }
    public event Action<Vector3, Vector3> changeDirection;

    public CapitalBar CapitalBar { set { hud = value; } }
    public GameEndedScreen GameEndedScreen { set { gameEnded = value; } }

    private void Awake()
    {
        capitalManager = GetComponent<CapitalManager>();
        animator = GetComponentInChildren<Animator>();
        gameEnd.AddListener(EndGame);
        idleTimer = Random.Range(15f, 25f);
    }

    public void SelfAwake()
    {
        animator.CrossFade("Run", 0.1f);
        
        Active = true;
        maxStamina = stamina = manager.GetUpgrade(1).GetValue();
    }

    private void Update()
    {
        if (Active)
        {
            if (t < 1)
            {
                t += Time.deltaTime;
                direction = Vector3.Lerp(direction, distVector, t);
                transform.rotation = Quaternion.Slerp(transform.rotation, distRotation, t);
            }

            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

            audioTimer -= Time.deltaTime;
            if (audioTimer <= 0f)
            {
                source.Play();
                audioActive = true;
                audioTimer = audioTime;
            }

            float delta = speed * Time.deltaTime;

            distance += delta;
            stamina -= delta;
            hud.RefreshStamina(stamina, maxStamina);

            if (stamina <= 0f)
            {
                GetLangAccurateStaminaEnded(out string endType, out string buttonText);
                GameEndInfo info = new GameEndInfo()
                {
                    endType = endType,
                    buttonText = buttonText,
                    distance = distance,
                    moneyEarn = capitalManager.Capital,
                };

                gameEnded.OpenEndGameMenu(ref info);
                gameEnd.Invoke();

                
                int rand = Random.Range(0, 2);
                switch (rand)
                {
                    case 0:
                        animator.CrossFade("tired", 0.1f); break;
                    case 1:
                        animator.CrossFade("sad", 0.1f); break;
                    case 2:
                        animator.CrossFade("defeted", 0.1f); break;
                }
            }
        }
        else if (finished)
        {
            if(t < 1f)
            {
                t += Time.deltaTime * 0.5f;
                transform.position = Vector3.Lerp(direction, distVector, t);
            }
            else
            {
                GetLangAccurateEndAproached(out string endType, out string buttonText);
                GameEndInfo info = new GameEndInfo()
                {
                    endType = endType,
                    buttonText = buttonText,
                    distance = distance,
                    moneyEarn = capitalManager.Capital,
                };

                gameEnded.OpenEndGameMenu(ref info);
                finished = false;
                animator.CrossFade("happy", 1f);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        if(!Active && audioActive)
        {
            source.Stop();
            audioActive = false;
        }

        if(!Active && !finished)
        {
            idleTimer -= Time.deltaTime;
            if(idleTimer <= 0f)
            {
                animator.CrossFade("idleVer2", 1f);
                idleTimer = Random.Range(25f, 50f);
            }
        }
    }

    public void Finish(Vector3 finishPosition)
    {
        direction = transform.position;
        distVector = finishPosition;

        finished = true;
        gameEnd.Invoke();
        t = 0f;
    }

    public void EndGame()
    {
        settings.GameState = false;
        YandexGame.GameplayStop();
        Active = false;
    }

    public void ChangeDirection(Vector3 newDir, Vector3 newRight)
    {
        distVector = newDir;
        distRotation = Quaternion.LookRotation(newDir, Vector3.up);
        t = 0;

        changeDirection?.Invoke(newDir, newRight);
    }

    private void GetLangAccurateStaminaEnded(out string endType, out string buttonText)
    {
        switch (YandexGame.lang)
        {
            case "ru":
                endType = "Выносливость истощена!"; buttonText = "Попрбовать снова";
                break;
            case "en":
                endType = "Stamina depleted!"; buttonText = "Try again";
                break;
            case "tr":
                endType = "Dayanıklılık tükendi!"; buttonText = "Tekrar deneyin";
                break;
            case "es":
                endType = "Aguante agotada!"; buttonText = "Intentar otra vez";
                break;
            case "de":
                endType = "Ausdauer erschöpft!"; buttonText = "Versuchen Sie es erneut";
                break;
            default: endType = "Выносливость истощена!"; buttonText = "Попрбовать снова"; break;
        }
    }

    private void GetLangAccurateEndAproached(out string endType, out string buttonText)
    {
        switch (YandexGame.lang)
        {
            case "ru":
                endType = "Уровень пройден!"; buttonText = "Следующий";
                break;
            case "en":
                endType = "Level passed!"; buttonText = "Next";
                break;
            case "tr":
                endType = "Seviye geçildi!"; buttonText = "Sonraki";
                break;
            case "es":
                endType = "¡Nivel pasado!"; buttonText = "Próxima";
                break;
            case "de":
                endType = "Level bestanden!"; buttonText = "Nächste";
                break;
            default: endType = "Выносливость истощена!"; buttonText = "Попрбовать снова"; break;
        }
    }
}

public struct GameEndInfo
{
    public string endType, buttonText;
    public float distance, moneyEarn;
}