using System.Collections;
using UnityEngine;

public abstract class Mouse_Hover_Item : MonoBehaviour
{
    [SerializeField]
    protected GameObject infoBox;
    [SerializeField]
    protected GameObject infoBoxController;

    protected GameObject tempInfoBoxController;

    protected float hoverDuration = 0.5f;
    protected Coroutine hoverCoroutine;

    protected void DestroyBox()
    {
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(this.hoverCoroutine);
        }
        if (this.tempInfoBoxController != null)
        {
            Destroy(this.tempInfoBoxController);
        }
    }

    /// <summary>
    /// Displays the item's info box after a certain amount of time if player keeps mouse over item
    /// </summary>
    protected IEnumerator DisplayInfoOnHover()
    {
        yield return new WaitForSeconds(this.hoverDuration);
        if (GetBase() != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            this.tempInfoBoxController = Instantiate(infoBoxController, mousePos, Quaternion.identity);
            Info_Box_Controller controller = this.tempInfoBoxController.GetComponent<Info_Box_Controller>();
            GameObject newBox = Instantiate(this.infoBox, this.tempInfoBoxController.transform);
            Effect_Info_Box boxScript = newBox.GetComponent<Effect_Info_Box>();
            Item_Description item = GameManager.instance.STR.GetItemDescription(GetBase().GetItemType());
            boxScript.UpdateText(item);
            newBox.SetActive(true);
            controller.AddInfoBox(newBox);
        }
    }

    protected abstract Item_Base GetBase();
}
