public class Timer
{
    private float duration;
    private float elapsed;

    public Timer(float duration)
    {
        this.duration = duration;
        elapsed = 0f;
    }

    public float Value()
    {
        return elapsed / duration;
    }

    public float Value(float scale)
    {
        return scale * (elapsed / duration);
    }

    public bool Update(float secs)
    {
        if (elapsed == duration)
        {
            return true;
        }
        elapsed += secs;
        if (elapsed >= duration)
        {
            elapsed = duration;
            return true;
        }
        return false;
    }
}
