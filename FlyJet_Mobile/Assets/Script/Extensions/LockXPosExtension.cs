using Cinemachine;
using UnityEngine;

public class LockXPosExtension : CinemachineExtension
{
    [Tooltip("Lock the camera's Z position to this value")]
    public float x_Pos = 0;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (enabled && stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos.x = x_Pos;
            state.RawPosition = pos;
        }
    }

}
