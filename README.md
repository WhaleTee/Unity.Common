Below will be described installation instrucitons.<br/>
If an instruction doesn't contain any steps, but URL only, it means that package must be installed as GIT dependency.<br/>

### How to install GIT depencendy via Package Manager? </summary>

<details>

<summary> Unity 2019.3 or newer </summary>

1. Open Package Manager window (Window | Package Manager)
1. Click `+` button on the upper-left of a window, and select "Add package from git URL..."
1. Enter the URL and click `Add` button

> **_NOTE:_** To install a concrete version you can specify the version by prepending #v{version} e.g. `#v2.0.0`. For more see [Unity UPM Documentation](https://docs.unity3d.com/Manual/upm-git.html).

</details>

#### Workspace Setup

```
https://github.com/WhaleTee/Unity.Common.git?path=/Tools/Setup
```

This package can import Flare Engine for 2D purposes, but Flare contains known bugs.</br>
This is how to fix it:
1. Import settings override only if it's your first installation.
2. Accept any automatic updates for API changes.
3. Unity made some changes to their API, so we will need to change some names. Click on the errors in the console to open the scripts, or open the scripts manually.
4. Change NativeMultiHasMap to NativeParallelMultiHashMap for:
    - Pathfinding.cs, lines 34, 159
    - PathfindingRT.cs, lines 40, 57
    - PathfindingJob.cs, line 12
    - PathfindingJobRT.cs, line 12
5. Change jobGrid.Count() to jobGrid.Count for:
    - PathfindingJobRT.cs, line 37
6. Save changes, go into unity to trigger a recompile. You might need to press CTRL + R.

#### R3 for Unity

1. Install Setup tools as described below
1. Install Essential Packages (Tools | Setup | Install Essential Packages)
1. Wait for the end of installation
1. Install R3 Package (Tools | Setup | Install R3 Package)
