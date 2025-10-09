using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamingXRCore.SpreadsheetIntegration
{
    [Serializable]
    public class StringTable
    {
        public List<Row> rows = new List<Row>();

        [Serializable]
        public class Row
        {
            public List<string> cells = new List<string>();
        }

        public static StringTable FromList(List<List<string>> source)
        {
            var table = new StringTable();

            if (source == null || source.Count == 0)
                return table;

            int maxColumns = 0;
            foreach (var row in source)
            {
                if (row != null && row.Count > maxColumns)
                    maxColumns = row.Count;
            }

            foreach (var row in source)
            {
                var newRow = new Row();

                for (int i = 0; i < maxColumns; i++)
                {
                    string value = (i < row.Count) ? row[i] : string.Empty;
                    newRow.cells.Add(value ?? string.Empty);
                }

                table.rows.Add(newRow);
            }

            return table;
        }

        public List<List<string>> ToList()
        {
            var list = new List<List<string>>();
            if (rows == null || rows.Count == 0)
                return list;

            int maxColumns = 0;
            foreach (var r in rows)
            {
                if (r?.cells != null && r.cells.Count > maxColumns)
                    maxColumns = r.cells.Count;
            }

            foreach (var r in rows)
            {
                var newRow = new List<string>();
                if (r?.cells == null)
                    r.cells = new List<string>();

                for (int i = 0; i < maxColumns; i++)
                {
                    string value = (i < r.cells.Count) ? r.cells[i] : string.Empty;
                    newRow.Add(value ?? string.Empty);
                }

                list.Add(newRow);
            }

            return list;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(StringTable))]
    public class StringTableDrawer : PropertyDrawer
    {
        private const float CellWidth = 100f;
        private const float CellHeight = 18f;
        private const float Padding = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var rows = property.FindPropertyRelative("rows");
            float height = EditorGUIUtility.singleLineHeight;
            for (int i = 0; i < rows.arraySize; i++)
            {
                var row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("cells");
                height += CellHeight + Padding;
            }

            height += EditorGUIUtility.singleLineHeight * 2 + Padding * 2;
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label);
            position.y += EditorGUIUtility.singleLineHeight + Padding;

            var rowsProp = property.FindPropertyRelative("rows");

            for (int i = 0; i < rowsProp.arraySize; i++)
            {
                var rowProp = rowsProp.GetArrayElementAtIndex(i).FindPropertyRelative("cells");
                float x = position.x;

                for (int j = 0; j < rowProp.arraySize; j++)
                {
                    var cellProp = rowProp.GetArrayElementAtIndex(j);
                    var cellRect = new Rect(x, position.y, CellWidth, CellHeight);
                    cellProp.stringValue = EditorGUI.TextField(cellRect, cellProp.stringValue);
                    x += CellWidth + Padding;
                }

                var addCellRect = new Rect(x, position.y, 20, CellHeight);
                if (GUI.Button(addCellRect, "+"))
                    rowProp.InsertArrayElementAtIndex(rowProp.arraySize);

                var removeCellRect = new Rect(x + 24, position.y, 20, CellHeight);
                if (GUI.Button(removeCellRect, "-") && rowProp.arraySize > 0)
                    rowProp.DeleteArrayElementAtIndex(rowProp.arraySize - 1);

                position.y += CellHeight + Padding;
            }

            if (GUI.Button(new Rect(position.x, position.y, 60, EditorGUIUtility.singleLineHeight), "Add Row"))
            {
                rowsProp.InsertArrayElementAtIndex(rowsProp.arraySize);
            }

            if (GUI.Button(new Rect(position.x + 70, position.y, 90, EditorGUIUtility.singleLineHeight), "Remove Row"))
            {
                if (rowsProp.arraySize > 0)
                    rowsProp.DeleteArrayElementAtIndex(rowsProp.arraySize - 1);
            }
        }
    }
#endif
}