using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public void OnPlayerInteract(Collider player, bool canInteract);
}
