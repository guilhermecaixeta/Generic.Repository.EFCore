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
.Does(() =>
{
    foreach (var project in GetFiles("./src/**/*.csproj"))
    {
        DotNetCorePack(
            project.GetDirectory().FullPath,
            new DotNetCorePackSettings()
            {
                Configuration = configuration,
                OutputDirectory = artifactDirectory
            });
    }
});

Task("Push-Nuget-Package")
.IsDependentOn("Create-Nuget-Package")
.Does(() =>
{
    var apiKey = "oy2pixvzsxxiu7hyftiqrbis4ezwf5duvavmnqcev7rvre"; //EnvironmentVariable("apiKey");
    
    foreach (var package in GetFiles($"{artifactsDirectory}/*.nupkg"))
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
RunTarget(target).IsDependentOn("Push-Nuget-Package");
/* END - RUN */