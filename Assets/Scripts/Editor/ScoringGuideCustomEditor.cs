using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoringGuide))]
public class ScoringGuideCustomEditor : Editor
{
    ScoringGuide scoringGuide;
    SerializedProperty roundIndex, scoreObjectTypes, scoresPerRound;

    private void OnEnable()
    {
        scoringGuide = (ScoringGuide)target;
        roundIndex = serializedObject.FindProperty("roundIndex");
        scoreObjectTypes = serializedObject.FindProperty("scoreObjectTypes");
        scoresPerRound = serializedObject.FindProperty("scoresPerRound");
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

            for (int i = 0; i < scoringGuide.scoreObjectTypes.Length; i++)
            {
                List<ScorePerRound> scorePerRound = new List<ScorePerRound>();

                for (int j = 0; j < scoringGuide.roundIndex.rounds.Count; j++)
                {
                    scorePerRound.Add(
                        new ScorePerRound()
                        {
                            RoundName = scoringGuide.roundIndex.rounds[i].roundName,
                            RoundNumber = j
                        });
                }
            }

        }

        EditorGUILayout.PropertyField(roundIndex);
        EditorGUILayout.PropertyField(scoresPerRound);

        for (int i = 0; i < scoringGuide.scoreObjectTypes.Length; i++)
        {
            EditorGUILayout.LabelField(scoringGuide.scoreObjectTypes[i].name);

            for (int j = 0; j < scoringGuide.roundIndex.rounds.Count; j++)
            {
                EditorGUILayout.LabelField("Round: " + scoringGuide.roundIndex.rounds[j].roundName);
                //EditorGUILayout.PropertyField(scoresPerRound.GetArrayElementAtIndex(i).GetArrayElementAtIndex(j));
            }

        }

        serializedObject.ApplyModifiedProperties();
    }

}

