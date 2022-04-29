using UnityEngine;
public interface IPickableItem : IInteractor
{
    // Start is called before the first frame update
    void PickUpItem(GameObject picker);
    void DropItem();
}
