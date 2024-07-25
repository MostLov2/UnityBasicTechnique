using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    void Awake()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;
        Managers.UI.ShowSceneUI<UI_Inven>();
        Dictionary<int,Data.Stat> dict =  Managers.Data.StatDict;

        GameObject player =  Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CamraController>().SetPlayer(player);
        //Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");

        GameObject go = new GameObject { name = "SpawningPool" };
        SpawningPool pool =  go.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);

    }

    public override void Clear()
    {

    }

}
