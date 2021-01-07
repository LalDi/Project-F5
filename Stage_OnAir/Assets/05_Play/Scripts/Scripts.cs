using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script
{
    public List<string> script;
    public bool[] Direction; //0 = 왼쪽, 1 = 오른쪽
    public bool Bgm; //0 = 밝은, 1 = 어두운
}
[System.Serializable]
public class ScriptS
{
    public List<Script> scripts;
}
