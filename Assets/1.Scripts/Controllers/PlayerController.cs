using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    float _speed = 10.0f;

    Vector3 _destPos;

    void Start()
    {
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle
    }
    PlayerState _state = PlayerState.Idle;

    void UpdateDie()
    {
        //아무것도 안함

    }
    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            //1000
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                _state = PlayerState.Idle;
                return;
            }


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            transform.LookAt(_destPos);
        }

        //애니메이션 처리
        Animator anim = GetComponent<Animator>();
        //현재 게임 상태에 대한 정보를 넘겨준다
        anim.SetFloat("speed", _speed);
    }
    void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    void Update()
    {
        switch (_state)
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

        }
    }


    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _destPos =  hit.point;
            _state = PlayerState.Moving;
        }
    }
}
