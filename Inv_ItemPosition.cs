using UnityEngine;

public class Inv_ItemPosition : MonoBehaviour
{
    //Wybieranie części ciała, na której ma się pojawić obiekt.
    public enum ItemPos
    {
        Head,
        Spine,
        RightArm
    }
    public ItemPos positon;
}
