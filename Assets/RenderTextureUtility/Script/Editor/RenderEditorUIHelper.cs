using UnityEditor;
using UnityEngine;

public static class RenderEditorUIHelper
{
    private static Camera GetRenderCamera()
    {
        var cameras = Camera.allCameras;
        for (var i = 0; i < cameras.Length; i++)
        {
            var targetCam = cameras[i];
            if (targetCam.targetTexture != null)
            {
                return targetCam;
            }
        }
        return Camera.main;
    }

    [MenuItem("GameObject/居中对齐渲染的相机", false)]
    public static void SetInTheCenterOfCamera()
    {
        var cam = GetRenderCamera();
        var selectedObject = Selection.activeGameObject;
        if (selectedObject != null && cam != null)
        {
            var pos = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z + 10);
            selectedObject.gameObject.transform.position = pos;
        }
    }
}
