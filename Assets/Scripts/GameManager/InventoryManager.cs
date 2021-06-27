using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status {get; private set;}

    private Dictionary<string, int> _items;

    private int _specialBombsCapacity;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip itemCollectedSound;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Startup(){
        Debug.Log("Inventory manager starting...");

        _items = new Dictionary<string, int>();

        _specialBombsCapacity = 10;

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
        _audioSource.PlayOneShot(itemCollectedSound);

        DisplayItems();
    }

    public void AddSpecialBomb(string name) {
        if(_items.ContainsKey(name)) {
            _items[name] += 1;
        } else {
            _items[name] = 1;
        }

        Messenger<int>.Broadcast(GameEvent.LIQUID_COLLECTED, int.Parse(name.Split(' ')[1]));
        _audioSource.PlayOneShot(itemCollectedSound);

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

        Messenger<int>.Broadcast(GameEvent.LIQUID_CONSUMED, int.Parse(name.Split(' ')[1]));

        DisplayItems();
    }

    public int GetSpecialBombsCapacity(){
        return _specialBombsCapacity;
    }
}
