# Test
I created the test folder (project) with the following command.
```shell
dotnet new xunit -o test
```

Then I linked the test project to the solution with this command.
```shell
dotnet sln add ./test.csproj
```

I ran the following command to add the main project as a dependency of the test project.
```shell
dotnet add test/test.csproj reference ./sync-video-subtitle-name.csproj
```

And then dotnet failed to understand how it runs or builds the projects.
This is because I nested test project into the main project.
It gets the nested project as a part of the main project.
So it needs to ignore the nested project.
<https://github.com/dotnet/sdk/issues/14286>
I included the following snippet to the main `.csproj`.
```xml
<ItemGroup>
    <Compile Remove="test\**" />
    <Content Remove="test\**" />
    <EmbeddedResource Remove="test\**" />
    <None Remove="test\**" />
</ItemGroup>
```


<https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test>
