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
                                                        // �κ��丮�� ���� ������Ƽ

    [SerializeField] float maxHP, nowHP, exp;           // �ִ� ü��, ���� ü��, ���� ����ġ
    [SerializeField] int level, coin;                   // ���� ����, ���� ��ȭ
    [SerializeField] float[] status;                    // �ɷ�ġ �迭(�̵��ӵ�, ��������, ���ݷ�, ġ��Ÿ Ȯ��, ġ��Ÿ ����)
    public float[] buffModifier;                        // �� �ɷ�ġ�� ���� ���� �迭

    public int jumpLimit, jumpCount;                                                            // �Ѱ� ���� ��, ���� ���� ��
    public bool attackCooldown, controllable, dodgeDamage, onGizmo, onESC, alive, onSession;    // ���� ��Ÿ�� ����, ���� ���� ����, ȸ�� ����, ����� ��� ����, ESCâ ����

    public float coolTime;                                  // ��Ÿ��
    public bool[] coolChecks = new bool[4];                 // �� �׼� ��Ÿ�� ����

    public float mouseSensivity;                            // ���콺 �ΰ���
    [SerializeField] bool playerTimeScaleMultiplier;        // �÷��̾� �ð� ��� ����

    public UnityEvent OnLevelEvent, OnHPEvent, OnEXPEvent;  // ���� �� �̺�Ʈ, ü�� ��ȭ �̺�Ʈ, ����ġ ��ȭ �̺�Ʈ
    public UnityEvent<int> OnCoinEvent;                     // ��ȭ ȹ���̺�Ʈ
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
        // ������ �Ŵ����� �÷��̾� ���
        GameManager.Data.Player = this;

        // ���� ����
        heroList = new List<Hero>();                            // ���� ����Ʈ �ʱ�ȭ
        heroList.AddRange(GetComponentsInChildren<Hero>());     // ĳ���Ϳ� ���Ե� �������� ����Ʈ�� �߰�
        for(int i = 0; i < heroList.Count; i++)
            heroList[i].gameObject.SetActive(false);            // ��� ������ �ϴ� ��Ȱ��ȭ

        // �ʿ� ������Ʈ ����
        rb = GetComponent<Rigidbody>();                             // ��ü
        playerAction = GetComponent<PlayerActionController>();      // �ൿ �� ��ȣ�ۿ� ��ũ��Ʈ
        playerMovement = GetComponent<PlayerMovementController>();  // �̵� ��ũ��Ʈ
        playerCamera = GetComponent<PlayerCameraController>();      // ī�޶� ��ũ��Ʈ
        playerSystem = GetComponent<PlayerSystemController>();      // ��Ÿ ��ũ��Ʈ
        Inventory = GetComponent<Inventory>();                      // �κ��丮

        // �ΰ��� ���� ������ �ʱ�ȭ
        playerTimeScaleMultiplier = false;                          // �ð� ��� ����
        status = new float[5] { 5f, 10f, 10f, 1f, 1.2f };         // �ʱ� �ɷ�ġ : �̵��ӵ�, ��������, ���� �����, ġ��Ÿ Ȯ��, ġ��Ÿ ����
        buffModifier = new float[5] { 1f, 1f, 1f, 1f, 1f };         // ���� ����ġ : �ɷ�ġ�� ���� ����
        coolChecks = new bool[4];                                   // 4�� ����� ���� ��Ÿ�� üũ
        for (int i = 0; i < coolChecks.Length; i++)
            coolChecks[i] = true;                                   // ��Ÿ�� �ʱ�ȭ
        damageSubscribers = new List<IDamageSubscriber>();          // ����� ������ ����Ʈ
        alive = true;                                               // �÷��̾� ���� ����
    }

    /// <summary>
    /// �ִ� ü�� ������Ƽ
    /// </summary>
    public float MAXHP
    {
        get { return maxHP; }
        set
        {
            if (value < 1f)
                maxHP = 1f;         // �ּ� 1 �̻��� ���� ���´�
            else
                maxHP = value;
            OnHPEvent?.Invoke();    // ü�� �̺�Ʈ �߻�
        }
    }

    /// <summary>
    /// ���� ü�� ������Ƽ
    /// </summary>
    public float NOWHP
    {
        get { return nowHP; }
        set
        {
            // ���� 0 �����̰� ���� ����ִٸ�
            if (value <= 0f && alive)
            {
                alive = false;          // ��� ����
                OnHPEvent?.Invoke();    // ü�� �̺�Ʈ �߻�
                playerSystem.Die();     // ��� �޼ҵ� ����
            }
            else
            {
                if(value >= nowHP)      // ���� ü���� �þ�ٸ� ȸ������ �߰�
                    GameManager.Data.NowRecords["Heal"] += value - nowHP;
                if(value > MAXHP)       // �þ ���� �ִ뺸�� ũ�ٸ� �ִ� ü�±�����
                    nowHP = MAXHP;
                else
                    nowHP = value;
                OnHPEvent?.Invoke();    // ü�� �̺�Ʈ �߻�
            }
        }
    }

    /// <summary>
    /// ����ġ ������Ƽ
    /// </summary>
    public float EXP
    {
        get { return exp; }
        set 
        {
            exp = value;
            while (LEVEL != 0 && exp >= LEVEL * 100f) // ���� 0�� �ƴѵ�, ����ġ�� ���� * 100 �̻��̶��
            {
                exp -= LEVEL * 100f;                  // ����ġ�� ���̰�
                LEVEL++;                              // ������ ����
            }
            OnEXPEvent?.Invoke();                     // ����ġ �̺�Ʈ �߻�
        }
    }

    /// <summary>
    /// ���� ������Ƽ
    /// </summary>
    public int LEVEL
    {
        get { return level; }
        set
        {
            level = value;              // ���� ���� �����ϰ�
            playerSystem.LevelUp();     // ������ �޼ҵ� ����
            OnLevelEvent?.Invoke();     // ���� �̺�Ʈ �߻�
        }
    }

    /// <summary>
    /// �̵� �ӵ� ������Ƽ
    /// </summary>
    public float MoveSpeed
    {
        // �̵� �ӵ� * �̵��ӵ� �������� ��ȯ
        get { return status[0] * buffModifier[0];}
        // �̵� �ӵ��� �״�� ����
        set { status[0] = value; }
    }

    /// <summary>
    /// ���� ���� ������Ƽ
    /// </summary>
    public float JumpPower
    {
        // ���� ���� * ���� ���� �������� ��ȯ
        get { return status[1] * buffModifier[1]; }
        // ���� ���̴� �״�� ����
        set { status[1] = value; }
    }

    /// <summary>
    /// ���� ����� ������Ƽ
    /// </summary>
    public float AttackDamage
    {
        // ���� ����� * ���� ����� �������� ��ȯ
        get { return status[2] * buffModifier[2]; }
        // ���� ������� �״�� ����
        set { status[2] = value; }
    }

    /// <summary>
    /// ġ��Ÿ Ȯ�� ������Ƽ
    /// </summary>
    public float CriticalProbability
    {
        // ġ��Ÿ Ȯ�� * ġ��Ÿ Ȯ�� �������� ��ȯ
        get { return status[3] * buffModifier[3]; }
        // ġ��Ÿ Ȯ���� �״�� ����
        set { status[3] = value; }
    }

    /// <summary>
    /// ġ��Ÿ ���� ������Ƽ
    /// </summary>
    public float CriticalRatio
    {
        // ġ��Ÿ ���� * ġ��Ÿ ���� �������� ��ȯ
        get { return status[4] * buffModifier[4]; }
        // ġ��Ÿ ������ �״�� ����
        set { status[4] = value; }
    }

    /// <summary>
    /// �÷��̾� �ð� ��� ������Ƽ
    /// </summary>
    public float TimeScale
    { 
        get 
        {
            if (playerTimeScaleMultiplier)  // ������̶�� 2 ��ȯ
                return 2f;
            else                            // �ƴ϶�� 1 ��ȯ
                return 1f;
        }
        set
        {
            if (value == 2f)                // 2�� �Է¹����� �����
                playerTimeScaleMultiplier = true;
            else                            // �ƴ϶�� ����� �ƴ�
                playerTimeScaleMultiplier = false;
        }
    }

    /// <summary>
    /// �ð� ��� ������ ���� ������Ƽ
    /// </summary>
    public float ReverseTimeScale
    {
        get
        {
            if (playerTimeScaleMultiplier)  // 2�� ���� 0.5 ��ȯ
                return 0.5f;
            else                            // 1�� ���� 1 ��ȯ
                return 1f;
        }
    }

    /// <summary>
    /// �÷��̾� transform ������Ƽ
    /// </summary>
    public Transform playerTransform
    {
        get { return transform; }   // transform ��ȯ
        set 
        {
            // ��ġ, ȸ��, ũ�� ����
            transform.SetPositionAndRotation(value.position, value.rotation);
            transform.localScale = value.localScale;
        }
    }

    /// <summary>
    /// ��ȭ ������Ƽ
    /// </summary>
    public int Coin
    {
        get { return coin; }    // ��ȭ ��ȯ
        set 
        { 
            if(value > coin)            // ��ȭ�� �þ�ٸ� ȹ�� ��ȭ �߰�
                GameManager.Data.NowRecords["Money"] += (value - coin);
            else if(value < coin)       // �پ����ٸ� �Ҹ� ��ȭ �߰�
                GameManager.Data.NowRecords["Cost"] += (coin - value);
            coin = value;               // ��ȭ�� �����ϰ�
            OnCoinEvent?.Invoke(coin);  // ��ȭ �̺�Ʈ �ߵ�
        }
    }
}
