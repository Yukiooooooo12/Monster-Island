using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    [Header("Pengaturan")]
    public float typingSpeed = 0.03f;

    // Dicek oleh script lain (misal PlayerMovement) agar player diam saat dialog
    public bool IsDialogueActive { get; private set; }

    private readonly Queue<string> lines = new Queue<string>();
    private Coroutine typingCoroutine;
    private bool isTyping;
    private string currentLine;

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string npcName, string[] dialogueLines)
    {
        IsDialogueActive = true;
        dialoguePanel.SetActive(true);
        nameText.text = npcName;

        lines.Clear();
        foreach (string line in dialogueLines)
            lines.Enqueue(line);

        DisplayNextLine();
    }

    // Dipanggil setiap player menekan tombol interaksi
    public void DisplayNextLine()
    {
        // Jika teks masih mengetik, langsung tampilkan penuh
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentLine;
            isTyping = false;
            return;
        }

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = lines.Dequeue();
        typingCoroutine = StartCoroutine(TypeLine(currentLine));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        isTyping = false;
        IsDialogueActive = false;
        dialoguePanel.SetActive(false);
    }
}