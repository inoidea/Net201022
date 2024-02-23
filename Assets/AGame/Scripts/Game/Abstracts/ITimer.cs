using System;

public interface ITimer
{
    void AwaiteFor(float timeMax, Action onCompleted);
}