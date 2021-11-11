using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GridGame;
using Multiplayer;

public class UnitOwnershipHandler : MonoBehaviour
{
    protected DoubleDictOneToMany<Player, Unit> PlayerUnits;

    public void Initialize()
    {
        PlayerUnits = new DoubleDictOneToMany<Player, Unit>();
    }

    public void AddPlayer(Player player)
    {

    }

    public void AddUnitToPlayer(Unit unit, Player player)
    {
        PlayerUnits.AddPair(player, unit);
    }

    public void ReplaceUnitOwner(Unit unit, Player player)
    {
        PlayerUnits.SetPair(player, unit);
    }

    public void RemoveUnit(Unit unit)
    {
        PlayerUnits.RemoveSecondary(unit);
    }

    public Player GetOwner(Unit unit)
    {
        return PlayerUnits.GetPrimary(unit);
    }

    public List<Unit> GetUnits(Player player)
    {
        return PlayerUnits.GetSecondaryList(player);
    }

    public virtual bool AreEnemies(Unit u1, Unit u2)
    {
        return GetAllianceState(u1, u2) == AllianceState.Enemy;
    }

    public virtual AllianceState GetAllianceState(Unit u1, Unit u2)
    {
        var ph = GetComponent<PlayerHandler>();
        var player1 = GetOwner(u1);
        var player2 = GetOwner(u2);

        return ph.GetAllianceState(player1, player2); 
    }

}
