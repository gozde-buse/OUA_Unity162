using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public enum Type { Fruit, Vegetable }
    public enum Fruit { None, Strawberry }
    public enum Vegetable { None, Onion }

    [SerializeField] private Type type;
    [SerializeField] private Fruit fruitName;
    [SerializeField] private Vegetable vegetableName;

    private GameObject button;

    void Awake()
    {
        if (type == Type.Fruit)
            vegetableName = Vegetable.None;
        else
            fruitName = Fruit.None;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
            DropToBasket();
    }

    void FixedUpdate()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0;
        transform.position = newPosition;
    }

    private void DropToBasket()
    {
        Type? dropBasketType = ControlDropBasket();

        if(dropBasketType == Type.Fruit)
        {
            if (Type.Fruit == type)
            {
                Debug.Log("Doðru.");
                button.GetComponent<ObjectButton>().CorrectPlacement();
            }
            else
            {
                Debug.Log("Yanlýþ.");
                button.GetComponent<ObjectButton>().LoadBack();
            }
        }
        else if(dropBasketType == Type.Vegetable)
        {
            if (Type.Vegetable == type)
            {
                Debug.Log("Doðru.");
                button.GetComponent<ObjectButton>().CorrectPlacement();
            }
            else
            {
                Debug.Log("Yanlýþ.");
                button.GetComponent<ObjectButton>().LoadBack();
            }
        }
        else
            button.GetComponent<ObjectButton>().LoadBack();

        Destroy(gameObject);
    }

    private Type? ControlDropBasket()
    {
        Vector2 dropPosition = Camera.main.WorldToScreenPoint(transform.position);

        if (dropPosition.y < LoadLevel1.yLimit)
            return null;
        if (dropPosition.x < LoadLevel1.xLimit)
            return Type.Fruit;
        else
            return Type.Vegetable;
    }

    public void SetButton(GameObject button)
    {
        this.button = button;
    }
}
