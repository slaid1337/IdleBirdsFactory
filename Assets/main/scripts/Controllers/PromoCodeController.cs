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
        string codeForReset = "2reset2all2";

        if (code == codeForGold)
        {
            _gameController.AddCash(5000);
        }
        else if (code == codeForLvlUp)
        {
            _gameController._levelController.LevelUp(); 
        }
        else if (code == codeForReset)
        {
            _gameController._saveController.ResetAllSaves();
        }
    }
}