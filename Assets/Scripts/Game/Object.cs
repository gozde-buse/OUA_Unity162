using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public enum Type { Fruit, Vegetable }

    [SerializeField] private Type type;
    [SerializeField] private Sprite[] faces;

    private ObjectButton button;
    private Animator animator;
    private bool dropped;
    private Type? dropBasketType;
    private float yOffset;

    void Awake()
    {
        yOffset = GetComponent<SpriteRenderer>().bounds.size.y / 2;

        animator = GetComponent<Animator>();
        ChangeFace(0);
        AudioController.instance.Play("Sfx", "Excited");
    }

    void Update()
    {
        Type? currentDropBasketType = ControlDropBasket();

        if (currentDropBasketType == Type.Fruit && dropBasketType != Type.Fruit)
        {
            dropBasketType = currentDropBasketType;
            LevelController.instance.ChangeSide(-1);
            ChangeFace(-1);
        }
        else if (currentDropBasketType == Type.Vegetable && dropBasketType != Type.Vegetable)
        {
            dropBasketType = currentDropBasketType;
            LevelController.instance.ChangeSide(1);
            ChangeFace(1);
        }
        else if(currentDropBasketType == null && dropBasketType != null)
        {
            dropBasketType = currentDropBasketType;
            LevelController.instance.ChangeSide(0);
            ChangeFace(0);
        }

#if UNITY_EDITOR

        if (Input.GetMouseButtonUp(0))
            DropToBasket();

#elif UNITY_ANDROID

        if ( Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled )
            DropToBasket();

#endif
    }

    void FixedUpdate()
    {

#if UNITY_EDITOR

        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

#elif UNITY_ANDROID

        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

#endif

        newPosition.y += yOffset;
        newPosition.z = 0;
        transform.position = newPosition;
    }

    private void DropToBasket()
    {
        dropped = true;

        Type? dropBasketType = ControlDropBasket();

        if ((dropBasketType == Type.Fruit && Type.Fruit == type) ||
            (dropBasketType == Type.Vegetable && Type.Vegetable == type))
        {
            AudioController.instance.Play("Sfx", "Correct");
            StartCoroutine(ToBasket());
        }
        else if ((dropBasketType == Type.Fruit && Type.Fruit != type) ||
            (dropBasketType == Type.Vegetable && Type.Vegetable != type))
        {
            AudioController.instance.Play("Sfx", "Wrong");
            StartCoroutine(BackToButton());
        }
        else
        {
            StartCoroutine(BackToButton());
        }
    }

    private void ChangeFace(int index)
    {
        if (dropped)
            return;

        int faceIndex = 0;

        if ((index == -1 && type == Type.Fruit) || (index == 1 && type == Type.Vegetable))
        {
            faceIndex = 1;
            AudioController.instance.Play("Sfx", "Happy");
            animator.SetBool("Dancing", true);
        }
        else if ((index == -1 && type != Type.Fruit) || (index == 1 && type != Type.Vegetable))
        {
            faceIndex = 2;
            AudioController.instance.Play("Sfx", "Sad");
            animator.SetBool("Dancing", false);
        }
        else
        {
            faceIndex = 0;
            animator.SetBool("Dancing", false);
        }

        foreach (Transform face in transform)
            face.GetComponent<SpriteRenderer>().sprite = faces[faceIndex];
    }

    private Type? ControlDropBasket()
    {

#if UNITY_EDITOR

        Vector3 dropPosition = Input.mousePosition;

#elif UNITY_ANDROID

        Vector3 dropPosition = Input.GetTouch(0).position;

#endif

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
