using System;
using System.Runtime.InteropServices;

namespace SleepyBear;

// Controls the system's sleep behavior using native OS calls.
//
// This implementation targets Windows and uses the Win32 API
// to signal that the system should remain awake without simulating input.
public sealed class SleepController
{
    // Flags used by the Windows API method SetThreadExecutionState.
    // These define how the system should behave regarding sleep and display.

    [Flags]
    private enum ExecutionState : uint
    {
        // Enables away mode (used for media applications). Not used currently.
        ES_AWAYMODE_REQUIRED = 0x00000040,

        // Indicates that the state should remain in effect until commanded to change.
        // Without this, the request would reset after a short time.
        ES_CONTINUOUS = 0x80000000,

        // Prevents the display from turning off.
        // (Not used in current version to avoid being intrusive.)
        ES_DISPLAY_REQUIRED = 0x00000002,

        // Prevents the system from entering sleep mode.
        // This is the primary flag we use.
        ES_SYSTEM_REQUIRED = 0x00000001
    }

    // Native Windows API call to control execution state.
    // 
    // Source: kernel32.dll
    // 
    // This function allows applications to inform Windows that
    // the system or display should remain active.
    // </summary>
    // <param name="esFlags">Combination of ExecutionState flags</param>
    // <returns>The previous execution state</returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

    // Enables keep-awake mode.
    // 
    // Prevents the system from going to sleep while this state is active.
    // Does NOT prevent the display from turning off (by design).

    public void EnableKeepAwake()
    {
        // Safety check: this implementation only works on Windows
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        // Request Windows to keep the system awake continuously
        SetThreadExecutionState(
            ExecutionState.ES_CONTINUOUS |
            ExecutionState.ES_SYSTEM_REQUIRED);
    }

    // Disables keep-awake mode.
    // 
    // Restores default system behavior, allowing normal sleep policies.
    public void DisableKeepAwake()
    {
        // Safety check: this implementation only works on Windows
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        // Reset execution state back to default behavior
        // (ES_CONTINUOUS alone clears previous flags)
        SetThreadExecutionState(ExecutionState.ES_CONTINUOUS);
    }
}