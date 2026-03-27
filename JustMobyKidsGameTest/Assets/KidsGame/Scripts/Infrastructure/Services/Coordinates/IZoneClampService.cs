using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Services.Coordinates
{
    public interface IZoneClampService
    {
        Vector2 Clamp(RectTransform zone, RectTransform cube, Vector2 desiredLocal);
    }
}

