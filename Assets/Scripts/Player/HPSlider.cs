using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    [SerializeField]
    private HPBar _hPBar;

    private Slider _slider;
    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue=_hPBar.MaxHp;
        _slider.value = _hPBar.CurrentHp;
        _hPBar.HPChanged+=OnHPChanged;
    }

    private void OnHPChanged(float newValue)
    {
        _slider.value = newValue;
    }
}
