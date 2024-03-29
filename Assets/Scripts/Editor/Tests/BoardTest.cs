﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardTests
{
    List<int[,]> gemsToCheck = new List<int[,]>()
    {
        new int[,] // 0
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 1
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,0,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 2
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 },
            { 5,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 3
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,0,3,1 },
            { 0,0,2,2,4,4,2,0,0 }
        },
        new int[,] // 4
        {
            { 0,0,2,2,4,4,2,2,5 },
            { 0,0,2,2,4,4,2,2,5 },
            { 1,1,3,3,5,5,3,0,1 },
            { 1,1,3,3,5,5,3,3,0 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 5
        {
            { 4,0,2,2,4,4,2,2,0 },
            { 4,0,2,2,4,4,2,2,0 },
            { 1,4,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 6
        {
            { 5,5,2,2,4,4,2,2,0 },
            { 0,0,5,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 7
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,0,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 8
        {
            { 0,0,2,2,4,4,2,0,0 },
            { 0,0,2,2,4,4,0,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 9
        {
            { 0,0,5,5,4,4,2,2,0 },
            { 0,5,2,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 10
        {
            { 0,0,1,1,4,4,2,2,0 },
            { 0,0,2,2,1,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 11
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 },
            { 1,1,3,4,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 12
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,3,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 13
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,5,2,2,4,4,2,2,0 },
            { 5,1,3,3,5,5,3,3,1 },
            { 5,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 14
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 },
            { 5,1,3,3,5,5,3,3,1 },
            { 5,1,3,3,5,5,3,3,1 },
            { 0,5,2,2,4,4,2,2,0 }
        },
        new int[,] // 15
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,5,0 },
            { 1,1,3,3,5,5,3,3,5 },
            { 1,1,3,3,5,5,3,3,5 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 16
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,5 },
            { 1,1,3,3,5,5,3,3,5 },
            { 0,0,2,2,4,4,2,5,0 }
        },
        new int[,] // 17
        {
            { 5,5,2,2,4,4,2,2,0 },
            { 1,0,2,2,4,4,2,2,0 },
            { 0,1,3,3,5,5,3,3,1 },
            { 1,5,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 18
        {
            { 0,0,2,5,2,4,5,2,0 },
            { 0,0,5,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 19
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,0,5 },
            { 1,1,3,3,5,5,3,3,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 20
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,2,5,5,3,3,1 },
            { 0,0,2,1,2,4,1,2,0 }
        },
        new int[,] // 21
        {
            { 0,0,5,5,4,4,2,2,0 },
            { 0,0,5,2,4,4,2,2,0 },
            { 1,1,2,3,5,5,3,3,1 },
            { 1,1,3,2,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 22
        {
            { 0,0,2,2,4,4,2,2,0 },                
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 23
        {
            { 0,2,0,2,4,4,2,2,0 },
            { 1,0,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 24
        {
            { 1,0,2,2,4,4,2,2,0 },
            { 5,1,3,3,5,5,3,3,1 },
            { 1,5,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 25
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 5,0,2,2,4,4,2,2,0 },
            { 0,5,0,2,4,4,2,2,0 }
        },
        new int[,] // 26
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,1,3,3,5,5,3,3,1 },
            { 5,0,2,2,4,4,2,2,0 },
            { 0,5,2,2,4,4,2,2,0 }
        },
        new int[,] // 27
        {
            { 0,0,2,2,4,4,2,5,2 },
            { 1,1,3,3,5,5,3,2,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 28
        {
            { 0,0,2,2,4,4,2,2,1 },
            { 1,1,3,3,5,5,3,1,5 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,2,2,0 },
            { 0,0,2,2,4,4,2,2,0 }
        },
        new int[,] // 29
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,1 },
            { 0,0,2,2,4,4,1,2,0 },
            { 0,0,2,2,4,4,2,1,2 }
        },
        new int[,] // 30
        {
            { 0,0,2,2,4,4,2,2,0 },
            { 1,1,3,3,5,5,3,3,1 },
            { 1,1,3,3,5,5,3,3,0 },
            { 0,0,2,2,4,4,2,0,1 },
            { 0,0,2,2,4,4,2,2,0 }
        },
    };
    List<bool> results = new List<bool>()
    {
        false, // 0
        true, // 1
        true, // 2
        true, // 3
        true, // 4
        true, // 5
        true, // 6
        true, // 7
        true, // 8
        true, // 9
        true, // 10
        true, // 11
        true, // 12
        true, // 13
        true, // 14
        true, // 15
        true, // 16
        true, // 17
        true, // 18
        true, // 19
        true, // 20
        true, // 21
        false, // 22
        true, // 23
        true, // 24
        true, // 25
        true, // 26
        true, // 27
        true, // 28
        true, // 29
        true // 30
    };

    [Test]
    public void BoardTest()
    {
        Board board = AssetDatabase.LoadAssetAtPath<Board>("Assets/Prefabs/Board.prefab");
        board = GameObject.Instantiate(board);

        typeof(Board).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(board, null);

        Gem gem = board.GetComponentInChildren<Gem>();
        Assert.IsNotNull(gem);

        FieldInfo gemsInfo = typeof(Board).GetField("gems", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo colorInfo = typeof(Gem).GetField("color", BindingFlags.NonPublic | BindingFlags.Instance);
        MethodInfo checkIsMatchPossibleInfo = typeof(Board).GetMethod("CheckIsMatchPossible", BindingFlags.NonPublic | BindingFlags.Instance);

        Gem[,] gems = (Gem[,])gemsInfo.GetValue(board);

        int i = 0;
        foreach (int[,] gemColors in gemsToCheck)
        {
            for (int y = 0; y < board.Size.y; y++)
            {
                for (int x = 0; x < board.Size.x; x++)
                {
                    colorInfo.SetValue(gems[x, y], (Gem.Colors)gemColors[x, y]);
                }
            }
            gemsInfo.SetValue(board, gems);
            Debug.Log("Testing.. " + i);
            Assert.AreEqual(results[i], (bool)checkIsMatchPossibleInfo.Invoke(board, null));
            i++;
        }
    }

}

