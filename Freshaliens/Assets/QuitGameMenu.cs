using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.UI
{
    public class QuitGameMenu : UIScreen
    {
        public void OnBackPressed()
        {
            TitleScreenManager.Instance.ResetMenuView();
        }
    }
}