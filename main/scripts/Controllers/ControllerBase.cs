using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public abstract class ControllerBase : MonoBehaviour
{
    protected BigInteger _money = new BigInteger();
    protected string[] _chars = { "", "K", "M", "B", "T", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "jj", "kk", "ll", "mm", "nn", "oo", "pp", "qq", "rr", "ss", "tt", "uu", "vv", "ww", "xx", "yy", "zz" };

    public void ShowMoney(Text moneyText , BigInteger Money)
    {
        int length = Money.ToString().Length;

        if (length > 3)
        {
            char[] money = Money.ToString().ToCharArray();
            if (length % 3 == 1)
            {
                string moneyString = money[0].ToString() + "." + money[1].ToString() + money[2].ToString() + _chars[length / 3];
                moneyText.text = moneyString;
            }
            else if (length % 3 == 2)
            {
                string moneyString = money[0].ToString() + money[1].ToString() + "." + money[2].ToString() + _chars[length / 3];
                moneyText.text = moneyString;
            }
            else
            {
                string moneyString = money[0].ToString() + money[1].ToString() + money[2].ToString() + _chars[(length / 3) - 1];
                moneyText.text = moneyString;
            }
        }
        else
        {
            moneyText.text = Money.ToString();
        }
    }

    public string ShowMoney(BigInteger Money)
    {
        int length = Money.ToString().Length;

        if (length > 3)
        {
            char[] money = Money.ToString().ToCharArray();
            if (length % 3 == 1)
            {
                string moneyString = money[0].ToString() + "." + money[1].ToString() + money[2].ToString() + _chars[length / 3];
                return moneyString;
            }
            else if (length % 3 == 2)
            {
                string moneyString = money[0].ToString() + money[1].ToString() + "." + money[2].ToString() + _chars[length / 3];
                return moneyString;
            }
            else
            {
                string moneyString = money[0].ToString() + money[1].ToString() + money[2].ToString() + _chars[(length / 3) - 1];
                return moneyString;
            }
        }
        else
        {
            return Money.ToString();
        }
    }
}
