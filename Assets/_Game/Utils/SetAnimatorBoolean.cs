using UnityEngine;

public class SetAnimatorBoolean : MonoBehaviour
{
    public string Name;

    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Set(bool value)
    {
        _animator.SetBool(Name, value);
    }
}
