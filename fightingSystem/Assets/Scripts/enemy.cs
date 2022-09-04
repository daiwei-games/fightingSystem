using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class enemy : MonoBehaviour
{
    /// <summary>
    /// �D��
    /// </summary>
    GameObject _player;
    /// <summary>
    /// �D���}��
    /// </summary>
    Player _playerScript;
    /// <summary>
    /// �O�_�I���D��
    /// </summary>
    bool isplayer;
    /// <summary>
    /// ��ʶ��j
    /// </summary>
    float _seconds;
    /// <summary>
    /// �p�ɽu
    /// </summary>
    Image _timing;
    /// <summary>
    /// ��ܩۦ�
    /// </summary>
    Text _enemySkill;

    Vector2 _v2;
    Vector2 _v2Player;

    /// <summary>
    /// �樫�t��
    /// </summary>
    [Header("�樫�t��")]
    public float _speed;

    [Header("�ۦ���")]
    public string[] _skill;

    string skillNow;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
            _playerScript = _player.GetComponent<Player>();
        _seconds = 2f;
        _speed = 20f;

        _timing = GameObject.Find("Timing").GetComponent<Image>();
        _enemySkill = GameObject.Find("���").GetComponent<Text>();
    }


    void FixedUpdate()
    {
        runToPlayer();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isplayer = true;
            StartCoroutine(skillPlayer(gameObject));

        }
        StartCoroutine("RunToPlayer");
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isplayer = false;
            StartCoroutine(skillPlayer(gameObject));
        }
    }
    /// <summary>
    /// ��D���X��
    /// </summary>
    IEnumerator skillPlayer(GameObject _go)
    {
        yield return new WaitForSeconds(_seconds);
        if (isplayer)
            _playerScript.AddSkill(gameObject);
        else
            _playerScript.RemoveSkill(gameObject);
    }
    /// <summary>
    /// �p�ɶ]�V�D��
    /// </summary>
    IEnumerator RunToPlayer()
    {
        while (isplayer)
        {
            if ((transform.position - _player.transform.position).sqrMagnitude > 15f)
            {

                transform.position = new Vector2(
                    Mathf.Lerp(_v2.x, _v2Player.x, _speed * Time.fixedDeltaTime),
                    Mathf.Lerp(_v2.y, _v2Player.y, _speed * Time.fixedDeltaTime));

            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    /// <summary>
    /// �]�V�D��
    /// </summary>
    void runToPlayer()
    {
        _v2 = transform.position;
        _v2Player = _player.transform.position;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SkillNow()
    {
        skillNow = _skill[Random.Range(0, _skill.Length)];
        _enemySkill.text = skillNow;
    }

}
