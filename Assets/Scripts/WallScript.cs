using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    public Sprite[] Sprites;
    public AudioClip broken;
    public GameObject ParticlePrefab;

    public float[] SpritePercentages = new[] {.85f, .7f, .45f, .3f, .15f};
    public float BlinkPercentage = .1f;

    public int MaxHp = 50;
    public float MinSpeedCauseDamage = 1;

    private int currentHP;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator; 
    // Use this for initialization
    void Start()
    {
        currentHP = MaxHp;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool Hit(float speed)
    {
        if (speed < MinSpeedCauseDamage || currentHP <= 0) return false;
        currentHP--;
        if (currentHP == 0)
        {
            AudioScript.instance.PlayOneShot(broken,5);
            Instantiate(ParticlePrefab,transform.position,Quaternion.identity);
            gameObject.SetActive(false);
            return true;
        }

        float percentage = (float) currentHP / MaxHp;
        if (percentage > SpritePercentages[0])
        {
            _spriteRenderer.sprite = Sprites[0];
            return true;
        }

        for (int i = 1; i < SpritePercentages.Length; i++)
        {
            if (percentage <= SpritePercentages[i - 1] && percentage > SpritePercentages[i])
            {
                _spriteRenderer.sprite = Sprites[i];
                return true;
            }
        }

        _spriteRenderer.sprite = Sprites[SpritePercentages.Length];
        if (percentage < BlinkPercentage) _animator.SetBool("Blink", true);
        return true;
    }

    public void Recover()
    {
        if (!gameObject.active) gameObject.SetActive(true);
        currentHP = MaxHp;
        _spriteRenderer.sprite = Sprites[0];
        _animator.SetBool("Blink", false);
    }
}