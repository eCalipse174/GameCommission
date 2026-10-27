// Presentation strategy for the sleep state.
// Swap implementations per character (animator bool, ScriptableObject clip set, etc).
public interface ISleepPresenter
{
    void PlaySleep();
    void PlayWake();
}