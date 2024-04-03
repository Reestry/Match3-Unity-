// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;

[System.Serializable]
public class CellRow
{
    public List<CellType> Cells;
    
    public enum CellType
    {
        Empty,  
        Normal,  
        SimpleIce,
        SimpleStone,
        StrongIce,
        StrongStone
    }

    public CellRow(int size)
    {
        Cells = new List<CellType>(new CellType[size]);
    }
}