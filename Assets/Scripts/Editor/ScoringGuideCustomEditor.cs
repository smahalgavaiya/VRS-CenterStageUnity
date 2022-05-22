using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(ScoringGuide))]
public class ScoringGuideCustomEditor : Editor
{
    ScoringGuide scoringGuide;
    SerializedProperty roundIndex, scoreObjectTypes, scoresPerRoundPerType, objectTypesFolder;

    private void OnEnable()
    {
        scoringGuide = (ScoringGuide)target;
        roundIndex = serializedObject.FindProperty("roundIndex");
        scoreObjectTypes = serializedObject.FindProperty("scoreObjectTypes");
        scoresPerRoundPerType = serializedObject.FindProperty("scoresPerRoundPerType");
        objectTypesFolder = serializedObject.FindProperty("objectTypesFolder");
    }

    public override void OnInspectorGUI()
    {
        if (scoringGuide.roundIndex == null)
            scoringGuide.roundIndex = 
                Resources.Load<RoundIndex>("Indexes/RoundIndexDefault");

        EditorGUILayout.PropertyField(objectTypesFolder);

        if (GUILayout.Button("Get or create path"))
        {
            string relativePath = "/Resources/DynamicObjects/"; 
            string folderPath = EditorUtility.OpenFolderPanel("Get or create path", "Assets" + relativePath, "");

            scoringGuide.objectTypesFolder = folderPath.Substring(Application.dataPath.Length + relativePath.Length);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        if (GUILayout.Button("Load Scoring Object Types"))
        {
            LoadScoreObjectTypes();
        }

        EditorGUILayout.PropertyField(roundIndex);

        // Update the custom editor's serialized properties so they match the data they are serializing
        serializedObject.Update();

        for (int i = 0; i < scoringGuide.scoreObjectTypes.Length; i++)
        {
            try
            {
                EditorGUILayout.LabelField(scoringGuide.scoreObjectTypes[i].name);
            }
            catch
            {
                LoadScoreObjectTypes();
            }

            for (int j = 0; j < scoringGuide.roundIndex.rounds.Count; j++)
            {
                string propertyLabel = "Round: " + scoringGuide.roundIndex.rounds[j].roundName;
                EditorGUILayout.PropertyField(scoresPerRoundPerType.GetArrayElementAtIndex(i).FindPropertyRelative("scoresPerRound").GetArrayElementAtIndex(j), new GUIContent(propertyLabel));
            }

            EditorGUILayout.Space();

        }
        serializedObject.ApplyModifiedProperties();
    }

    private void LoadScoreObjectTypes()
    {
        ScorePerRoundPerType[] oldScorePerRoundPerType = null;
        ObjectType[] oldScoreObjectTypes = null;

        if (scoringGuide.scoresPerRoundPerType != null)
        {
            oldScorePerRoundPerType = scoringGuide.scoresPerRoundPerType;
            oldScoreObjectTypes = scoringGuide.scoreObjectTypes;
        }

        // Load all the scoring object types from Resources
        var tempArrayAllObjectTypes = 
            Resources.LoadAll<ObjectType>("DynamicObjects/" + scoringGuide.objectTypesFolder);

        var tempArrayScoringObjects = tempArrayAllObjectTypes.TakeWhile(element => element.isScoringObject == true);

        scoringGuide.scoreObjectTypes = tempArrayScoringObjects.ToArray();

        // Set the number of scores per round, based on type
        scoringGuide.scoresPerRoundPerType =
            new ScorePerRoundPerType[scoringGuide.scoreObjectTypes.Length];

        // Create new ScorePerRoundPerType classes and populate them with the number of scores per round
        for (int i = 0; i < scoringGuide.scoreObjectTypes.Length; i++)
        {
            scoringGuide.scoresPerRoundPerType[i] = new ScorePerRoundPerType();
            scoringGuide.scoresPerRoundPerType[i].scoresPerRound =
                new int[scoringGuide.roundIndex.rounds.Count];
        }

        // If we updated values, retain the old values
        if (oldScoreObjectTypes != null)
        {
            for (int i = 0; i < oldScoreObjectTypes.Length; i++)
            {
                CheckObjectTypeMatchAndLoadStoredValues(i);
            }

            void CheckObjectTypeMatchAndLoadStoredValues(int indexValue)
            {
                for (int j = 0; j < scoringGuide.scoreObjectTypes.Length; j++)
                {
                    if (oldScoreObjectTypes[indexValue] != null
                        && oldScoreObjectTypes[indexValue].name == scoringGuide.scoreObjectTypes[j].name)
                        scoringGuide.scoresPerRoundPerType[j] = oldScorePerRoundPerType[indexValue];
                }
            }
        }

    }
}

