using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;      // simpan slot asal
        transform.SetParent(transform.root);    // tampil di atas semua UI
        canvasGroup.blocksRaycasts = false;     // supaya slot di bawah bisa terdeteksi
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot originalSlot = originalParent.GetComponent<Slot>();
        Slot dropSlot = null;

        // Cari slot di bawah pointer (bisa objek slot-nya, atau item di dalam slot)
        if (eventData.pointerEnter != null)
        {
            dropSlot = eventData.pointerEnter.GetComponentInParent<Slot>();
        }

        // Dilepas di luar slot, atau di slot asalnya sendiri: kembali ke posisi semula
        if (dropSlot == null || dropSlot == originalSlot)
        {
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            return;
        }

        if (dropSlot.currentItem != null)
        {
            // SWAP: item2 pindah ke slot asal item1
            GameObject item2 = dropSlot.currentItem;
            item2.transform.SetParent(originalSlot.transform);
            item2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            originalSlot.currentItem = item2;
        }
        else
        {
            // Slot tujuan kosong: slot asal jadi kosong
            originalSlot.currentItem = null;
        }

        // Item1 pindah ke slot tujuan
        transform.SetParent(dropSlot.transform);
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        dropSlot.currentItem = gameObject;
    }
}