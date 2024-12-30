using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public string[] lines;

    public float textSpeed;
    private int index;

    private void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    
    private IEnumerator TypeLine()
    {
        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
        }
    }


    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}