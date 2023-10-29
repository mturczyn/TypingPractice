using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

/// <summary>
/// Copied from DotNet Mage package:
/// https://github.com/dotnet/deployment-tools/blob/main/Documentation/dotnet-mage/ApplicationDeployment.cs#L3
/// </summary>
public class ApplicationDeployment
{

    private static ApplicationDeployment currentDeployment = null;
    private static bool currentDeploymentInitialized = false;

    private static bool isNetworkDeployed = false;
    private static bool isNetworkDeployedInitialized = false;

    public static bool IsNetworkDeployed
    {
        get
        {
            if (!isNetworkDeployedInitialized)
            {
                bool.TryParse(Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.isNetworkDeployedEnvVar), out ApplicationDeployment.isNetworkDeployed);
                ApplicationDeployment.isNetworkDeployedInitialized = true;
            }

            return ApplicationDeployment.isNetworkDeployed;
        }
    }

    public static ApplicationDeployment CurrentDeployment
    {
        get
        {
            if (!currentDeploymentInitialized)
            {
                ApplicationDeployment.currentDeployment = IsNetworkDeployed ? new ApplicationDeployment() : null;
                ApplicationDeployment.currentDeploymentInitialized = true;
            }

            return ApplicationDeployment.currentDeployment;
        }
    }

    public static Uri ActivationUri
    {
        get
        {
            Uri.TryCreate(Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.activationUriEnvVar), UriKind.Absolute, out Uri val);
            return val;
        }
    }

    public static Version CurrentVersion
    {
        get
        {
            Version.TryParse(Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.currentVersionEnvVar), out Version val);
            return val;
        }
    }
    public static string DataDirectory
    {
        get { return Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.dataDirectoryEnvVar); }
    }

    public static bool IsFirstRun
    {
        get
        {
            bool.TryParse(Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.isFirstRunEnvVar), out bool val);
            return val;
        }
    }

    public static DateTime TimeOfLastUpdateCheck
    {
        get
        {
            DateTime.TryParse(Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.timeOfLastUpdateCheckEnvVar), out DateTime value);
            return value;
        }
    }
    public static string UpdatedApplicationFullName
    {
        get
        {
            return Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.updatedApplicationFullNameEnvVar);
        }
    }

    public static Version UpdatedVersion
    {
        get
        {
            Version.TryParse(Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.updatedVersionEnvVar), out Version val);
            return val;
        }
    }

    public static Uri UpdateLocation
    {
        get
        {
            Uri.TryCreate(Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.updateLocationEnvVar), UriKind.Absolute, out Uri val);
            return val;
        }
    }

    public static Version LauncherVersion
    {
        get
        {

            Version.TryParse(Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.launcherVersionEnvVar), out Version val);
            return val;
        }
    }

    private ApplicationDeployment()
    {
        // As an alternative solution, we could initialize all properties here
    }
}
