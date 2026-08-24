param(
    [ValidateSet('Debug', 'Staging', 'Release')]
    [string]$Configuration = 'Release',

    [string]$ArchiveRuntimeIdentifier = '',

    [switch]$ArchiveSelfContained
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$IsWindowsPlatform = [System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform(
    [System.Runtime.InteropServices.OSPlatform]::Windows
)
$IsLinuxPlatform = [System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform(
    [System.Runtime.InteropServices.OSPlatform]::Linux
)
$IsMacOSPlatform = [System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform(
    [System.Runtime.InteropServices.OSPlatform]::OSX
)

function Get-ProjectProperty {
    param(
        [Parameter(Mandatory = $true)]
        [xml]$Project,

        [Parameter(Mandatory = $true)]
        [string]$Name
    )

    foreach ($group in $Project.Project.PropertyGroup) {
        $value = $group.$Name
        if ($null -ne $value -and 0 -lt "$value".Length) {
            return "$value"
        }
    }

    throw "Project property '$Name' was not found."
}

function Invoke-DotNet {
    param(
        [Parameter(Mandatory = $true)]
        [string[]]$Arguments
    )

    Write-Host "> dotnet $($Arguments -join ' ')"
    & dotnet @Arguments
    if (0 -ne $LASTEXITCODE) {
        throw "dotnet exited with status $LASTEXITCODE."
    }
}

function Get-ToolShimPath {
    param(
        [Parameter(Mandatory = $true)]
        [string]$ToolPath,

        [Parameter(Mandatory = $true)]
        [string]$CommandName
    )

    $fileName = if ($IsWindowsPlatform) {
        "$CommandName.exe"
    } else {
        $CommandName
    }

    $path = Join-Path $ToolPath $fileName
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
        throw "Expected tool shim '$path' was not created."
    }

    return $path
}

function Invoke-Tool {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path,

        [string[]]$Arguments = @(),

        [int]$ExpectedExitCode = 0
    )

    Write-Host "> $Path $($Arguments -join ' ')"
    & $Path @Arguments
    if ($ExpectedExitCode -ne $LASTEXITCODE) {
        throw "Tool '$Path' exited with status $LASTEXITCODE; expected $ExpectedExitCode."
    }
}

function Get-CurrentRuntimeIdentifier {
    $architecture = switch ($env:RUNNER_ARCH) {
        'ARM64' { 'arm64' }
        'X64' { 'x64' }
        default {
            $runtimeArchitecture = [System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture
            if ([System.Runtime.InteropServices.Architecture]::Arm64 -eq $runtimeArchitecture) {
                'arm64'
            } elseif ([System.Runtime.InteropServices.Architecture]::X64 -eq $runtimeArchitecture) {
                'x64'
            } else {
                throw "Unsupported architecture '$runtimeArchitecture'."
            }
        }
    }

    if ($IsWindowsPlatform) {
        return "win-$architecture"
    }
    if ($IsLinuxPlatform) {
        return "linux-$architecture"
    }
    if ($IsMacOSPlatform) {
        return "osx-$architecture"
    }

    throw 'Unsupported operating system for archive verification.'
}

function Read-ToolSettingsFromPackage {
    param(
        [Parameter(Mandatory = $true)]
        [string]$PackagePath,

        [Parameter(Mandatory = $true)]
        [string]$TargetFramework
    )

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $archive = [System.IO.Compression.ZipFile]::OpenRead($PackagePath)
    try {
        $settingsPath = "tools/$TargetFramework/any/DotnetToolSettings.xml"
        $entry = $archive.Entries | Where-Object { $_.FullName -eq $settingsPath } | Select-Object -First 1
        if ($null -eq $entry) {
            throw "Package '$PackagePath' does not contain '$settingsPath'."
        }

        $reader = [System.IO.StreamReader]::new($entry.Open())
        try {
            [xml]$settings = $reader.ReadToEnd()
        } finally {
            $reader.Dispose()
        }

        $commands = @($settings.DotNetCliTool.Commands.Command)
        if (0 -eq $commands.Count) {
            throw "Package '$PackagePath' declares no .NET tool commands."
        }

        return @{
            Archive = $archive
            Commands = $commands
        }
    } catch {
        $archive.Dispose()
        throw
    }
}

function Assert-PackageCommands {
    param(
        [Parameter(Mandatory = $true)]
        [string]$PackagePath,

        [Parameter(Mandatory = $true)]
        [string]$TargetFramework,

        [Parameter(Mandatory = $true)]
        [string[]]$ExpectedCommands,

        [Parameter(Mandatory = $true)]
        [string[]]$RequiredAssemblies
    )

    $result = Read-ToolSettingsFromPackage -PackagePath $PackagePath -TargetFramework $TargetFramework
    $archive = $result.Archive
    try {
        $actualCommands = @($result.Commands | ForEach-Object { "$($_.Name)" } | Sort-Object)
        $expected = @($ExpectedCommands | Sort-Object)
        if (($actualCommands -join "`n") -ne ($expected -join "`n")) {
            throw "Package '$PackagePath' commands were '$($actualCommands -join ', ')'; expected '$($expected -join ', ')'."
        }

        foreach ($command in $result.Commands) {
            if ('dotnet' -ne "$($command.Runner)") {
                throw "Command '$($command.Name)' in '$PackagePath' uses unexpected runner '$($command.Runner)'."
            }
        }

        foreach ($assembly in $RequiredAssemblies) {
            $entryPath = "tools/$TargetFramework/any/$assembly"
            if (-not ($archive.Entries | Where-Object { $_.FullName -eq $entryPath } | Select-Object -First 1)) {
                throw "Package '$PackagePath' does not contain '$entryPath'."
            }
        }
    } finally {
        $archive.Dispose()
    }
}

function Write-LocalNuGetConfig {
    param(
        [Parameter(Mandatory = $true)]
        [string]$PackageDirectory,

        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    $escapedPath = [System.Security.SecurityElement]::Escape($PackageDirectory)
    $content = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="local" value="$escapedPath" />
  </packageSources>
</configuration>
"@

    [System.IO.File]::WriteAllText($Path, $content, [System.Text.UTF8Encoding]::new($false))
}

$repositoryRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$routerProjectPath = Join-Path $repositoryRoot 'diffutil/Icod.DiffUtils.DiffUtil.csproj'
$packagingProjectPath = Join-Path $repositoryRoot 'packaging/Icod.DiffUtils.Packaging.csproj'
$solutionPath = Join-Path $repositoryRoot 'Icod.DiffUtils.sln'

[xml]$routerProject = Get-Content -LiteralPath $routerProjectPath -Raw
[xml]$packagingProject = Get-Content -LiteralPath $packagingProjectPath -Raw

$targetFramework = Get-ProjectProperty -Project $routerProject -Name 'TargetFramework'
$routerPackageId = Get-ProjectProperty -Project $routerProject -Name 'PackageId'
$routerVersion = Get-ProjectProperty -Project $routerProject -Name 'PackageVersion'
$aggregatePackageId = Get-ProjectProperty -Project $packagingProject -Name 'PackageId'
$aggregateVersion = Get-ProjectProperty -Project $packagingProject -Name 'PackageVersion'

if ($routerVersion -ne $aggregateVersion) {
    throw "Router package version '$routerVersion' does not match aggregate package version '$aggregateVersion'."
}

$validationRoot = Join-Path $repositoryRoot 'artifacts/distribution-validation'
$packageDirectory = Join-Path $validationRoot 'packages'
$routerToolPath = Join-Path $validationRoot 'router-tool'
$aggregateToolPath = Join-Path $validationRoot 'aggregate-tool'
$archiveExtractPath = Join-Path $validationRoot 'archive'
$nugetConfigPath = Join-Path $validationRoot 'NuGet.Config'

if (Test-Path -LiteralPath $validationRoot) {
    Remove-Item -LiteralPath $validationRoot -Recurse -Force
}
New-Item -ItemType Directory -Path $packageDirectory -Force | Out-Null

Push-Location $repositoryRoot
try {
    Invoke-DotNet -Arguments @('restore', $solutionPath)
    Invoke-DotNet -Arguments @(
        'build',
        $solutionPath,
        '-c', $Configuration,
        '--no-restore',
        '-p:ContinuousIntegrationBuild=true'
    )
    Invoke-DotNet -Arguments @(
        'test',
        $solutionPath,
        '-c', $Configuration,
        '--no-build',
        '--logger', 'trx'
    )

    Invoke-DotNet -Arguments @(
        'pack',
        $routerProjectPath,
        '-c', $Configuration,
        '-o', $packageDirectory
    )
    Invoke-DotNet -Arguments @(
        'pack',
        $packagingProjectPath,
        '-c', $Configuration,
        '-o', $packageDirectory
    )

    $routerPackagePath = Join-Path $packageDirectory "$routerPackageId.$routerVersion.nupkg"
    $aggregatePackagePath = Join-Path $packageDirectory "$aggregatePackageId.$aggregateVersion.nupkg"
    if (-not (Test-Path -LiteralPath $routerPackagePath -PathType Leaf)) {
        throw "Router package '$routerPackagePath' was not produced."
    }
    if (-not (Test-Path -LiteralPath $aggregatePackagePath -PathType Leaf)) {
        throw "Aggregate package '$aggregatePackagePath' was not produced."
    }

    $commandAssemblies = @('diffutil.dll', 'cmp.dll', 'diff.dll', 'diff3.dll', 'sdiff.dll')
    Assert-PackageCommands `
        -PackagePath $routerPackagePath `
        -TargetFramework $targetFramework `
        -ExpectedCommands @('diffutil') `
        -RequiredAssemblies $commandAssemblies
    Assert-PackageCommands `
        -PackagePath $aggregatePackagePath `
        -TargetFramework $targetFramework `
        -ExpectedCommands @('diffutil', 'cmp', 'diff', 'diff3', 'sdiff') `
        -RequiredAssemblies $commandAssemblies

    Write-LocalNuGetConfig -PackageDirectory $packageDirectory -Path $nugetConfigPath

    Invoke-DotNet -Arguments @(
        'tool', 'install', $routerPackageId,
        '--version', $routerVersion,
        '--tool-path', $routerToolPath,
        '--configfile', $nugetConfigPath
    )

    $routerShim = Get-ToolShimPath -ToolPath $routerToolPath -CommandName 'diffutil'
    Invoke-Tool -Path $routerShim -Arguments @('--version')
    foreach ($commandName in @('cmp', 'diff', 'diff3', 'sdiff')) {
        Invoke-Tool -Path $routerShim -Arguments @($commandName, '--version')
    }

    Invoke-DotNet -Arguments @(
        'tool', 'install', $aggregatePackageId,
        '--version', $aggregateVersion,
        '--tool-path', $aggregateToolPath,
        '--configfile', $nugetConfigPath
    )

    foreach ($commandName in @('diffutil', 'cmp', 'diff', 'diff3', 'sdiff')) {
        $shim = Get-ToolShimPath -ToolPath $aggregateToolPath -CommandName $commandName
        Invoke-Tool -Path $shim -Arguments @('--version')
    }

    $rid = if (0 -lt $ArchiveRuntimeIdentifier.Length) {
        $ArchiveRuntimeIdentifier
    } else {
        Get-CurrentRuntimeIdentifier
    }
    $selfContainedValue = if ($ArchiveSelfContained) { 'true' } else { 'false' }

    Invoke-DotNet -Arguments @(
        'msbuild',
        $packagingProjectPath,
        '-t:CreateArchive',
        "-p:Configuration=$Configuration",
        "-p:ArchiveRuntimeIdentifier=$rid",
        "-p:ArchiveSelfContained=$selfContainedValue"
    )

    $archivePath = Join-Path $repositoryRoot "artifacts/$Configuration/Icod.DiffUtils-$aggregateVersion-$rid.zip"
    if (-not (Test-Path -LiteralPath $archivePath -PathType Leaf)) {
        throw "Archive '$archivePath' was not produced."
    }

    if (Test-Path -LiteralPath $archiveExtractPath) {
        Remove-Item -LiteralPath $archiveExtractPath -Recurse -Force
    }
    Expand-Archive -LiteralPath $archivePath -DestinationPath $archiveExtractPath -Force

    foreach ($commandName in @('diffutil', 'cmp', 'diff', 'diff3', 'sdiff')) {
        $archiveCommandPath = Get-ToolShimPath -ToolPath $archiveExtractPath -CommandName $commandName
        if (-not $IsWindowsPlatform) {
            & chmod +x $archiveCommandPath
            if (0 -ne $LASTEXITCODE) {
                throw "chmod failed for '$archiveCommandPath'."
            }
        }
        Invoke-Tool -Path $archiveCommandPath -Arguments @('--version')
    }

    Write-Host ''
    Write-Host 'Distribution verification completed successfully.'
    Write-Host "  Router package:    $routerPackagePath"
    Write-Host "  Aggregate package: $aggregatePackagePath"
    Write-Host "  Archive:           $archivePath"
} finally {
    Pop-Location
}
