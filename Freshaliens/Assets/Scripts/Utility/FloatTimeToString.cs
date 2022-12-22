public static class FloatTimeToString
{
    public static string Convert(float time)
    {
        int secondsAsInt = (int)time;
        int minutes = secondsAsInt / 60;
        int remainingSeconds = secondsAsInt % 60;
        int centiseconds = (int)((time - secondsAsInt) * 100);
        string secondsAsString = (remainingSeconds < 10 ? "0" : "") + remainingSeconds.ToString();
        string centisecondsAsString = (centiseconds < 10 ? "0" : "") + centiseconds.ToString();
        return $"{minutes}:{secondsAsString}.{centisecondsAsString}";
    }

    public static float ParseString(string time) {
        string[] minSecs = time.Split(':');
        float mins = float.Parse(minSecs[0]);
        float secs = float.Parse(minSecs[1]);
        return mins * 60f + secs;
    }
}
