using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BuildingBlock : MonoBehaviour
{
    [SerializeField] public bool isCoreBlock = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isAttachedToCore()
    {
        return transform.root.GetComponent<BuildingBlock>().isCoreBlock;
    }
}
