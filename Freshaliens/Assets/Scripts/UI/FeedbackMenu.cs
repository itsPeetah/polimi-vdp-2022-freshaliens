using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.UI
{
    public class FeedbackMenu : UIScreen
    {
        public void OnBackPressed()
        {
            TitleScreenManager.Instance.ResetMenuView();
        }
    }
}