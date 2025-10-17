using UnityEngine;
using UnityEngine.EventSystems;

public class StoveDropZone : MonoBehaviour, IDropHandler
{
    [Header("Cookware Prefabs")]
    public GameObject panPrefab;
    public GameObject potPrefab;
    public GameObject wokPrefab;

    [Header("Mini-Game Panels")]
    public GameObject mixingPanel;
    public GameObject simmerClockPanel;
    public GameObject boilClockPanel;
    public GameObject sautePanel;

    [Header("AudioSetting")]
    [Range(0f, 1f)] private float sfxVolume = 1f;
    private AudioSource audioSource;    

    private GameObject currentCookware;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null )
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            Debug.LogWarning("[StoveDropZone] pointerDrag is null");
            return;
        }

        ItemData draggedData = eventData.pointerDrag.GetComponent<ItemData>();
        if (draggedData == null || draggedData.itemSO == null)
        {
            Debug.LogWarning("[StoveDropZone] No valid ItemData on dragged object");
            return;
        }

        ItemSO item = draggedData.itemSO;
        Debug.Log($"[StoveDropZone] Dropped item: {item.itemName}, Type: {item.itemType}");

        // === 1️⃣ Handle Cookware (Pan, Pot, Wok, etc.) ===
        if (item.itemType == ItemType.Utility)
        {
            TryPlaceCookware(item, eventData);
            return;
        }

        // === 2️⃣ Handle Ingredient ===
        if (currentCookware != null && item.itemType == ItemType.Ingredient)
        {
            HandleIngredientDrop(item, draggedData);
            return;
        }

        Debug.Log("[StoveDropZone] Drop ignored (no matching condition).");
    }

    private void TryPlaceCookware(ItemSO item, PointerEventData eventData)
    {
        if (currentCookware != null)
        {
            Debug.Log("[StoveDropZone] Cookware already placed");
            return;
        }

        GameObject prefabToSpawn = GetCookwarePrefab(item.itemName);
        if (prefabToSpawn == null)
        {
            Debug.LogWarning($"[StoveDropZone] No prefab found for {item.itemName}");
            return;
            //CookingStepManager.Instance.NextStep();
        }

        currentCookware = Instantiate(prefabToSpawn, transform);
        currentCookware.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        PanStateHandler handler = currentCookware.GetComponent<PanStateHandler>();
        if (handler != null)
        {
            handler.mixingMechPanel = mixingPanel;
            handler.simmerClockPanel = simmerClockPanel;
            handler.boilClockPanel = boilClockPanel;
            handler.sauteMechPanel = sautePanel;
        }

        PlaySFX(item.dropSFX);

        Debug.Log($"✅ {item.itemName} placed on stove");
        Destroy(eventData.pointerDrag);
        CookingStepManager.Instance.NextStep();
    }

    private GameObject GetCookwarePrefab(string itemName)
    {
        switch (itemName)
        {
            case "Pan": return panPrefab;
            case "Pot": return potPrefab;
            case "Wok": return wokPrefab;
            default: return null;
        }
    }

    private void HandleIngredientDrop(ItemSO item, ItemData draggedData)
    {
        bool correct = CookingStepManager.Instance.IsCorrectItem(item);
        if (correct)
        {
            PanStateHandler handler = currentCookware.GetComponent<PanStateHandler>();
            handler.AdvanceStep();

            CookingStepManager.Instance.NextStep();
            Destroy(draggedData.gameObject);
        }
        else
        {
            draggedData.GetComponent<Draggable>()?.RevertToOriginalPosition();
            CookingStepManager.Instance.WrongAttempt();
        }
    }
    public void ResetStove()
    {
        if (currentCookware != null)
        {
            Destroy(currentCookware);
            currentCookware = null;
            Debug.Log("[StoveDropZone] Stove reset.");
        }
    }
    private void PlaySFX(AudioClip clip)
    {
        if(clip == null)
        {
            Debug.LogWarning("[StoveDropZone] No SFX clip assigned!");
            return;
        }
        Debug.Log($"[StoveDropZone] Playing sound: {clip.name}");
        audioSource.PlayOneShot(clip, sfxVolume);

    }
}
