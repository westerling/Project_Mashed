using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    //Masks
    public static LayerMask TerrainLayerMask = 1 << 8;
    public static LayerMask CarLayerMask = 1 << 7;

    //Traffic Light colors
    public static Color TrafficRed = new Color32(204, 50, 50, 100);
    public static Color TrafficGreen = new Color32(45, 201, 55, 100);
    public static Color TrafficBlack = new Color32(5, 5, 5, 100);

    //Car Colors
    public static Color CarRed = new Color32(208, 66, 66, 100);
    public static Color CarBlue = new Color32(64, 204, 208, 100);
    public static Color CarGreen = new Color32(60, 219, 78, 100);
    public static Color CarYellow = new Color32(236, 219, 51, 100);
    

    public static List<Color> Colours = new List<Color>()
    {
        CarRed,
        CarBlue,
        CarGreen,
        CarYellow
    };

    public static int CalculatePoints(int position, int currentPoints, int numberOfPlayers)
    {
        switch (numberOfPlayers)
        {
            case 2:
                return CalculatePointsTwoPlayers(position, currentPoints);
            case 3:
                return CalculatePointsThreePlayers(position, currentPoints);
            case 4:
                return CalculatePointsFourPlayers(position, currentPoints);
        }

        return 0;
    }

    private static int CalculatePointsTwoPlayers(int position, int currentPoints)
    {
        switch (position)
        {
            case 0:
                return currentPoints == 0 ? 0 : -1;
            case 1:
                return 1;
        }

        return 0;
    }

    private static int CalculatePointsThreePlayers(int position, int currentPoints)
    {
        switch (position)
        {
            case 0:
                return currentPoints == 0 ? 0 : -1;
            case 1:
                return 0;
            case 2:
                return 1;
        }

        return 0;
    }

    private static int CalculatePointsFourPlayers(int position, int currentPoints)
    {
        switch (position)
        {
            case 0:
                switch(currentPoints)
                {
                    case 0:
                        return 0;
                    case 1:
                        return -1;
                }
                return -2;
            case 1:
                return currentPoints == 0 ? 0 : -1;
            case 2:
                return currentPoints == 9 ? 0 : 1;
            case 3:
                return currentPoints == 9 ? 1 : 2;
        }

        return 0;
    }

    public static float KphToMs(float kph)
    {
        return kph / 3.6f;
    }

    public static float MsToKph(float ms)
    {
        return ms * 3.6f;
    }

    public static float GetGameLength(int index)
    {
        switch (index)
        {
            case 0:
                return 60f;
            case 1:
                return 180f;
            case 2:
                return 300f;
            case 3:
                return 600f;
        }

        return 300f;
    }

    public static string GetMapName(int index)
    {
        switch (index)
        {
            case 1:
                return "Bobas by";
            case 2:
                return "Negerlandet";
            case 3:
                return "Krahlen";
        }

        return string.Empty;
    }

    public static float LoopClamp(float value, float minValue, float maxValue)
    {
        while (value < minValue || value >= maxValue)
        {
            if (value < minValue)
            {
                value += maxValue - minValue;
            }
            else if (value >= maxValue)
            {
                value -= maxValue - minValue;
            }
        }
        return value;
    }

    public static int MaxPoints(int numberOfPlayers)
    {
        switch(numberOfPlayers)
        {
            case 2:
            case 3:
                return 8;
            case 4:
                return 12;
        }
        return 12;
    }

    public static int StartPoints(int numberOfPlayers)
    {
        switch (numberOfPlayers)
        {
            case 2:
            case 3:
                return 4;
            case 4:
                return 6;
        }
        return 6;
    }
}
