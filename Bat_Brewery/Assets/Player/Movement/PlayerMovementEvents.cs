using System;
public class PlayerMovementEvents
{
    public event Action<string, bool> onFreezePlayerMovement;
    public void SetFreezePlayerMovement(string id, bool freeze)
    {
        if(onFreezePlayerMovement != null)
        {
            onFreezePlayerMovement(id, freeze);
        }
    }
}
