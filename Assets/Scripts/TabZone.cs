using UnityEngine;

public class TabZone : MonoBehaviour
{
    private Vector2 _normalScale;
    private Vector2 _clickedScale;
    private void Start()
    {
        _normalScale = transform.localScale;
        _clickedScale = new Vector2(_normalScale.x - .05f, _normalScale.y - .05f);
    }
    private void OnMouseDown()
    {
        Game.IncreaseScore();
        transform.localScale = _clickedScale;
    }
    private void OnMouseUp()
    {
        transform.localScale = _normalScale;
    }
}
