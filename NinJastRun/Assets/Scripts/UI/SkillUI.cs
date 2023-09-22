using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image mask;
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        mask.fillAmount = player.currentSkillCD / player.skillCD;
    }
}
