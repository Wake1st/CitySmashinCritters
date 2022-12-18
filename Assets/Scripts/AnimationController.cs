using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public float collapseSpeed = 1;

    private bool collapsing = false;

    private void Update() {
        if (collapsing) {
            UpdateCollapse();
        }
    }

    public void StartCollapse() {
        collapsing = true;
    }

    private void UpdateCollapse() {
        Transform parentTransform = this.transform.parent.gameObject.transform;
        float topOfParent = topOfParent = parentTransform.localPosition.y 
                + (parentTransform.transform.localScale.y);

        parentTransform.Translate(
            new Vector3(0, -1, 0) * collapseSpeed * Time.deltaTime
        );

        collapsing = topOfParent > 0;
    }
}
