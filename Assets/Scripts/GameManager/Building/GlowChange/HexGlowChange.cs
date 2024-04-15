using UnityEngine;

public class HexGlowChange : MonoBehaviour
{
    public bool MeError = false;
    [SerializeField] private Texture texstura;
    private ColorChangeRules _colorRules;
    private Material[] _materials;
    private Texture[] _defaultTextures;
    private LayerMask _myLayer;

    void Start()
    {
        _materials = GetComponent<MeshRenderer>().materials;
        _defaultTextures = new Texture[_materials.Length];
        _myLayer = gameObject.layer;
        if (MeError)
        {
            Debug.Log("");
        }
        for (int i = 0; i < _materials.Length; i++)
        {
            _defaultTextures[i] = _materials[i].mainTexture;
        }
        _colorRules = ColorChange.Instance.colorRules;
    }

    private void OnEnable()
    {
        EventManager.BuildingColorChange += ChangeGlow;
    }
    private void OnDisable()
    {
        EventManager.BuildingColorChange -= ChangeGlow;
    }


    private void ChangeGlow(WhichBudynek? type)
    {


        switch (type)
        {
            case null:
                ChangeMaterial();
                break;
            default:
                var turnGreen = _colorRules.CheckType(type, tag);

                if (_colorRules.CheckLayer(_myLayer))
                {
                    ChangeMaterial(turnGreen);
                }
                else
                {
                    ChangeMaterial(false);
                }

                break;
        }

    }
    private void ChangeMaterial(bool turnGreen)
    {
        if (!turnGreen)
        {
            foreach (var material in _materials)
            {
                material.mainTexture = texstura;
            }
        }

    }


    private void ChangeMaterial()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].mainTexture = _defaultTextures[i];
        }
    }


    private void ChangeMaterial(string tagid)
    {
        if (CompareTag(tagid))
        {
            foreach (var item in _materials)
            {
                item.color = _colorRules.green;
            }
        }
        else
        {
            foreach (var item in _materials)
            {
                item.color = _colorRules.grey;
            }
        }
    }


}
