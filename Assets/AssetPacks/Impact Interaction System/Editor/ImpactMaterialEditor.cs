﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Impact.Materials;
using Impact.TagLibrary;
using Impact.Interactions;
using System;

namespace Impact.EditorScripts
{
    [CustomEditor(typeof(ImpactMaterial))]
    public class ImpactMaterialEditor : Editor
    {
        private readonly Color warningColor = new Color(1, 1, 0.5f);

        private ImpactMaterial material;

        private SerializedProperty tagLibraryProperty;
        private SerializedProperty materialTagsMaskProperty;

        private SerializedProperty interactionSetsProperty;

        private SerializedProperty fallbackTagsMaskProperty;

        private ImpactTagNameList tagNames;
        private List<bool> interactionFoldouts = new List<bool>();

        private void OnEnable()
        {
            material = target as ImpactMaterial;

            tagLibraryProperty = serializedObject.FindProperty("_tagLibrary");
            materialTagsMaskProperty = serializedObject.FindProperty("_materialTagsMask");

            interactionSetsProperty = serializedObject.FindProperty("_interactionSets");

            fallbackTagsMaskProperty = serializedObject.FindProperty("_fallbackTagMask");

            tagNames = new ImpactTagNameList(ImpactTagLibraryConstants.TagCount);
            updateTagNames();

            for (int i = 0; i < material.InteractionSetCount; i++)
            {
                interactionFoldouts.Add(false);
            }
        }

        private void updateTagNames()
        {
            ImpactEditorUtilities.GetTagNames(material.TagLibrary, ref tagNames);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(tagLibraryProperty, new GUIContent("Tag Library", "The Tag Library to use for displaying tag names."));
            updateTagNames();

            if (material.TagLibrary == null)
            {
                EditorGUILayout.HelpBox(ImpactTagLibraryConstants.TagLibraryNotFoundErrorMessage, MessageType.Info);
            }

            ImpactEditorUtilities.TagMaskPropertyEditor(materialTagsMaskProperty, new GUIContent("Material Tags", "The tags that apply to this material."), tagNames);

            ImpactEditorUtilities.Separator();

            drawInteractionsList();

            serializedObject.ApplyModifiedProperties();
        }

        private void drawInteractionsList()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Interaction Sets", EditorStyles.boldLabel);

            if (interactionFoldouts.Count < interactionSetsProperty.arraySize)
            {
                int diff = interactionSetsProperty.arraySize - interactionFoldouts.Count;
                for (int i = 0; i < diff; i++)
                {
                    interactionFoldouts.Add(false);
                }
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("New"))
            {
                Undo.RecordObject(target, "Add Interaction Set");

                ImpactMaterialInteractionSet newInteractionSet = new ImpactMaterialInteractionSet();
                newInteractionSet.Name = "New Interaction Set";
                material.AddInteractionSet(newInteractionSet);
                interactionFoldouts.Add(true);

                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Clear"))
            {
                if (EditorUtility.DisplayDialog("Clear All Interaction Sets", "Are you sure you want to remove all Interaction Sets?", "Yes", "No"))
                {
                    interactionSetsProperty.ClearArray();
                }
            }

            EditorGUILayout.EndHorizontal();

            if (interactionSetsProperty.arraySize == 0)
            {
                EditorGUILayout.HelpBox("No Interaction Sets have been added.", MessageType.Info);
            }
            else
            {
                EditorGUILayout.Separator();

                for (int i = 0; i < Mathf.Min(interactionSetsProperty.arraySize, material.InteractionSetCount); i++)
                {
                    drawInteractionSet(interactionSetsProperty.GetArrayElementAtIndex(i), material[i], i);
                }

                EditorGUILayout.Separator();

                ImpactEditorUtilities.TagMaskPropertyEditor(fallbackTagsMaskProperty, new GUIContent("Fallback Tags", "Tags that will be used if the input tags are unknown."), tagNames);
            }
        }

        private void drawInteractionSet(SerializedProperty interactionSetProperty, ImpactMaterialInteractionSet interactionSet, int index)
        {
            bool removed = false;

            EditorGUILayout.BeginHorizontal();

            SerializedProperty nameProperty = interactionSetProperty.FindPropertyRelative("_name");
            SerializedProperty includeTagsFilterProperty = interactionSetProperty.FindPropertyRelative("_includeTagsFilter");
            SerializedProperty excludeTagsFilterProperty = interactionSetProperty.FindPropertyRelative("_excludeTagsFilter");

            interactionFoldouts[index] = EditorGUILayout.Foldout(interactionFoldouts[index], new GUIContent(nameProperty.stringValue, ""), true);

            Color originalColor = GUI.color;
            GUI.color = warningColor;
            if (GUILayout.Button(new GUIContent("X", "Remove"), GUILayout.Width(20), GUILayout.Height(15)))
            {
                Undo.RecordObject(target, "Remove Interaction Set");
                material.RemoveInteractionSet(index);
                interactionFoldouts.RemoveAt(index);
                removed = true;
                EditorUtility.SetDirty(target);
            }
            GUI.color = originalColor;

            EditorGUILayout.EndHorizontal();

            if (!removed && interactionFoldouts[index])
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginVertical();

                GUILayout.Space(2);

                EditorGUILayout.PropertyField(nameProperty, new GUIContent("Name", "A human-readable name for the interaction set."));

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("Filters", EditorStyles.boldLabel);

                drawTagMaskFilter(includeTagsFilterProperty, new GUIContent("Include Tags", "Tags that, when present, will cause this interaction set to be used."));
                drawTagMaskFilter(excludeTagsFilterProperty, new GUIContent("Exclude Tags", "Tags that, when present, will cause this interaction set to be ignored."));

                ImpactEditorUtilities.Separator();

                drawInteractionsList(interactionSetProperty, interactionSet);
            }
        }

        private void drawInteractionsList(SerializedProperty interactionSetProperty, ImpactMaterialInteractionSet interactionSet)
        {
            EditorGUILayout.LabelField("Interactions", EditorStyles.boldLabel);
            SerializedProperty interactionsListProperty = interactionSetProperty.FindPropertyRelative("_interactions");

            for (int i = 0; i < interactionsListProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                SerializedProperty interactionProperty = interactionsListProperty.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(interactionProperty, new GUIContent());

                Color originalColor = GUI.color;
                GUI.color = warningColor;
                if (GUILayout.Button(new GUIContent("X", "Remove"), GUILayout.Width(20), GUILayout.Height(15)))
                {
                    interactionsListProperty.DeleteArrayElementAtIndex(i);
                }
                GUI.color = originalColor;

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.HelpBox("You can drag-and-drop Interactions here to add them.", MessageType.Info);

            GUILayout.Space(2);
            EditorGUILayout.EndVertical();

            //Drag and drop
            Rect listRect = GUILayoutUtility.GetLastRect();
            string[] paths;
            bool drop = ImpactEditorUtilities.DragAndDropArea(listRect, out paths);

            if (drop && paths != null)
            {
                Undo.RecordObject(target, "Drag-and-drop Interaction Sets");
                for (int i = 0; i < paths.Length; i++)
                {
                    ImpactInteractionBase a = AssetDatabase.LoadAssetAtPath<ImpactInteractionBase>(paths[i]);
                    if (a != null)
                    {
                        interactionSet.AddInteraction(a);
                    }
                }
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Add Interaction"))
            {
                interactionsListProperty.InsertArrayElementAtIndex(interactionsListProperty.arraySize);
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2);

            EditorGUILayout.EndVertical();
        }

        private void drawTagMaskFilter(SerializedProperty filterProperty, GUIContent label)
        {
            EditorGUILayout.BeginHorizontal();

            SerializedProperty tagMaskProperty = filterProperty.FindPropertyRelative("TagMask");
            SerializedProperty exactProperty = filterProperty.FindPropertyRelative("ExactMatch");

            ImpactEditorUtilities.TagMaskPropertyEditor(tagMaskProperty, label, tagNames);
            exactProperty.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Exact", "Should the tags exactly match the specified tags?"), exactProperty.boolValue, GUILayout.Width(60));

            EditorGUILayout.EndHorizontal();
        }
    }
}