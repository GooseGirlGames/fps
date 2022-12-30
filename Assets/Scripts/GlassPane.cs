using UnityEngine;

public class GlassPane : GiantEnemySpider {
    public GameObject Instact;
    public GameObject Fractured;

    public override void WasShot() {
        base.WasShot();
        Instact.SetActive(false);
        Fractured.SetActive(true);
    }

    void OnCollisionEnter(Collision c) {
        WasShot();
    }
}
