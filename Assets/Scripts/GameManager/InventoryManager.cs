using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status {get; private set;}

    private Dictionary<string, int> _items;

    private int _specialBombsCapacity;

    [SerializeField] public Text[] specialBombsValue;

    [SerializeField] public Text[] specialBombsCapacityValue;

    public void Startup(){
        Debug.Log("Inventory manager starting...");

        _items = new Dictionary<string, int>();

        _specialBombsCapacity = 10;

        foreach(Text capacityValue in specialBombsCapacityValue){
            capacityValue.text = _specialBombsCapacity.ToString();
        }

        status = ManagerStatus.Started;
    }

    private void DisplayItems() {
        string itemDisplay = "List of Items: ";

        foreach(KeyValuePair<string, int> item in _items){
            itemDisplay += item.Key + "(" + item.Value + ") ";
        }

        Debug.Log(itemDisplay);
    }

    public void AddItem(string name) {
        if(_items.ContainsKey(name)) {
            _items[name] += 1;
        } else {
            _items[name] = 1;
        }

        DisplayItems();
    }

    public void AddSpecialBomb(string name) {
        if(_items.ContainsKey(name)) {
            _items[name] += 1;
        } else {
            _items[name] = 1;
        }

        specialBombsValue[int.Parse(name.Split(' ')[1])].text=GetItemCount(name).ToString();

        DisplayItems();
    }

    public List<string> GetItemList() {
        List<string> list = new List<string>(_items.Keys);
        return list;
    }

    public int GetItemCount(string name){
        if(_items.ContainsKey(name)){
            return _items[name];
        }
        return 0;
    }

    public void ConsumeItem(string name){
        if(_items.ContainsKey(name)){
            _items[name]--;
            if(_items[name] == 0){
                _items.Remove(name);
            }
        } else {
            Debug.Log("cannot consume " + name);
        }
        DisplayItems();
    }

    public void ConsumeSpecialBomb(int i){
        string name = "Liquid " + i;
        if(_items.ContainsKey(name)){
            _items[name]--;
            if(_items[name] == 0){
                _items.Remove(name);
            }
        } else {
            Debug.Log("cannot consume " + name);
        }

        specialBombsValue[i].text=GetItemCount(name).ToString();

        DisplayItems();
    }

    public int GetSpecialBombsCapacity(){
        return _specialBombsCapacity;
    }
}
