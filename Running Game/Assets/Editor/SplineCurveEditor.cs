using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplineCurve))]
public class SplineCurveEditor : Editor
{

    // https://catlikecoding.com/unity/tutorials/curves-and-splines/

    private SplineCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private const int stepsPerCurve = 10;
    private const float directionScale = 0.5f;
    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;

    private int selectedIndex = -1;

    private void OnSceneGUI()
    {
        // Initial setup
        curve = target as SplineCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;
        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < curve.points.Length; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }

        ShowDirections();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        curve = target as SplineCurve;
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(curve, "Add Curve");
            curve.AddCurve();
            EditorUtility.SetDirty(curve);
        }
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = curve.GetPoint(0f);
        Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve * curve.CurveCount;
        for (int i = 1; i <= steps; i++)
        {
            point = curve.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + curve.GetDirection(i / (float)steps) * directionScale);
        }
    }

    private Vector3 ShowPoint (int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        float size = HandleUtility.GetHandleSize(point);
        Handles.color = Color.white;
        if (Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap))
        {
            selectedIndex = index;
        }
        if (selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curve, "Move Point");
                EditorUtility.SetDirty(curve);
                curve.points[index] = handleTransform.InverseTransformPoint(point);
            }
        }
        return point;
    }
}
