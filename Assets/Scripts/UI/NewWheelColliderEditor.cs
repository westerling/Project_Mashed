using UnityEditor;

[CustomEditor(typeof(NewWheelCollider)), CanEditMultipleObjects]
public class NewWheelColliderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();

		base.OnInspectorGUI();

		if (EditorGUI.EndChangeCheck())
		{
			for (int i = 0; i < targets.Length; i++)
			{
				(targets[i] as NewWheelCollider).UpdateConfig();
			}
		}
	}

	void OnEnable()
	{
		for (int i = 0; i < targets.Length; i++)
		{
			if ((targets[i] as NewWheelCollider).CheckFirstEnable())
			{
				serializedObject.SetIsDifferentCacheDirty();
				serializedObject.Update();
			}
		}
	}
}
