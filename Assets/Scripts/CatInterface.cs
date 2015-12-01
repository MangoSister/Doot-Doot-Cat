using UnityEngine;
using System.Collections;

public interface CatInterface
{
    void Dash(Vector2 dir);
    void Boost(Vector2 dir);
    void Bounce(bool horiDir);
    bool OutsideBoundary(BoundaryType type);
    void EnableCtrl(bool enable);
}


public enum BoundaryType
{
    Left, Right, Top, Bottom,
}