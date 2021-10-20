using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using HartLib;
using static HartLib.Utils;

public class InfoModule
{
    public  HashSet<IGetInfoable> infoObjects { get; } = new HashSet<IGetInfoable>();
    public  HashSet<T> GetInfoObjectsOfType<T>() => new HashSet<T>(infoObjects.OfType<T>());
    public  HashSet<PlayerCharacter> GetPlayerInfoObjects() => GetInfoObjectsOfType<PlayerCharacter>();
}
