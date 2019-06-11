/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

public static class KinectCopyPluginDataHelper
{
    private const string DataDirSuffix = "_Data";
    private const string PluginsDirName = "Plugins";

    private static Dictionary<BuildTarget, string> TargetToDirName = new Dictionary<BuildTarget, string>()
        {
            {BuildTarget.StandaloneWindows, "x86"},
            {BuildTarget.StandaloneWindows64, "x86_64"}
        };

    public static void CopyPluginData(BuildTarget target, string buildTargetPath, string subDirToCopy)
    {
        string subDirName;
        if (!TargetToDirName.TryGetValue (target, out subDirName))
        {
            // No work to do
            return;
        }

        // Get Required Paths
        var buildName = Path.GetFileNameWithoutExtension(buildTargetPath);
        var targetDir = Directory.GetParent(buildTargetPath);
        var separator = Path.DirectorySeparatorChar;

        var buildDataDir = targetDir.FullName + separator + buildName + DataDirSuffix + separator;
        var tgtPluginsDir = buildDataDir + separator + PluginsDirName + separator + subDirToCopy + separator;
        var srcPluginsDir = Application.dataPath + separator + PluginsDirName + separator + subDirName + separator + subDirToCopy + separator;

        CopyAll (new DirectoryInfo (srcPluginsDir), new DirectoryInfo(tgtPluginsDir));
    }

    /// <summary>
    /// Recursive Copy Directory Method
    /// </summary>
    private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        // Check if the source directory exists, if not, don't do any work.
        if (!Directory.Exists(source.FullName))
        {
            return;
        }

        // Check if the target directory exists, if not, create it.
        if (!Directory.Exists(target.FullName))
        {
            Directory.CreateDirectory(target.FullName);
        }

        // Copy each file into it’s new directory.
        foreach (var fileInfo in source.GetFiles())
        {
            fileInfo.CopyTo (Path.Combine (target.ToString (), fileInfo.Name), true);
        }
        
        // Copy each subdirectory using recursion.
        foreach (var subDirInfo in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(subDirInfo.Name);
            CopyAll(subDirInfo, nextTargetSubDir);
        }
    }

}
