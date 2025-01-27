using UnityEngine;

public class MazeCellObject : MonoBehaviour
{
    [SerializeField] private GameObject topWall;  
    [SerializeField] private GameObject bottomWall;  
    [SerializeField] private GameObject rightWall;  
    [SerializeField] private GameObject leftWall;  

    public void Init(bool top, bool bottom, bool right, bool left)
    {
        if (topWall != null) topWall.SetActive(top); 
        if (bottomWall != null) bottomWall.SetActive(bottom); 
        if (rightWall != null) rightWall.SetActive(right); 
        if (leftWall != null) leftWall.SetActive(left); 
    }
}
