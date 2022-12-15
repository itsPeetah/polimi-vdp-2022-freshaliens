using Freshaliens.Management;
namespace Freshaliens.UI
{
    public class LevelFailedHUD : UIScreen
    {
        public void OnGoBakcPressed()
        {
            LevelManager.Instance.QuitLevel();
        }

        public void OnRestartPress()
        {
            LevelManager.Instance.ReloadLevel();
        }
    }
}