using UnityEngine;

public class ModuleScript : MonoBehaviour
{
    [SerializeField]
    private ItemScript _tripletModule;

    private IModuleEffect _effect;

    public ItemScript TripletModule { get => _tripletModule; }

    private void Awake()
    {
        _effect=GetComponent<IModuleEffect>();
    }

    public void Activate(bool activate)
    {
        gameObject.SetActive(activate);
        _effect.ActivateEffect(activate);
    }
}
