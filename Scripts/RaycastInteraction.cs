using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    public Color selectedColor = Color.green; // Seçilince yeşil olsun
    public Color defaultColor = Color.white;  // Normal hali beyaz
    private GameObject selectedObject;

    void Update()
    {
        // 1. SOL TIK: NESNE SEÇME (Raycast)
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }

        // 2. SPACE: YUKARI TAŞIMA
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveSelectedObjectUp();
        }

        // 3. DELETE: SİLME (Geliştirme Görevi)
        if (Input.GetKeyDown(KeyCode.Delete) && selectedObject != null)
        {
            Destroy(selectedObject);
            Debug.Log("Nesne silindi.");
        }
    }

    void SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Tag Kontrolü (Ödev maddesi)
            if (hit.collider.CompareTag("Selectable"))
            {
                // Eski seçilenin rengini geri düzelt
                if (selectedObject != null)
                {
                    selectedObject.GetComponent<Renderer>().material.color = defaultColor;
                }

                // Yeni nesneyi seç ve rengini değiştir
                selectedObject = hit.collider.gameObject;
                selectedObject.GetComponent<Renderer>().material.color = selectedColor;

                // Konsola isim yazdır (Ödev maddesi)
                Debug.Log("Seçilen nesne: " + selectedObject.name);
            }
        }
    }

    void MoveSelectedObjectUp()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.position += Vector3.up * 1f;
            Debug.Log(selectedObject.name + " yukarı taşındı.");
        }
    }
}