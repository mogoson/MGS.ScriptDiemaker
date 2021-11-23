/*************************************************************************
 *  Copyright © 2021 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  ScriptTemplateEditor.cs
 *  Description  :  Editor for script template.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  2/12/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MGS.ScriptTemplate.Editors
{
    public class ScriptTemplateEditor : EditorWindow
    {
        #region Field and Property
        private static ScriptTemplateEditor instance;
        private const float BUTTON_WIDTH = 60;
        private Vector2 scrollPosition = Vector2.zero;

        private readonly string TemplatesDirectory = EditorApplication.applicationContentsPath + "/Resources/ScriptTemplates";
        private string[] templateFiles = { };
        private string templateText = string.Empty;
        private int templateIndex = 0;
        #endregion

        #region Private Method
        [MenuItem("Tool/Template Editor &T")]
        private static void ShowEditor()
        {
            instance = GetWindow<ScriptTemplateEditor>("TemplateEditor");
            instance.Show();
        }

        private void OnEnable()
        {
            templateFiles = FindScriptTemplateFiles();
            templateText = ReadScriptTemplateText(templateFiles[templateIndex]);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical("Window");

            EditorGUI.BeginChangeCheck();
            templateIndex = EditorGUILayout.Popup("Template", templateIndex, templateFiles);
            if (EditorGUI.EndChangeCheck())
            {
                scrollPosition = Vector2.zero;
                templateText = ReadScriptTemplateText(templateFiles[templateIndex]);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Content");
            EditorGUILayout.Space();
            if (GUILayout.Button("Save", GUILayout.Width(BUTTON_WIDTH)))
            {
                SaveScriptTemplate(templateFiles[templateIndex], templateText);
            }
            EditorGUILayout.EndHorizontal();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            templateText = EditorGUILayout.TextArea(templateText, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        private string[] FindScriptTemplateFiles()
        {
            try
            {
                var searchFiles = Directory.GetFiles(TemplatesDirectory, "*.txt", SearchOption.AllDirectories);
                var templateFiles = new string[searchFiles.Length];
                for (int i = 0; i < searchFiles.Length; i++)
                {
                    templateFiles[i] = Path.GetFileNameWithoutExtension(searchFiles[i]);
                }
                return templateFiles;
            }
            catch (Exception e)
            {
                ShowNotification(new GUIContent(e.Message));
                return new string[0];
            }
        }

        private string ReadScriptTemplateText(string templateName)
        {
            try
            {
                var templatePath = GetScriptTemplatePath(templateName);
                return File.ReadAllText(templatePath);
            }
            catch (Exception e)
            {
                ShowNotification(new GUIContent(e.Message));
                return string.Empty;
            }
        }

        private void SaveScriptTemplate(string templateName, string templateText)
        {
            try
            {
                var templatePath = GetScriptTemplatePath(templateName);
                File.WriteAllText(templatePath, templateText);
                ShowNotification(new GUIContent("The script template is saved."));
            }
            catch (Exception e)
            {
                ShowNotification(new GUIContent(e.Message));
            }
        }

        private string GetScriptTemplatePath(string templateName)
        {
            return string.Format("{0}/{1}.txt", TemplatesDirectory, templateName);
        }
        #endregion
    }
}