using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class NPCDialogue : MonoBehaviour
{
    [Header("Data NPC")]
    public string npcName = "NPC";

    [TextArea(2, 5)]
    public string[] dialogueLines;

    [Header("Interaksi")]
    public KeyCode interactKey = KeyCode.E;
    public GameObject interactPrompt; // contoh: teks "Tekan E" di atas kepala NPC

    private bool playerInRange;

    void Start()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!DialogueManager.Instance.IsDialogueActive)
            {
                DialogueManager.Instance.StartDialogue(npcName, dialogueLines);
                if (interactPrompt != null) interactPrompt.SetActive(false);
            }
            else
            {
                DialogueManager.Instance.DisplayNextLine();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        if (interactPrompt != null && !DialogueManager.Instance.IsDialogueActive)
            interactPrompt.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (interactPrompt != null) interactPrompt.SetActive(false);

        // Jika player menjauh saat dialog, tutup dialog
        if (DialogueManager.Instance.IsDialogueActive)
            DialogueManager.Instance.EndDialogue();
    }
}