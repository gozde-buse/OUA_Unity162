using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public enum Type { Fruit, Vegetable }

    [SerializeField] private Type type;
    [SerializeField] private Sprite[] faces;

    private ObjectButton button;

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

        Type? dropBasketType = ControlDropBasket();

        if (dropBasketType == Type.Fruit)
        {
            LevelController.instance.ChangeSide(-1);
            ChangeFace(-1);
        }
        else if (dropBasketType == Type.Vegetable)
        {
            LevelController.instance.ChangeSide(1);
            ChangeFace(1);
        }
        else
        {
            LevelController.instance.ChangeSide(0);
            ChangeFace(0);
        }
    }

    private void DropToBasket()
    {
        Type? dropBasketType = ControlDropBasket();

        if((dropBasketType == Type.Fruit && Type.Fruit == type) ||
            (dropBasketType == Type.Vegetable && Type.Vegetable == type))
        {
            StartCoroutine(ToBasket());
        }
        else
        {
            StartCoroutine(BackToButton());
        }
    }

    private void ChangeFace(int index)
    {
        int faceIndex = 0;

        if ((index == -1 && type == Type.Fruit) || (index == 1 && type == Type.Vegetable))
        {
            faceIndex = 1;
            //Animasyonu baþlat
        }
        else if ((index == -1 && type != Type.Fruit) || (index == 1 && type != Type.Vegetable))
        {
            faceIndex = 2;
            //Animasyonu durdur
        }
        else
        {
            faceIndex = 0;
            //Animasyonu durdur
        }

        foreach (Transform face in transform)
            face.GetComponent<SpriteRenderer>().sprite = faces[faceIndex];
    }

    private Type? ControlDropBasket()
    {
        Vector2 dropPosition = Camera.main.WorldToScreenPoint(transform.position);

        if (dropPosition.y < LevelController.instance.yLimit)
            return null;
        if (dropPosition.x < LevelController.instance.xLimit)
            return Type.Fruit;
        else
            return Type.Vegetable;
    }

    private IEnumerator ToBasket()
    {
        float animationTime = .2f;
        float elapsedTime = 0;

        Vector3 initialPosition = transform.position;
        Vector3 endPosition = button.GetCorrectObjectPosition();
        endPosition.z = transform.position.z;

        Vector3 initialScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        while (elapsedTime <= animationTime)
        {
            transform.position = Vector3.Lerp(initialPosition, endPosition, elapsedTime / animationTime);
            transform.localScale = Vector3.Lerp(initialScale, endScale, elapsedTime / animationTime);
            elapsedTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        transform.position = endPosition;
        transform.localScale = endScale;
        button.CorrectPlacement();
        Destroy(gameObject);
    }

    private IEnumerator BackToButton()
    {
        float animationTime = .3f;
        float elapsedTime = 0;

        Vector3 initialPosition = transform.position;
        Vector3 endPosition = button.transform.position;
        endPosition.z = transform.position.z;

        while (elapsedTime <= animationTime)
        {
            transform.position = Vector3.Lerp(initialPosition, endPosition, elapsedTime / animationTime);
            elapsedTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        transform.position = endPosition;
        button.LoadBack();
        Destroy(gameObject);
    }

    public void SetButton(ObjectButton button)
    {
        this.button = button;
    }
}
