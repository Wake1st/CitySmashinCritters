using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableHealthController : HealthController {
    protected override void Die() {
        this.GetComponent<AnimationController>().StartCollapse();
    }
}