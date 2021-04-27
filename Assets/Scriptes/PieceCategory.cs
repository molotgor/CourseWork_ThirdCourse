using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PieceCategory
{
    //First to bits represent piece rank
    public const int None = 0;
    public const int Student = 1;
    public const int Master = 2;
    //Next two bits represent piece color
    public const int Blue = 4;
    public const int Red = 8;
}