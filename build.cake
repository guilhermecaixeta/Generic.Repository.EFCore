// Install modules
#module nuget:?package=Cake.DotNetTool.Module&version=0.3.0
// Install Tools
#tool "nuget:?package=GitVersion.CommandLine"
// Install Addin's
#addin nuget:?package=Newtonsoft.Json

using Newtonsoft.Json;

/* BEGIN - Parameters */
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactDirectory = MakeAbsolute(Directory("./artifacts"));
/* END - Parameters */

/* BEGIN - Setup */
Setup(context =>
{
    CleanDirectory(artifactDirectory);
    CleanDirectories("./src/**/obj");
});
/* END */

/* BEGIN - Tasks */
Task("Default")
.IsDependentOn("Push-Nuget-Package");

Task("Build").
    Does(() => 
    {
        foreach (var project in GetFiles("./src/Generic.Repository.EFCore.sln"))
        {
            DotNetCoreBuild(
                project.GetDirectory().FullPath,
                new DotNetCoreBuildSettings()
                {
                    Configuration = configuration
                });
        }      
    });

Task("Test")
.IsDependentOn("Build")
.Does(() =>
{
    foreach(var project in GetFiles("./tests/**/*.csproj"))
    {
        DotNetCoreTest(
            project.GetDirectory().FullPath,
            new DotNetCoreTestSettings()
            {
                Configuration = configuration
            });
    }
});

Task("Create-Nuget-Pack")
.IsDependentOn("Test")
.WithCriteria(ShouldRunRelease())
.Does(() =>
{
    var version = GetPackageVersion();

    foreach (var project in GetFiles("./src/**/*.csproj"))
    {
        DotNetCorePack(
            project.GetDirectory().FullPath,
            new DotNetCorePackSettings()
            {
                Configuration = configuration,
                OutputDirectory = artifactDirectory,
                ArgumentCustomization= args => args.Append($"/p:Version={version}")
            });
    }
});

Task("Push-Nuget-Package")
.IsDependentOn("Create-Nuget-Pack")
.WithCriteria(ShouldRunRelease())
.Does(() =>
{
    var apiKey = EnvironmentVariable("NUGET_API_KEY");
    
    foreach (var package in GetFiles($"{artifactDirectory}/*.nupkg"))
    {
        NuGetPush(package, 
            new NuGetPushSettings {
                Source = "https://www.nuget.org/api/v2/package",
                ApiKey = apiKey
            });
    }
});
/* END - Tasks */

/* BEGIN - RUN */
RunTarget(target);
/* END - RUN */

/* BEGIN - METHODS */
private bool ShouldRunRelease() => AppVeyor.IsRunningOnAppVeyor && AppVeyor.Environment.Repository.Tag.IsTag;

private string GetPackageVersion()
{
    var gitVersion = GitVersion(new GitVersionSettings {
        RepositoryPath = "."
    });

    Information($"Git Semantic Version: {JsonConvert.SerializeObject(gitVersion)}");
    
    return gitVersion.NuGetVersionV2;
}
/* END */