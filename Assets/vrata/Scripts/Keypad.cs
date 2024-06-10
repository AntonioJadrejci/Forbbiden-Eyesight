using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    [SerializeField] private Text Ans;
    [SerializeField] private Animator Door;
    [SerializeField] private GameObject canvas;

    private string Answer = "532";

    public void Number(int number)
    {
        Ans.text += number.ToString();
    }

    public void Execute()
    {
        if (Ans.text == Answer)
        {
            Ans.text = "RUN";
            Door.SetBool("Open", true);

            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
        else
        {
            Ans.text = "DEAD";
        }
    }
}