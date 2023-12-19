using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Displayable : MonoBehaviour
{
    public enum State {
        visible,
        hidden,
        none,
    }
    public State state;
    public float fade_duration = 0.3f; 
    private  CanvasGroup canvasGroup;

    public CanvasGroup CanvasGroup{
        get{
            if (canvasGroup == null){
                canvasGroup = gameObject.AddComponent(typeof(CanvasGroup)) as CanvasGroup;
            }

            return canvasGroup;
        }
    }

    [SerializeField]
    private GameObject group;

    public GameObject Group{
        get{
            if (group == null){
                group = gameObject;
            }

            return group;
        }
    }

    private Transform _transform;
    public Transform GetTransform{
        get {
            if (_transform==null){
                _transform = transform;
            }

            return _transform;
        }
    }

    public virtual void Start(){
        /*switch (state) {
            case State.visible:
                Show();
                break;
            case State.hidden:
                Hide();
                break;
            case State.none:

                break;
            default:
                break;
        }*/
    }

    public virtual void Show(){
        Group.SetActive(true);
    }    

    public virtual void Hide(){
        Group.SetActive(false);
    }

    public void FadeOut()
    {
        CanvasGroup.DOFade(0f, fade_duration);

        CancelInvoke("Hide");
        Invoke("Hide", fade_duration);
    }

    public void FadeInInstant()
    {
        Show();
        CanvasGroup.alpha = 0f;

        FadeInDelay();
    }

    public void FadeIn()
    {
        Show();
        CanvasGroup.alpha = 0f;


        CancelInvoke("FadeInDelay");
        Invoke("FadeInDelay", fade_duration);
    }

    void FadeInDelay()
    {
        CanvasGroup.DOFade(1f, fade_duration);
    }
}
