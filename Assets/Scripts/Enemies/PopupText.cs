using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupText : MonoBehaviour
{

    public Animator animator;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        AnimatorClipInfo[] infos = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, infos[0].clip.length); // destroy object once animation has ended
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }

    public void SetColor(Color c)
    {
        text.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
