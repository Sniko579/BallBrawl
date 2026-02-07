using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StateMachine : MonoBehaviour
{
    public IState CurrentState { get; private set; }

    private Animator _animator;

    Dictionary<string, AnimationClip> _animationClipKeys = new Dictionary<string, AnimationClip>();
    [SerializeField] private AnimationClip HitGroundClip;
    [SerializeField] private AnimationClip JumpClip;
    


    private CapsuleCollider2D col;

    public void ChangeState(IState newState)
    {
        if (newState == CurrentState) return;

        CurrentState.OnExit(this);
        CurrentState = newState;
        CurrentState.OnEnter(this);


    }


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        CurrentState = new Idel();
        CurrentState.OnEnter(this);

        _animationClipKeys.Add("HitGroundAnim", HitGroundClip);
        _animationClipKeys.Add("JumpAnim", JumpClip);
        
    }
    private void Update()
    {
        CurrentState.OnUpdate(this);
    }



    public void PlayAnimation(string Name, float CorssFade)
    {
        _animator.CrossFade(Name, CorssFade);
        
    }


    public float GetAnimationLength(string animationName)
    {
        _animationClipKeys.TryGetValue(animationName, out AnimationClip value);
        return value.length;
    }
   


    public void ResetCollider()
    {

        Vector2 spriteSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        col = GetComponent<CapsuleCollider2D>();

        col.direction = spriteSize.x > spriteSize.y ? CapsuleDirection2D.Horizontal : CapsuleDirection2D.Vertical;

        col.size = spriteSize;
        col.offset = transform.InverseTransformPoint(GetComponent<SpriteRenderer>().bounds.center);
       
    }


}








public interface IState
{
    public void OnEnter(StateMachine state);
    public void OnUpdate(StateMachine state);
    public void OnExit(StateMachine state);
}
