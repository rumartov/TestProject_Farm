using UnityEngine;

public class Harvest : MonoBehaviour
{
    public VegetationType VegetationType { get; private set; }
    private bool _pickedUp;
    
    public void Construct(VegetationType vegetationType)
    {
        VegetationType = vegetationType;
        _pickedUp = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
             PickUp(collider.GetComponentInChildren<Backpack>());
        }
    }

    private void PickUp(Backpack backpack)
    {
        if (backpack.IsFull())
            return;
        if (_pickedUp)
            return;
        backpack.AddItem(gameObject);
        PlayAnimation();
        gameObject.transform.position = backpack.transform.position;
        gameObject.transform.SetParent(backpack.transform);

        _pickedUp = true;
    }

    private void PlayAnimation()
    {
        // TODO add dotween 
        //throw new NotImplementedException();
    }
}
