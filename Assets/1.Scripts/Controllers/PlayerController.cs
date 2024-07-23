using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;
    Vector3 _destPos;

    Animator anim;

    void Start()
    {

        _stat = gameObject.GetComponent<PlayerStat>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
        anim = GetComponent<Animator>();
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,

    }
    PlayerState _state = PlayerState.Idle;
    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case PlayerState.Die:
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Idle:
                    anim.SetFloat("speed", 0);
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Moving:
                    anim.SetBool("attack", false);
                    anim.SetFloat("speed", _stat.MoveSpeed);
                    break;
                case PlayerState.Skill:
                    anim.SetBool("attack", true);
                    break;
            }
        }
    }

    void UpdateDie()
    {
        //�ƹ��͵� ����

    }
    void UpdateMoving()
    {
        //���Ͱ� �� �����Ÿ����� ������ ����
        if (_lockTarget != null)
        {
            float distance =  (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }
        //�̵�
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = PlayerState.Idle;
        }
        else
        {
            
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            //1000
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(dir.normalized * moveDist);

            if (Physics.Raycast(transform.position, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (!Input.GetMouseButton(0))
                    State = PlayerState.Idle;
                return;
            }


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        
    }
    void UpdateIdle()
    {
        
    }
    void UpdateSkill()
    {
        
    }
    void OnHitEvent()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("attack", false);

        State = PlayerState.Idle;

    }
    void Update()
    {
        switch (State)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Skill:
                UpdateSkill();
                break;

        }
    }


    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    GameObject _lockTarget;
    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (State == PlayerState.Die) 
            return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        State = PlayerState.Moving;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget != null)
                        _destPos = _lockTarget.transform.position;
                    else if (raycastHit)
                            _destPos = hit.point;
                }
                break;
        }

    }
}
