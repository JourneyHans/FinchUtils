using System.Threading.Tasks;

namespace FinchUtils.Common;

public static class TimeUtils {
    public static Task DelaySeconds(float seconds) {
        return Task.Delay((int)(seconds * 1000));
    }
}