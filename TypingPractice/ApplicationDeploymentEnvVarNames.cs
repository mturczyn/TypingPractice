namespace TypingPractice;

public static class ApplicationDeploymentEnvVarNames
{
    public const string clickOncePrefix = "ClickOnce_";
    public const string isNetworkDeployedEnvVar = $"{clickOncePrefix}IsNetworkDeployed";
    public const string activationUriEnvVar = $"{clickOncePrefix}ActivationUri";
    public const string currentVersionEnvVar = $"{clickOncePrefix}CurrentVersion";
    public const string dataDirectoryEnvVar = $"{clickOncePrefix}DataDirectory";
    public const string isFirstRunEnvVar = $"{clickOncePrefix}IsFirstRun";
    public const string timeOfLastUpdateCheckEnvVar = $"{clickOncePrefix}TimeOfLastUpdateCheck";
    public const string updatedApplicationFullNameEnvVar = $"{clickOncePrefix}UpdatedApplicationFullName";
    public const string updatedVersionEnvVar = $"{clickOncePrefix}UpdatedVersion";
    public const string updateLocationEnvVar = $"{clickOncePrefix}UpdateLocation";
    public const string launcherVersionEnvVar = $"{clickOncePrefix}LauncherVersion";
}
