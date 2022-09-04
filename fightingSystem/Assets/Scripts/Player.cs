using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region ����
    /// <summary>
    /// ���ʳt��
    /// </summary>
    [Header("���ʳt��")]
    public float speed;
    /// <summary>
    /// ���o�d�򤺪��ĤH
    /// </summary>
    [Header("���o�d�򤺪��ĤH")]
    public List<GameObject> _enemys;

    /// <summary>
    /// �d�򤺪��ĤH�ƶq
    /// </summary>
    [Header("�d�򤺪��ĤH�ƶq")]
    public int _enemysCount;

    /// <summary>
    /// �Ǫ��X��
    /// </summary>
    [Header("�Ǫ��X��")]
    public List<GameObject> _enemysSkill;
    #endregion
    #region �p��
    /// <summary>
    /// �O�_���b����
    /// </summary>
    bool isrun;
    /// <summary>
    /// �a�V����
    /// </summary>
    float _v;
    /// <summary>
    /// ��V����
    /// </summary>
    float _h;

    /// <summary>
    /// �ۨ�����
    /// </summary>
    Rigidbody2D _rd2d;
    /// <summary>
    /// Aim����
    /// </summary>
    Rigidbody2D _rd2dAim;
    /// <summary>
    /// ��v��
    /// </summary>
    Camera _camera;

    /// <summary>
    /// �s�u����
    /// </summary>
    LineRenderer _lineRenderer;
    /// <summary>
    /// �_�l�I(����)
    /// </summary>
    Transform _startLintPoint;
    /// <summary>
    /// ���I(�ثe�ؼ�)
    /// </summary>
    Transform _endLintPoint;

    Image _timing;

    Text _enemySkill;

    Text _playerSkill;

    GameObject _enemy;

    float _seconds;
    #endregion
    void Start()
    {
        _rd2d = GetComponent<Rigidbody2D>();
        _rd2dAim = GameObject.Find("Aim").gameObject.GetComponent<Rigidbody2D>();
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        isrun = false;

        _timing = GameObject.Find("Timing").GetComponent<Image>();
        _enemySkill = GameObject.Find("���").GetComponent<Text>();
        _playerSkill = GameObject.Find("Skill").GetComponent<Text>();
        _lineRenderer = GetComponent<LineRenderer>();
    }
    void FixedUpdate()
    {
        if (isrun)
        {
            _rd2d.velocity = new Vector2(_h * speed, _v * speed);
            _camera.transform.position = new Vector3(transform.position.x, transform.position.y, _camera.transform.position.z);
        }
        _startLintPoint = transform;



        Aim();
    }

    void Update()
    {

        _v = Input.GetAxis("Vertical");
        _h = Input.GetAxis("Horizontal");
        if (_v != 0 || _h != 0) isrun = true;

        skillKeyDow();

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemys"))
        {
            if (!_enemys.Contains(collision.gameObject))
                _enemys.Add(collision.gameObject);
        }
        ListIndexReset();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (_enemys.Contains(collision.gameObject))
            _enemys.Remove(collision.gameObject);
        if (_enemysSkill.Contains(collision.gameObject))
            _enemysSkill.Remove(collision.gameObject);
        ListIndexReset();
    }

    /// <summary>
    /// �H�ɧ�s�d�򤺪��ĤH�ƶq
    /// </summary>
    void ListIndexReset()
    {
        _enemysCount = _enemys.Count;
    }
    /// <summary>
    /// �ؼ�
    /// </summary>
    void Aim()
    {
        _lineRenderer.SetPosition(0, _startLintPoint.position);
        if (_enemysSkill.Count > 0)
        {
            _endLintPoint = _enemysSkill[0].transform;
            _lineRenderer.SetPosition(1, _endLintPoint.position);
            _rd2dAim.position = new Vector2(
                Mathf.Lerp(_rd2dAim.position.x, _endLintPoint.position.x, speed * Time.fixedDeltaTime),
                Mathf.Lerp(_rd2dAim.position.y, _endLintPoint.position.y, speed * Time.fixedDeltaTime));

            _enemy = _enemysSkill[0];
            timeOut(_enemy);
        }



        if (_enemysSkill.Count == 0)
        {
            _enemy = null;
            _rd2dAim.position = new Vector2(
                Mathf.Lerp(_rd2dAim.position.x, _startLintPoint.position.x, speed * Time.fixedDeltaTime),
                Mathf.Lerp(_rd2dAim.position.y, _startLintPoint.position.y, speed * Time.fixedDeltaTime));
            _lineRenderer.SetPosition(1, _startLintPoint.position);
        }
    }
    /// <summary>
    /// �s�W�Q�����C��
    /// </summary>
    /// <param name="_go">�������H</param>
    public void AddSkill(GameObject _go)
    {
        if (!_enemysSkill.Contains(_go))
            _enemysSkill.Add(_go);
    }
    /// <summary>
    /// �R����������
    /// </summary>
    /// <param name="_go">�Q�R���H</param>
    public void RemoveSkill(GameObject _go)
    {
        if (_enemysSkill.Contains(_go))
            _enemysSkill.Remove(_go);
    }

    /// <summary>
    /// ��ܩۦ�
    /// </summary>
    public void skillKeyDow()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            changeEnemy(_enemy);
            _playerSkill.text = "����";
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            changeEnemy(_enemy);
            _playerSkill.text = "���m";
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            changeEnemy(_enemy);
            _playerSkill.text = "�}��";
        }
    }
    /// <summary>
    /// ���m�p�ɱ�
    /// </summary>
    void timingFillAmountReset()
    {
        _timing.fillAmount = 1;
    }
    /// <summary>
    /// �W��
    /// </summary>
    void timeOut(GameObject _go)
    {
        enemy _enemyScript;
        if (_go != null)
        {
            _seconds += Time.fixedDeltaTime;
            if (_seconds > 1f)
            {
                _timing.fillAmount -= 0.02f;
                if (_timing.fillAmount == 0)
                {
                    _enemyScript = _go.GetComponent<enemy>();
                    _enemyScript.SkillNow();
                    changeEnemy(_go);
                    timingFillAmountReset();
                }
            }
        }
    }
    /// <summary>
    /// �󴫹�H
    /// </summary>
    void changeEnemy(GameObject go)
    {
        if (_enemysSkill.Count > 0)
        {
            if (_enemys.Contains(go))
            {
                _enemysSkill.Add(go);
                timingFillAmountReset();
                _enemysSkill.RemoveAt(0);
            }
        }
    }
    /// <summary>
    /// �{�b�X��
    /// </summary>
    void skillNo(GameObject go)
    {
        enemy _enemyScript;
        if (go != null)
        {
            _enemyScript = go.GetComponent<enemy>();
            _enemyScript.SkillNow();
        }
    }
}
