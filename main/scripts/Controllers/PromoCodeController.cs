using UnityEngine;
using TMPro;

public class PromoCodeController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _codeText;
    [SerializeField] private GameController _gameController;

    public void CheckCode()
    {
        string code = _codeText.text;
        string codeForGold = "xx0k01xf";
        string codeForLvlUp = "xt0k20hl";

        if (code == codeForGold)
        {
            _gameController.AddCash(5000);
        }
        if (code == codeForLvlUp)
        {
            _gameController._levelController.LevelUp(); 
        }
    }
}
