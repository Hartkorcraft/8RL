using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class HealthSystem : IHealth
{
    public int HealthCap { get; set; } = 5;
    public int Health { get; set; } = 5;

    public virtual void Damage(int dmg)
    {
        if (Health - dmg <= 0)
        {
            Kill();
        }
        else
        {
            Health -= dmg;
        }
        GD.Print("Damaged: ", this.ToString(), " ", dmg, " New health: ", Health);
    }

    public virtual void Heal(int health)
    {
        if (Health + health <= HealthCap)
        {
            Health += health;
            GD.Print("Healed: ", this.ToString(), " ", health, " New health: ", Health);
        }
    }

    public virtual void Kill()
    {
        GD.Print("Killed: ", this.ToString());
        // QueueFree();
    }
}
