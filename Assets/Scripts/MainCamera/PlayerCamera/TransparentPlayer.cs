using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject playerBody;

    [SerializeField] private Material visibleMaterial;

    [SerializeField] private Material trasparentMaterial;

    private SkinnedMeshRenderer _playerRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _playerRenderer = playerBody.GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        CharacterController playerCharContr = player.GetComponent<CharacterController>();
        Vector3 p1 = transform.position + playerCharContr.center + Vector3.up * -playerCharContr.height * 0.2F;
        Vector3 p2 = p1 + Vector3.up * playerCharContr.height;
        float playerToCamDistance = (transform.position-player.transform.position).magnitude;

        //if there something between camera and player, it swap material to a trasparent one and viceversa
        if (Physics.CapsuleCast(p1, p2, playerCharContr.radius, (player.transform.position-transform.position).normalized, out hit, playerToCamDistance))
        {
            if(hit.collider.gameObject.GetComponent<PlayerCharacter>() != null)
            {
                _playerRenderer.material=visibleMaterial;
            }
            else
            {
                _playerRenderer.material=trasparentMaterial;
            }
        }
    }
}
