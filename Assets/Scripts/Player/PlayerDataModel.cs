using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �÷��̾� ĳ������ ������ ����ҿ� �÷��̾� ĳ���Ϳ� ������ ��ũ��Ʈ���� �߰� ��ü ������ �ϴ� ��ũ��Ʈ
/// </summary>
public class PlayerDataModel : MonoBehaviour
{
    public PlayerActionController playerAction;         // �÷��̾��� �ൿ�� ���õ� �۾��� �ϴ� ��ũ��Ʈ
    public PlayerMovementController playerMovement;     // �÷��̾��� �̵��� ���õ� �۾��� �ϴ� ��ũ��Ʈ
    public PlayerCameraController playerCamera;         // �÷��̾��� ī�޶�� ���õ� �۾��� �ϴ� ��ũ��Ʈ
    public PlayerSystemController playerSystem;         // �÷��̾� ĳ���Ϳ� ���õ� �۾��� �ϴ� ��ũ��Ʈ
    public Hero hero;                                   // ���� ������� ĳ���� ����
    public List<Hero> heroList;                         // ����� �� �ִ� ���� ����Ʈ
    [SerializeField][Range(0, 2)] public int heroNum;   // ���õ� ���� ��ȣ
    public Animator animator;                           // ĳ���� �ִϸ�����(������ ���Ե� �ִϸ����͸� ����)
    public Rigidbody rb;                                // ĳ���� ��ü
    public Inventory inventory;                         // ĳ���� �κ��丮
    public Inventory Inventory { get { return inventory; } set { inventory = value; } }

    [SerializeField] float maxHP, nowHP, exp;   // �ִ� ü��, ���� ü��, ���� ����ġ
    [SerializeField] int level, coin;           // ���� ����, ���� ��ȭ
    [SerializeField] float[] status;            // �ɷ�ġ �迭(�̵��ӵ�, ��������, ���ݷ�, ġ��Ÿ Ȯ��, ġ��Ÿ ����)
    public float[] buffModifier;                // �� �ɷ�ġ�� ���� ���� �迭

    public int jumpLimit, jumpCount;                                                            // �Ѱ� ���� ��, ���� ���� ��
    public bool attackCooldown, controllable, dodgeDamage, onGizmo, onESC, alive, onSession;    // ���� ��Ÿ�� ����, ���� ���� ����, ȸ�� ����, ����� ��� ����, ESCâ ����

    public float coolTime;                  // ��Ÿ��
    public bool[] coolChecks = new bool[4]; // �� �׼ǿ� ���� ��Ÿ�� ����

    public float mouseSensivity;            // ���콺 �ΰ���
    [SerializeField] bool playerTimeScaleMultiplier;  // �÷��̾� �ð� ��� ����

    public UnityEvent OnLevelEvent, OnHPEvent, OnEXPEvent;  // ���� ���� ���� �̺�Ʈ, ü�� ��ȭ�� ���� �̺�Ʈ, ����ġ ��ȭ�� ���� �̺�Ʈ
    public UnityEvent<int> OnCoinEvent;                     // ��ȭ ȹ�濡 ���� �̺�Ʈ
    public List<IDamageSubscriber> damageSubscribers;       // ������ �߻��� ������ �������̽� ����Ʈ

    void Awake()
    {
        Initailze();
    }

    /// <summary>
    /// �ʱ�ȭ �޼ҵ�
    /// </summary>
    void Initailze()
    {
        GameManager.Data.Player = this;

        heroList = new List<Hero>();
        heroList.AddRange(GetComponentsInChildren<Hero>());
        for(int i = 0; i < heroList.Count; i++)
            heroList[i].gameObject.SetActive(false);

        rb = GetComponent<Rigidbody>();
        playerAction = GetComponent<PlayerActionController>();
        playerMovement = GetComponent<PlayerMovementController>();
        playerCamera = GetComponent<PlayerCameraController>();
        playerSystem = GetComponent<PlayerSystemController>();
        Inventory = GetComponent<Inventory>();

        playerTimeScaleMultiplier = false;
        status = new float[5] { 5f, 10f, 10f, 1f, 1.2f };
        buffModifier = new float[5] { 1f, 1f, 1f, 1f, 1f };

        coolChecks = new bool[4];
        for (int i = 0; i < coolChecks.Length; i++)
            coolChecks[i] = true;

        damageSubscribers = new List<IDamageSubscriber>();
        alive = true;
    }

    public float MAXHP
    {
        get { return maxHP; }
        set
        {
            if (value < 0)
                maxHP = 1f;
            else
                maxHP = value;
            OnHPEvent?.Invoke();
        }
    }

    public float NOWHP
    {
        get { return nowHP; }
        set
        {
            if (value <= 0f)
            {
                if (alive)
                {
                    alive = false;
                    OnHPEvent?.Invoke();
                    playerSystem.Die();
                }
            }
            else
            {
                if(value >= nowHP)
                    GameManager.Data.NowRecords["Heal"] += value - nowHP;
                if(value > MAXHP)
                    nowHP = MAXHP;
                else
                    nowHP = value;
                OnHPEvent?.Invoke();
            }
        }
    }

    public float EXP
    {
        get { return exp; }
        set 
        {
            exp = value;
            while (exp != 0 && exp >= LEVEL * 100f)
            {
                exp -= LEVEL * 100f;
                LEVEL++;
            }
            OnEXPEvent?.Invoke();
        }
    }

    public int LEVEL
    {
        get { return level; }
        set
        {
            level = value;
            playerSystem.LevelUp();
            OnLevelEvent?.Invoke();
        }
    }

    public float MoveSpeed
    {
        get
        {
            return status[0] * buffModifier[0];
        }
        set
        {
            status[0] = value;
        }
    }
    public float JumpPower
    {
        get
        {
            return status[1] * buffModifier[1];
        }
        set
        {
            status[1] = value;
        }
    }
    public float AttackDamage
    {
        get
        {
            return status[2] * buffModifier[2];
        }
        set
        {
            status[2] = value;
        }
    }
    public float CriticalProbability
    {
        get
        {
            return status[3] * buffModifier[3];
        }
        set
        {
            status[3] = value;
        }
    }
    public float CriticalRatio
    {
        get
        {
            return status[4] * buffModifier[4];
        }
        set
        {
            status[4] = value;
        }
    }

    public float TimeScale
    { 
        get 
        {
            if (playerTimeScaleMultiplier)
                return 2f;
            else
                return 1f;
        }
        set
        {
            if (value == 2f)
                playerTimeScaleMultiplier = true;
            else
                playerTimeScaleMultiplier = false;
        }
    }

    public float ReverseTimeScale
    {
        get
        {
            if (playerTimeScaleMultiplier)
                return 0.5f;
            else
                return 1f;
        }
    }

    public Transform playerTransform
    {
        get { return transform; }
        set 
        {
            transform.SetPositionAndRotation(value.position, value.rotation);
            transform.localScale = value.localScale;
        }
    }

    public int Coin
    {
        get { return coin; }
        set 
        { 
            if(value > coin)
                GameManager.Data.NowRecords["Money"] += (value - coin);
            else if(value < coin)
                GameManager.Data.NowRecords["Cost"] += (coin - value);
            coin = value; 
            OnCoinEvent?.Invoke(coin); 
        }
    }
}
