using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoringGuide))]
public class ScoringGuideCustomEditor : Editor
{
    ScoringGuide scoringGuide;
    SerializedProperty roundIndex, scoreObjectTypes, scoresPerRoundPerType;

    private void OnEnable()
    {
        scoringGuide = (ScoringGuide)target;
        roundIndex = serializedObject.FindProperty("roundIndex");
        scoreObjectTypes = serializedObject.FindProperty("scoreObjectTypes");
        scoresPerRoundPerType = serializedObject.FindProperty("scoresPerRoundPerType");
    }

    public override void OnInspectorGUI()
    {
        if (scoringGuide.roundIndex == null)
            scoringGuide.roundIndex = 
                Resources.Load<RoundIndex>("Indexes/RoundIndexDefault");

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
        ScoreObjectType[] oldScoreObjectTypes = null;

        if (scoringGuide.scoresPerRoundPerType != null)
        {
            oldScorePerRoundPerType = scoringGuide.scoresPerRoundPerType;
            oldScoreObjectTypes = scoringGuide.scoreObjectTypes;
        }

        // Load all the scoring object types from Resources
        scoringGuide.scoreObjectTypes =
            Resources.LoadAll<ScoreObjectType>("SpawnableObjects/ScoringObjectTypes");

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

