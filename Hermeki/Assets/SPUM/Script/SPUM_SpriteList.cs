using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SPUM_SpriteList : MonoBehaviour
{
    public List<SpriteRenderer> _itemList = new List<SpriteRenderer>();
    public List<SpriteRenderer> _eyeList = new List<SpriteRenderer>();
    public List<SpriteRenderer> _hairList = new List<SpriteRenderer>();
    public List<SpriteRenderer> _bodyList = new List<SpriteRenderer>();
    public List<SpriteRenderer> _clothList = new List<SpriteRenderer>();
    public List<SpriteRenderer> _armorList = new List<SpriteRenderer>();
    public List<SpriteRenderer> _pantList = new List<SpriteRenderer>();
    public List<SpriteRenderer> _weaponList = new List<SpriteRenderer>();
    public List<SpriteRenderer> _backList = new List<SpriteRenderer>();

    public SPUM_HorseSpriteList _spHorseSPList;
    public string _spHorseString;
    // Start is called before the first frame update

    public Texture2D _bodyTexture;
    public string _bodyString;
    public string _eyeString;

    public List<string> _hairListString = new List<string>();
    public List<string> _clothListString = new List<string>();
    public List<string> _armorListString = new List<string>();
    public List<string> _pantListString = new List<string>();
    public List<string> _weaponListString = new List<string>();
    public List<string> _backListString = new List<string>();

    public void Reset()
    {
        for (var i = 0; i < _hairList.Count; i++)
        {
            if (_hairList[i] != null) _hairList[i].sprite = null;
        }
        for (var i = 0; i < _clothList.Count; i++)
        {
            if (_clothList[i] != null) _clothList[i].sprite = null;
        }
        for (var i = 0; i < _armorList.Count; i++)
        {
            if (_armorList[i] != null) _armorList[i].sprite = null;
        }
        for (var i = 0; i < _pantList.Count; i++)
        {
            if (_pantList[i] != null) _pantList[i].sprite = null;
        }
        for (var i = 0; i < _weaponList.Count; i++)
        {
            if (_weaponList[i] != null) _weaponList[i].sprite = null;
        }
        for (var i = 0; i < _backList.Count; i++)
        {
            if (_backList[i] != null) _backList[i].sprite = null;
        }
    }

    public void LoadSpriteSting()
    {

    }

    public void LoadSpriteStingProcess(List<SpriteRenderer> SpList, List<string> StringList)
    {
        for (var i = 0; i < StringList.Count; i++)
        {
            if (StringList[i].Length > 1)
            {

                // Assets/SPUM/SPUM_Sprites/BodySource/Species/0_Human/Human_1.png
            }
        }
    }

    public void LoadSprite(SPUM_SpriteList data)
    {
        //스프라이트 데이터 연동
        SetSpriteList(_hairList, data._hairList);
        SetSpriteList(_bodyList, data._bodyList);
        SetSpriteList(_clothList, data._clothList);
        SetSpriteList(_armorList, data._armorList);
        SetSpriteList(_pantList, data._pantList);
        SetSpriteList(_weaponList, data._weaponList);
        SetSpriteList(_backList, data._backList);
        SetSpriteList(_eyeList, data._eyeList);

        if (data._spHorseSPList != null)
        {
            SetSpriteList(_spHorseSPList._spList, data._spHorseSPList._spList);
            _spHorseSPList = data._spHorseSPList;
        }
        else
        {
            _spHorseSPList = null;
        }

        //색 데이터 연동.
        if (_eyeList.Count > 2 && data._eyeList.Count > 2)
        {
            _eyeList[2].color = data._eyeList[2].color;
            _eyeList[3].color = data._eyeList[3].color;
        }

        _hairList[3].color = data._hairList[3].color;
        _hairList[0].color = data._hairList[0].color;
        //꺼져있는 오브젝트 데이터 연동.x
        _hairList[0].gameObject.SetActive(!data._hairList[0].gameObject.activeInHierarchy);
        _hairList[3].gameObject.SetActive(!data._hairList[3].gameObject.activeInHierarchy);

        _hairListString = data._hairListString;
        _clothListString = data._clothListString;
        _pantListString = data._pantListString;
        _armorListString = data._armorListString;
        _weaponListString = data._weaponListString;
        _backListString = data._backListString;
    }

    public void SetSpriteList(List<SpriteRenderer> tList, List<SpriteRenderer> tData)
    {
        for (var i = 0; i < tData.Count; i++)
        {
            if (tData[i] != null)
            {
                tList[i].sprite = tData[i].sprite;
                tList[i].color = tData[i].color;
            }
            else tList[i] = null;
        }
    }

    public void ResyncData()
    {
        SyncPath(_hairList, _hairListString);
        SyncPath(_clothList, _clothListString);
        SyncPath(_armorList, _armorListString);
        SyncPath(_pantList, _pantListString);
        SyncPath(_weaponList, _weaponListString);
        SyncPath(_backList, _backListString);
        SyncPath(_bodyList, _bodyString, true); //추가
        SyncPath(_eyeList, _eyeString, false);  //추가
    }

    public void SyncPath(List<SpriteRenderer> _objList, List<string> _pathList)
    {
        for (var i = 0; i < _pathList.Count; i++)
        {
            if (_pathList[i].Length > 1)
            {
                string tPath = _pathList[i];
                tPath = tPath.Replace("Assets/Resources/", "");
                tPath = tPath.Replace(".png", "");

                Sprite[] tSP = Resources.LoadAll<Sprite>(tPath);
                if (tSP.Length > 1)
                {
                    if (_objList[0].name == "ClothBody" || _objList[0].name == "BodyArmor")
                    {
                        string tmpName = "";
                        switch (i)
                        {
                            case 0:
                                tmpName = "Body";
                                break;

                            case 1:
                                tmpName = "Left";
                                break;

                            case 2:
                                tmpName = "Right";
                                break;
                        }

                        foreach (Sprite ttS in tSP)
                        {
                            if (ttS.name == tmpName)
                            {
                                _objList[i].sprite = ttS;
                                break;
                            }
                        }
                    }
                }
                else if (tSP.Length > 0)
                {
                    _objList[i].sprite = tSP[0];
                }
            }
            else
            {
                _objList[i].sprite = null;
            }
        }
    }

    public void SyncPath(List<SpriteRenderer> _objList, List<Sprite> _sprites)
    {
        for (int i = 0; i < _objList.Count; i++)
        {
            if (_sprites == null)
            {
                _objList[i].sprite = null;
                continue;
            }

            if (_sprites.Count - 1 >= i)
            {
                _objList[i].sprite = _sprites[i];
            }
        }
    }
    public void SyncPath(List<SpriteRenderer> _objList, string _path, bool _order)
    {        
        string tPath = _path;
        tPath = tPath.Replace("Assets/Resources/", "");
        tPath = tPath.Replace(".png", "");

        Sprite[] tSP = Resources.LoadAll<Sprite>(tPath);
        if(_order)
        {
            for (var i = 0; i < _objList.Count; i++)
            {
                if (tSP[i] == null)
                {
                    _objList[i].sprite = null;
                    continue;
                }
                _objList[i].sprite = tSP[i];
            }
        }
        else
        {
            for (var i = 0; i < _objList.Count; i++)
            {
                foreach (var tsp in tSP)
                {
                    if (_objList[i].name == tsp.name)
                    {
                        _objList[i].sprite = tsp;
                        break;
                    }
                    else
                    {
                        _objList[i].sprite = null;
                    }
                }
            } 
        }
    }

    public void SyncPath(SpriteRenderer _obj, Sprite _sprite)
    {
        if (_obj == null || _sprite == null)
            return;

        _obj.sprite = _sprite;
    }

    public void SetHair(string Path)
    {
        if(Path == null)
        {
            _hairList[0].sprite = null;
            _hairListString[0] = "";
            return;
        }
        _hairListString[0] = Path;
        string tPath = Path;
        tPath = tPath.Replace("Assets/Resources/", "");
        tPath = tPath.Replace(".png", "");
        Sprite tSP = Resources.Load<Sprite>(tPath);
        _hairList[0].sprite = tSP;
    }

    public void SetBody(string Path)
    {
        if (Path == null)
        {
            foreach (var cloth in _clothList)
            {
                cloth.sprite = null;
            }
            for (int i = 0; i < _clothListString.Count; i++)
            {
                _clothListString[i] = "";
            }
            _clothList[0].sprite = GlobalValue.Base_SPUM_Cloth;
            _clothListString[0] = Path;
            return;
        }
        string tPath = Path;
        tPath = tPath.Replace("Assets/Resources/", "");
        tPath = tPath.Replace(".png", "");
        Sprite[] tSP = Resources.LoadAll<Sprite>(tPath);
        for (int i = 0; i < _clothList.Count; i++)
        {
            if (i >= tSP.Length)
            {
                _clothList[i].sprite = null;
                _clothListString[i] = "";
                continue;
            }
            _clothList[i].sprite = tSP[i];
            _clothListString[i] = tPath;
        }
    }
    public void SetBack(string Path)
    {
        if (Path == null)
        {
            _backList[0].sprite = null;
            _backListString[0] = "";
            return;
        }
        string tPath = Path;
        tPath = tPath.Replace("Assets/Resources/", "");
        tPath = tPath.Replace(".png", "");
        Sprite tSP = Resources.Load<Sprite>(tPath);
        _backList[0].sprite = tSP;
        _backListString[0] = tPath;
    }
}
