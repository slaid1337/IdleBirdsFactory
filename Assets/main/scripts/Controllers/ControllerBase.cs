using UnityEngine;
using UnityEngine.UI;

public abstract class ControllerBase : MonoBehaviour
{
    protected int _money = 0;
   public void ShowMoney(Text moneyText, int money)
    {
        moneyText.text = money.ToString();
    }

    public string ShowMoney(int money)
    {
        return money.ToString();
    }
}