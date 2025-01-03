using UnityEngine;

namespace WJ.Core.Base.Input
{
    public interface WJBaseInputInterface
    {
        Vector2 GetMovementInput();
        bool GetButtonDown(string buttonName);
        bool GetButton(string buttonName);
        bool GetButtonUp(string buttonName);
    }
} 