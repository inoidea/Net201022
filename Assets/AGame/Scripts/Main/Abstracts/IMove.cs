using UnityEngine;

public interface IMove
{
    float Speed { get; set; }
    bool Invulnerable { get; set; }
    bool Stunned {  get; set; }
    Transform PlayerTransform { get; }
}
