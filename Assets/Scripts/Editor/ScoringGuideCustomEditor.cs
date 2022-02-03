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
            scoringGuide.scoreObjectTypes =
                Resources.LoadAll<ScoreObjectType>("SpawnableObjects/Score Object Types");

            scoringGuide.scoresPerRoundPerType = 
                new ScorePerRoundPerType[scoringGuide.scoreObjectTypes.Length];

            for (int i = 0; i < scoringGuide.scoreObjectTypes.Length; i++)
            {
                scoringGuide.scoresPerRoundPerType[i] = new ScorePerRoundPerType();
                scoringGuide.scoresPerRoundPerType[i].scoresPerRound =
                    new int[scoringGuide.roundIndex.rounds.Count];
            }
        }

        EditorGUILayout.PropertyField(roundIndex);

        for (int i = 0; i < scoringGuide.scoreObjectTypes.Length; i++)
        {
            EditorGUILayout.LabelField(scoringGuide.scoreObjectTypes[i].name);

            for (int j = 0; j < scoringGuide.roundIndex.rounds.Count; j++)
            {
                string propertyLabel = "Round: " + scoringGuide.roundIndex.rounds[j].roundName;
                EditorGUILayout.PropertyField(scoresPerRoundPerType.GetArrayElementAtIndex(i).FindPropertyRelative("scoresPerRound").GetArrayElementAtIndex(j), new GUIContent(propertyLabel));
            }

            EditorGUILayout.Space();

        }

        serializedObject.ApplyModifiedProperties();
    }

}

