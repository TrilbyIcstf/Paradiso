using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Run_Manager : ManagerBehavior
{
    public List<Exploration_Floor_Data> FloorList { get; private set; } = new List<Exploration_Floor_Data>();
    
    private Layout_Generator generator = new Layout_Generator();

    public void GenerateBasicRun(int length)
    {
        for (int i = 1; i <= length; i++)
        {
            Exploration_Floor_Data floor = generator.GenerateBasicFloor(i);

            FloorList.Add(floor);
        }
    }
}
