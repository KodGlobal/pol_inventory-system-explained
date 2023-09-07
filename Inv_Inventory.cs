using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inv_Inventory : MonoBehaviour
{       
    // Lista wszystkich przycisków inwentarza
    [SerializeField] List<Button> buttons = new List<Button>();   
    // Wszystkie obiekty z folderu Zasoby
    [SerializeField] List<GameObject> resourceItems = new List<GameObject>();
    [SerializeField] GameObject buttonsPath;
    // Nazwy obiektów, które zebraliśmy
    [SerializeField] List<string> inventoryItems = new List<string>();
    // Obiekt, który jest aktualnie wyposażony
    GameObject itemInArm;
    // Miejsce, w którym obiekt ma się pojawić
    [SerializeField] Transform itemPoint;
    [SerializeField] Transform[] itemPositions;
    // Pole tekstowe dla ostrzeżeń inwentarza (Tekst)
    [SerializeField] TMP_Text warning;
    // Lista przedmiotów zebranych przez gracza
    [SerializeField] List<GameObject> playerItems = new List<GameObject>();
    GameObject itemPosition;


    private void Start()
    {
        // Wczytywanie wszystkich przedmiotów inwentarza znajdujących się w folderze Zasoby
        GameObject[] objArr = Resources.LoadAll<GameObject>("Items");
        
        // Wypełnianie listy możliwych przedmiotów inwentarza
        resourceItems.AddRange(objArr);
        // Przechodzenie przez wszystkie przyciski inwentarza i przechowywanie ich na liście
        foreach(Transform child in buttonsPath.transform)
        {
            buttons.Add(child.GetComponent<Button>());
        }
    }
    private void Update()
    {
        // Włączanie/wyłączanie kursora myszy, gdy gracz naciska Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void AddItem(Sprite img, string itemName, GameObject obj)
    {        
        // Jeśli inwentarz jest pełny, wyświetlamy komunikat ostrzegawczy i przerywamy metodę
        if (inventoryItems.Count >= buttons.Count)
        {
            warning.text = "Full Inventory!";
            Invoke("WarningUpdate", 1f);
            return;
        }
        // Jeśli gracz już ma kopię przedmiotu, wyświetlamy komunikat ostrzegawczy i przerywamy metodę
        if (inventoryItems.Contains(itemName))
        {
            warning.text = "You already have " + itemName;
            Invoke("WarningUpdate", 1f);
            return;
        }
        // Dodawanie nazwy przedmiotu do inwentarza
        inventoryItems.Add(itemName);
        // Pobieranie następnego wolnego przycisku i jego komponentu Obrazu
        var buttonImage = buttons[inventoryItems.Count - 1].GetComponent<Image>();
        // Ustawianie obrazu przycisku na obrazek zebranego przedmiotu
        buttonImage.sprite = img;
        // Niszczenie zebranego przedmiotu
        Destroy(obj);
    }
    // Metoda usuwająca wszystkie komunikaty ostrzegawcze
    void WarningUpdate()
    {
        warning.text = "";
    }
    // Ta metoda jest wywoływana, gdy naciśnięty zostanie przycisk
    public void UseItem(int itemPos)
    {           
        // Jeśli nacisnęliśmy przycisk, do którego nie jest dołączony żaden przedmiot, przerywamy funkcję
        if (inventoryItems.Count <= itemPos) return;
        // Przechowywanie nazwy obiektu dołączonego do tego przycisku w zmiennej
        string item = inventoryItems[itemPos];
        // Wywoływanie metody, która wyposaża przedmiot z inwentarza, przekazując nazwę przedmiotu jako inwentarz
        GetItemFromInventory(item);
    }
    // Ta metoda wyposaża przedmiot. Jest ona wywoływana z metody UzyjPrzedmiot
    public void GetItemFromInventory(string itemName)
    {
        // Wyszukiwanie obiektu o określonej nazwie na liście wszystkich obiektów
        var resourceItem = resourceItems.Find(x => x.name == itemName);
        // Jeśli obiekt o takiej nazwie nie istnieje w folderze zasobów, przerywamy funkcję

        if (resourceItem == null) return;

        // Szukanie obiektu o określonej nazwie na liście obiektów gracza
        var putFind = playerItems.Find(x => x.name == itemName);

        // Jeśli taki przedmiot nie istnieje, to
        if (putFind == null)
        {
            // Jeśli gracz ma już zaopatrzony przedmiot, wyłączamy go
            if (itemInArm != null)
            {
                itemInArm.SetActive(false);
            }
                // Sprawdzanie, do której części ciała powinien być dołączony obiekt
            var pos = resourceItem.GetComponent<Inv_ItemPosition>().positon;
            if (pos == Inv_ItemPosition.ItemPos.Head)
            {
                itemPoint.position = itemPositions[0].position;
                itemPosition = itemPositions[0].gameObject;
            }
            else if (pos == Inv_ItemPosition.ItemPos.Spine)
            {
                itemPoint.position = itemPositions[1].position;
                itemPosition = itemPositions[1].gameObject;
            }
            else
            {
                itemPoint.position = itemPositions[2].position;
                itemPosition = itemPositions[2].gameObject;
            }
            // Tworzenie obiektu w wcześniej zdefiniowanym punkcie
            var newItem = Instantiate(resourceItem, itemPoint);
            // Przenoszenie tego obiektu do hierarchii gracza
            newItem.transform.parent = itemPosition.transform;
            // Nadawanie nowemu obiektowi nazwy
            newItem.name = itemName;
            // Dodawanie obiektu do listy obiektów w ekwipunku gracza
            playerItems.Add(newItem);
                // Informowanie Unity, że ekwipunek itemInArm równa się nowo wyposażonemu przedmiotowi (innymi słowy, pamiętamy przedmiot, który jest obecnie wyposażony)
            itemInArm = newItem;
        }
        // Jeśli ten przedmiot już istnieje na scenie, to
        else
        {
                // Jeśli ten obiekt to obiekt, który jest już wyposażony, po prostu zmieniamy jego stan
            {
                putFind.SetActive(!putFind.activeSelf);
            }
                // Jeśli to inny obiekt, po prostu wyłączamy obecnie wyposażony przedmiot i włączamy nowy
            else
            {
                itemInArm.SetActive(false);
                putFind.SetActive(true);
                itemInArm = putFind;
            }
        }
    }
}
