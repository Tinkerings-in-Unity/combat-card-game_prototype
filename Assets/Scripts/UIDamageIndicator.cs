using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamageIndicator : MonoBehaviour
{
    [SerializeField] private float moveSpeed, lifeTime;
    [SerializeField] private TMP_Text _damageText;
    private RectTransform _rectTransform;

    public void SetDamage(int damageAmount) => _damageText.text = damageAmount.ToString();    
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);

        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        _rectTransform.anchoredPosition += new Vector2(0f, -moveSpeed * Time.deltaTime);
    }
}
