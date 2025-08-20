/*************************************************************************
 *  Copyright © 2025 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  ScriptDiemaker.cs
 *  Description  :  Diemaker for script header.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  2/12/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace MGS.Script.Diemaker
{
    public sealed class ScriptDiemaker : UnityEditor.AssetModificationProcessor
    {
        public const string USER_COMPANY = "DIEMAKER_USER_COMPANY";
        public const string USER_AUTHOR = "DIEMAKER_USER_AUTHOR";

        const string SCRIPT_EXTENSIONS = ".cs$|.js$|.boo$|.shader$|.compute$";
        const string META_EXTENSION = ".meta";

        const string HEADER_NAME = "Header.txt";
        const string COPYRIGHT_YEAR = "#COPYRIGHTYEAR#";
        const string COMPANY_NAME = "#COMPANYNAME#";
        const string SCRIPT_NAME = "#SCRIPTNAME#";
        const string AUTHOR_NAME = "#AUTHORNAME#";
        const string CREATE_DATE = "#CREATEDATE#";

        static string ResolveEditorDir()
        {
            var editorClass = $"{typeof(ScriptDiemaker).Name}.cs";
            var editorPath = AssetDatabase.GetAllAssetPaths().First(path => { return path.Contains(editorClass); });
            return editorPath.Replace(editorClass, string.Empty);
        }

        static void OnWillCreateAsset(string metaPath)
        {
            var assetPath = metaPath.Replace(META_EXTENSION, string.Empty);
            if (Regex.IsMatch(Path.GetExtension(assetPath), SCRIPT_EXTENSIONS))
            {
                try
                {
                    var content = File.ReadAllText(assetPath);
                    if (!content.Contains("Copyright"))
                    {
                        var headerPath = $"{ResolveEditorDir()}/{HEADER_NAME}";
                        var header = File.ReadAllText(headerPath);

                        header = header.Replace(COPYRIGHT_YEAR, DateTime.Now.Year.ToString());
                        header = header.Replace(COMPANY_NAME, EditorPrefs.GetString(USER_COMPANY));
                        header = header.Replace(SCRIPT_NAME, Path.GetFileName(assetPath));
                        header = header.Replace(AUTHOR_NAME, EditorPrefs.GetString(USER_AUTHOR));
                        header = header.Replace(CREATE_DATE, DateTime.Now.ToShortDateString());

                        File.WriteAllText(assetPath, $"{header}\r\n{content}");
                        AssetDatabase.Refresh();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
    }
}