using System;
using UnityEngine;

public class PillStorage : MonoBehaviour
{
    public int pillCount;

    public delegate void OnPillCollectedHandler(object sender, EventArgs args);

    public event OnPillCollectedHandler OnPillCollect;
    
    private PlayerMovement _playerMovement;
    
    public void Initialize(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
        _playerMovement.PillCollected += OnPillCollected;
    }

    private void OnPillCollected()
    {
        pillCount++;
        OnPillCollect?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable()
    {
        _playerMovement.PillCollected -= OnPillCollected;
    }
}
