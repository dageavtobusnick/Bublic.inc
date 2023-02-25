using UnityEngine;
using UnityEngine.UI;

public class CoinScore : MonoBehaviour
{
    [SerializeField] 
    private Text _score;
    [SerializeField]
    private int _coinCount;

    public int CoinCount { get => _coinCount;}

    private void Start()
    {
        _coinCount = 0;
        var player = GameController.Player;
        player.OnCoinTake += ChangeScore;
        _score.text = _coinCount.ToString();
     }

    public void ChangeScore()
    {
        _coinCount++;
        _score.text = _coinCount.ToString(); 
    }

    public void SpendCoins(int coins)
    {
        _coinCount -= coins;
        _score.text = _coinCount.ToString();
    }
}


