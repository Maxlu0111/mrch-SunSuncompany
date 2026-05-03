using UnityEngine;
using System.Collections;

public class detectwalk : MonoBehaviour
{
    // 在 Inspector 面板中拖入对应的 UI 对象
    public GameObject choice1;
    public GameObject choice2;
    public GameObject questionUI; // 新增：Question UI 画面

    void Start()
    {
        // 初始状态下隐藏所有 UI
        if (choice1 != null) choice1.SetActive(false);
        if (choice2 != null) choice2.SetActive(false);
        if (questionUI != null) questionUI.SetActive(false); // 初始隐藏 Question
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Soldier"))
        {
            Animator anim = other.GetComponentInChildren<Animator>();

            if (anim != null)
            {
                anim.Play("stopwalking");
                Debug.Log("成功找到子物体的动画机并播放！");
                
                // --- 新增逻辑：触发时立即显示 Question 画面 ---
                if (questionUI != null) 
                {
                    questionUI.SetActive(true);
                }
                // ------------------------------------------

                // 开启协程，处理 2 秒延迟显示 按钮
                StartCoroutine(ShowUIWithDelay(2f));
            }
            else
            {
                Debug.LogError("在 " + other.name + " 的子物体里也没找到 Animator！");
            }
        }
    }

    IEnumerator ShowUIWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 2秒后显示具体的回答选项按钮
        if (choice1 != null) choice1.SetActive(true);
        if (choice2 != null) choice2.SetActive(true);
        
        Debug.Log("UI 选项按钮已显示");
    }
}