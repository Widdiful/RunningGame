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

    private static Color[] modeColours = {
        Color.white,
        Color.yellow,
        Color.cyan
    };

    private void OnSceneGUI()
    {
        // Initial setup
        curve = target as SplineCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < curve.ControlPointCount; i += 3)
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
        CalculateLength();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        curve = target as SplineCurve;

        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", curve.Loop);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(curve, "Toggle Loop");
            EditorUtility.SetDirty(curve);
            curve.Loop = loop;
        }

        if (selectedIndex >= 0 && selectedIndex < curve.ControlPointCount) {
            DrawSelectedPointInspector();
        }

        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(curve, "Add Curve");
            curve.AddCurve();
            EditorUtility.SetDirty(curve);
        }
        if (GUILayout.Button("Remove Curve")) {
            Undo.RecordObject(curve, "Remove Curve");
            curve.RemoveCurve();
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

    private void CalculateLength() {
        Vector3 point = curve.GetPoint(0f);
        Vector3 previousPoint = point;
        curve.splineLength = 0;
        int steps = stepsPerCurve * curve.CurveCount;
        for (int i = 1; i <= steps; i++) {
            point = curve.GetPoint(i / (float)steps);
            curve.splineLength += Vector3.Distance(point, previousPoint);
            previousPoint = point;
        }
    }

    private Vector3 ShowPoint (int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        if (index == 0) {
            size *= 2f;
        }
        Handles.color = modeColours[(int)curve.GetControlPointMode(index)];
        if (Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap))
        {
            selectedIndex = index;
            Repaint();
        }
        if (selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curve, "Move Point");
                EditorUtility.SetDirty(curve);
                curve.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }
        return point;
    }

    private void DrawSelectedPointInspector() {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", curve.GetControlPoint(selectedIndex));
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.SetControlPoint(selectedIndex, point);
        }
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)
            EditorGUILayout.EnumPopup("Mode", curve.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(curve, "Change Point Mode");
            curve.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(curve);
        }
    }
}
