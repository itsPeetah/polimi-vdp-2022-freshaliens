using Freshaliens.Management;
namespace Freshaliens.UI
{
    public class LevelFailedHUD : LevelUIScreen
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