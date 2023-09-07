using UnityEngine;

public class Inv_Collected : MonoBehaviour
{
    // Nazwa obiektu 
    public string name;
      // Obraz (sprite), który będzie wyświetlany w ekwipunku
    public Sprite image;
    // Referencja do skryptu ekwipunku
    private Inv_Inventory inventory;

    private void Start()
    {
                // Wyszukanie obiektu ze skryptem ekwipunku i przechowanie go w zmiennej
        inventory = FindObjectOfType<Inv_Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {                 
        // Gdy obiekt jest pobierany, wywołujemy metodę AddItem ze skryptu ekwipunku, przekazując
        // sprite obiektu, jego nazwę oraz pobrany przez gracza obiekt
        inventory.AddItem(image, name, gameObject);
    }
}
