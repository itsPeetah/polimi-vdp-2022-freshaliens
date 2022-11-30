using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement
{
    public class FeedBackScreen : Menu<FeedBackScreen>
    {
        /*[SerializeField]
        private SendToGoogle _sendToGoogle;*/
        public override void OnBackPressed()
        {
            base.OnBackPressed();
                
            // custom behavior ,..
        }

        /*public void OnSubmitPressed()
        {
            _sendToGoogle.SendFeedback();
        }*/
    }
}