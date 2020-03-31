﻿using System;
using Timespawn.Core.Extensions;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Assertions;

namespace Timespawn.Core.DOTS.Grids
{
    public static class GridUtils
    {
        public static Entity CreateCellEntity(float3 gridCenter, GridData gridData, UInt16 x, UInt16 y, Entity prefab)
        {
            Assert.IsTrue(gridData.IsValidCoordinates(x, y), "Should be valid coordinates in the grid.");
            Assert.IsTrue(prefab != Entity.Null, "Should provide a non-null entity prefab.");

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity entity = entityManager.Instantiate(prefab);
            entityManager.SetComponentData(entity, new Translation
            {
                Value = gridData.GetWorldCellCenter(gridCenter, x, y),
            });
            
            entityManager.AddComponentData(entity, new CellData
            {
                x = x,
                y = y,
            });

            return entity;
        }

        public static void SetCellData(Entity entity, int2 coords)
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            entityManager.SetComponentData(entity, new CellData
            {
                x = (ushort) coords.x,
                y = (ushort) coords.y,
            });
        }

        public static bool IsCellEmpty(NativeArray<CellData> cells, int2 coords)
        {
            foreach (CellData cell in cells)
            {
                if (cell.x == coords.x && cell.y == coords.y)
                {
                    return false;
                }
            }

            return true;
        }
    }
}