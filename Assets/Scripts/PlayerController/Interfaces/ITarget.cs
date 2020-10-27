using UnityEngine;

namespace PlayerController.Interfaces
{
    public interface ITarget
    {
        bool IsDead();
        Vector3 GetCurrentPosition();
        Quaternion GetLocalRotation();
    }
}